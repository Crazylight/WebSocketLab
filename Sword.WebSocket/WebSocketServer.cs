using System;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Sword.WebSocket
{
	public class WebSocketServer
	{
		public System.Net.WebSockets.WebSocket _WebSocket { get; private set; }
		public async Task SendAsync(ArraySegment<byte> buffer, WebSocketMessageType messageType, bool endOfMessage, CancellationToken cancellationToken)
		{
			try
			{
				await _WebSocket.SendAsync(buffer, messageType, endOfMessage, cancellationToken);
			}
			catch (Exception)
			{
				await Task.CompletedTask;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="webSocket"></param>
		/// <returns></returns>
		public async Task ConnectClientAsync(System.Net.WebSockets.WebSocket webSocket)
		{
			var buffer = new byte[1024 * 4];
			WebSocketReceiveResult result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
			while (!result.CloseStatus.HasValue)
			{
				string content = Encoding.UTF8.GetString(buffer);

				await webSocket.SendAsync(new ArraySegment<byte>(buffer, 0, result.Count), result.MessageType, result.EndOfMessage, CancellationToken.None);

				result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
			}

			await webSocket.CloseAsync(result.CloseStatus.Value, result.CloseStatusDescription, CancellationToken.None);
		}

		public async Task ReceiveAsync(ArraySegment<byte> buffer, CancellationToken cancellationToken)
		{
			try
			{
				await _WebSocket.ReceiveAsync(buffer, cancellationToken);
			}
			catch (Exception)
			{
				await Task.CompletedTask;
			}
		}

		public async Task CloseAsync(WebSocketCloseStatus closeStatus, string statusDescription, CancellationToken cancellationToken)
		{
			try
			{
				await _WebSocket.CloseAsync(closeStatus, statusDescription, cancellationToken);
			}
			catch (Exception)
			{
				await Task.CompletedTask;
			}
		}

	}
}
