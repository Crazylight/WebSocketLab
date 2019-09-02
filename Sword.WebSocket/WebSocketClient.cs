using System;
using System.Collections.Generic;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Sword.WebSocket
{
	public class WebSocketClient
	{
		public System.Net.WebSockets.ClientWebSocket _clientWebSocket { get; private set; }

		public WebSocketClient()
		{
			_clientWebSocket = new System.Net.WebSockets.ClientWebSocket();
			//_clientWebSocket.SubProtocol = "arraybuffer";
		}
		public void Test()
		{

			//_clientWebSocket.SendAsync();
		}

		public async Task Connect(string url = "ws://127.0.0.1:5000/ws?clientid=192.168.6.18&groupid=usquote")
		{
			await _clientWebSocket.ConnectAsync(new Uri(url), cancellationToken: CancellationToken.None);

		}


		public async Task SendByteAsync(byte[] data)
		{
			ArraySegment<byte> seg = new ArraySegment<byte>(data);

			await _clientWebSocket.SendAsync(seg, System.Net.WebSockets.WebSocketMessageType.Binary, true, CancellationToken.None);

		}

		public async Task SendTextAsync(string reqTest)
		{
			int count = 0;
			while (count < 10)
			{
				if (_clientWebSocket.State == WebSocketState.Connecting)
				{
					Thread.Sleep(1000);
				}
				count++;
			}

			if (_clientWebSocket.State == WebSocketState.Open)
			{
				var reqByte = Encoding.UTF8.GetBytes(reqTest);
				ArraySegment<byte> seg = new ArraySegment<byte>(reqByte);

				await _clientWebSocket.SendAsync(seg, System.Net.WebSockets.WebSocketMessageType.Text, true, CancellationToken.None);
			}
		}

		public async Task Receieve()
		{
			var message = new List<byte>();
			var buffer = new byte[1024];
			while (true)
			{
				try
				{
					if (_clientWebSocket.State == WebSocketState.Connecting)
					{
						Thread.Sleep(1000);
						continue;
					}
					await SendTextAsync("2");
					WebSocketReceiveResult result;
					do
					{

						ArraySegment<byte> receSeg = new ArraySegment<byte>(buffer);
						//result = await _clientWebSocket.ReceiveAsync(receSeg, CancellationToken.None);
						result = await _clientWebSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);

						//webSocketReceiveResult = await session._WebSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);

						message.AddRange(new ArraySegment<byte>(buffer, 0, result.Count));
					} while (!result.EndOfMessage);

					string tesxt = Encoding.UTF8.GetString(message.ToArray()).Trim();

					Console.WriteLine(tesxt);
				}
				catch (Exception e)
				{
					Thread.Sleep(1000);
				}
			}
		}

		public string ReceiveText()
		{
			var buffer = new byte[1024 * 4];
			ArraySegment<byte> seg = new ArraySegment<byte>(buffer);

			WebSocketReceiveResult result;
			do
			{
				result = _clientWebSocket.ReceiveAsync(seg, CancellationToken.None).Result;


			}
			while (result.EndOfMessage);

			string text = Encoding.UTF8.GetString(seg.Array);
			return text;
		}
		public async Task<string> ReceiveTextAsync()
		{
			ArraySegment<byte> seg = new ArraySegment<byte>();


			WebSocketReceiveResult result;
			do
			{
				result = await _clientWebSocket.ReceiveAsync(seg, CancellationToken.None);


			}
			while (result.EndOfMessage);

			string text = Encoding.UTF8.GetString(seg.Array);
			return text;
		}
	}
}
