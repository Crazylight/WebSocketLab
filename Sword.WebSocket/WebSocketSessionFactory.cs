using System;
using System.Collections.Generic;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Sword.WebSocket
{
	public class WebSocketFactory
	{
		private WebSocketFactory()
		{

		}
		public static WebSocketFactory Instance = new WebSocketFactory();
		private List<WebSocketSession> _SessionList = new List<WebSocketSession>();

		public void AddSession(WebSocketSession session)
		{
			_SessionList.Add(session);
		}
		public void RemoveSession(WebSocketSession session)
		{
			_SessionList.Remove(session);
		}

		public List<WebSocketSession> GetIsSubSessionList(int subType)
		{
			return _SessionList.FindAll(o => o.IsSub(subType));
		}
	}
}

