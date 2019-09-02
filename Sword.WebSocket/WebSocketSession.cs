using System;
using System.Collections.Generic;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Sword.WebSocket
{
	public class WebSocketSession
	{
		public WebSocketSession()
		{
			SessionID = Guid.NewGuid().ToString();
		}

		/// <summary>
		/// 订阅的消息类型集合
		/// </summary>
		List<int> sumMsgTypeList = new List<int>();

		public void SubMsgType(params int[] msgTypelist)
		{
			if (msgTypelist == null || msgTypelist.Length == 0)
			{
				sumMsgTypeList = new List<int>();
				return;
			}

			foreach (var msgType in msgTypelist)
			{
				if (!sumMsgTypeList.Contains(msgType))
				{
					sumMsgTypeList.Add(msgType);
				}
			}
		}

		public void cancleSubMsgType(params int[] msgTypelist)
		{
			if (msgTypelist == null || msgTypelist.Length == 0)
			{
				return;
			}

			foreach (var msgType in msgTypelist)
			{
				if (sumMsgTypeList.Contains(msgType))
				{
					sumMsgTypeList.Remove(msgType);
				}
			}
		}

		/// <summary>
		/// 心跳超时时间
		/// </summary>
		public const int KeepAliveTimeout = 30000;

		public bool IsSub(int subType)
		{
			return sumMsgTypeList.Contains(subType);
		}

		public bool IsConnect()
		{
			if (_WebSocket == null)
			{
				return false;
			}

			if (_WebSocket.State == System.Net.WebSockets.WebSocketState.Open)
			{
				return true;
			}

			return false;
		}

		public string SessionID { get; set; }

		public System.Net.WebSockets.WebSocket _WebSocket { get; set; }

		public bool SendBytes(byte[] data)
		{
			return _WebSocket.SendAsync(new ArraySegment<byte>(data, 0, data.Length), System.Net.WebSockets.WebSocketMessageType.Binary, true, CancellationToken.None).IsCompleted;
		}

		public bool SendText(string text)
		{
			byte[] data = Encoding.UTF8.GetBytes(text);
			return _WebSocket.SendAsync(new ArraySegment<byte>(data, 0, data.Length), System.Net.WebSockets.WebSocketMessageType.Text, true, CancellationToken.None).IsCompleted;
		}


		public async Task SendAsync(ArraySegment<byte> buffer)
		{
			try
			{
				await _WebSocket.SendAsync(buffer, System.Net.WebSockets.WebSocketMessageType.Binary, true, CancellationToken.None);
			}
			catch (Exception)
			{
				await Task.CompletedTask;
			}
		}

		public async Task SendTextAsync(string text)
		{
			try
			{
				if (!_WebSocket.CloseStatus.HasValue)
				{
					byte[] data = Encoding.UTF8.GetBytes(text);
					await _WebSocket.SendAsync(new ArraySegment<byte>(data, 0, data.Length), System.Net.WebSockets.WebSocketMessageType.Text, true, CancellationToken.None);
				}
			}
			catch (Exception)
			{
				await Task.CompletedTask;
			}
		}


		public void Dispose()
		{
			if (this._WebSocket == null)
			{
				return;
			}
			try
			{
				this._WebSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "服务端释放", CancellationToken.None).Wait();
			}
			catch
			{
			}
		}

	}
}
