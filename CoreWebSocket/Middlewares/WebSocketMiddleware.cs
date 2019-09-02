using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Sword.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CoreWebSocket.Middlewares
{
	public class WebSocketMiddleware
	{
		RequestDelegate _next;
		ILogger _logger;
		public WebSocketMiddleware(RequestDelegate next, ILogger<WebSocketMiddleware> logger)
		{
			_next = next;
			_logger = logger;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="context"></param>
		/// <returns></returns>
		public async Task Invoke(HttpContext context)
		{
			if (context.Request.Path == "/ws")
			{
				if (context.WebSockets.IsWebSocketRequest)
				{
					Console.WriteLine(context.Request.Path);

					_logger.LogInformation($"收到了来自{context.Request.QueryString}的请求");

					//dosomething
					WebSocketSession session = new WebSocketSession();
					session._WebSocket = await context.WebSockets.AcceptWebSocketAsync().ConfigureAwait(false);
					WebSocketFactory.Instance.AddSession(session);

					////触发要发送的内容
					////await SendText(context, session);
					await ProtectTheSession(context, session);
				}
				else
				{
					context.Response.StatusCode = 400;
				}
			}
			else
			{
				await _next.Invoke(context);
			}
		}

		public async Task SendText(HttpContext context, WebSocketSession session)
		{
			while (session.IsConnect())
			{

				var message = new List<byte>();
				WebSocketReceiveResult webSocketReceiveResult;
				var buffer = new byte[1024 * 4];

				do
				{
					webSocketReceiveResult = await session._WebSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);

					message.AddRange(new ArraySegment<byte>(buffer, 0, webSocketReceiveResult.Count));
				} while (!webSocketReceiveResult.EndOfMessage);


				var content = Encoding.UTF8.GetString(message.ToArray());
				Console.WriteLine($"发送：{content}");
				await session.SendTextAsync("我收到了信息:" + content);
				switch (content)
				{
					case "1":
						await session.SendTextAsync("First 1:");
						break;
					case "2":
						int count = 0;
						while (count < 10)
						{
							await session.SendTextAsync("报道：" + count);
							Thread.Sleep(1000);
							count++;
						}
						break;
					default:
						break;
				}

			}
		}

		public async Task ProtectTheSession(HttpContext context, WebSocketSession session)
		{
			try
			{
				WebSocketReceiveResult webSocketReceiveResult = null;
				byte[] buffer = new byte[1024];
				while (session.IsConnect())
				{
					if (context.RequestAborted.IsCancellationRequested)
						break;
					if (session._WebSocket.State != WebSocketState.Open)
						break;
					using (var cts = new CancellationTokenSource(WebSocketSession.KeepAliveTimeout))//最起码5秒能收到一个心跳包
					{
						webSocketReceiveResult = await session._WebSocket.ReceiveAsync(new ArraySegment<byte>(buffer), cts.Token);
					}
					if (webSocketReceiveResult.MessageType == WebSocketMessageType.Close)
					{
						_logger.LogInformation($"关闭会话,SessionID={session.SessionID},ClientAddr={context.Connection.RemoteIpAddress}");

						Sword.WebSocket.WebSocketFactory.Instance.RemoveSession(session);
						return;
					}

					var message = new List<byte>();

					do
					{
						webSocketReceiveResult = await session._WebSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);

						message.AddRange(new ArraySegment<byte>(buffer, 0, webSocketReceiveResult.Count));
					} while (!webSocketReceiveResult.EndOfMessage);

					var content = Encoding.UTF8.GetString(message.ToArray());
					switch (content)
					{
						case "1":
							session.SubMsgType(1);
							break;
						case "2":
							session.SubMsgType(2);
							break;
						default:
							break;
					}
					Sword.WebSocket.WebSocketFactory.Instance.AddSession(session);
					string stopTest = $"{session.SessionID}连接{content}：";
					_logger.LogInformation(stopTest);
					Console.WriteLine(stopTest);
				}

				Sword.WebSocket.WebSocketFactory.Instance.RemoveSession(session);

			}
			catch (System.OperationCanceledException e)
			{
				string text = e.ToString();
				_logger.LogInformation(text);
				Sword.WebSocket.WebSocketFactory.Instance.RemoveSession(session);
				Console.WriteLine(text);
			}

			catch (WebSocketException e)
			{
				string text = e.ToString();
				_logger.LogInformation(text);
				Sword.WebSocket.WebSocketFactory.Instance.RemoveSession(session);
				Console.WriteLine(text);
				//The remote party closed the WebSocket connection without completing the close handshake.
			}
			catch (Exception e)
			{
				string text = e.ToString();
				_logger.LogInformation(text);
				Console.WriteLine(text);
			}
		}


	}
}
