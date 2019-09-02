using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Sword.WebSocket
{
	/// <summary>
	/// 负责给Session发送Msg
	/// </summary>
	public class SessionMsgServerSender
	{
		public static SessionMsgServerSender Instance = new SessionMsgServerSender();
		private SessionMsgServerSender()
		{

		}

		List<WebSocketSession> sessionList = new List<WebSocketSession>();

		public void AddSessionMsg(WebSocketSession session)
		{
			sessionList.Add(session);
		}

		public void StartThread()
		{
			{
				Thread thread = new Thread(ExecuteThread1);
				thread.Start();
			}
			{
				Thread thread = new Thread(ExecuteThread2);
				thread.Start();
			}
		}

		void ExecuteThread1()
		{
			Console.WriteLine("Thread 1");
			while (true)
			{
				try
				{
					var list = WebSocketFactory.Instance.GetIsSubSessionList(1);

					foreach (var session in list)
					{
						session.SendText("发送消息1");
					}

					Thread.Sleep(1000);
					//list.AsParallel().WithDegreeOfParallelism(5).ForAll((session) =>
					//{
					//	try
					//	{
					//		var count = Math.Ceiling((data.Count * 1.0) / BATCH_SEND_COUNT);
					//		for (int i = 0; i < count; i++)
					//		{
					//			var message = new KlineDataResp();
					//			message.Data = data.Skip(i * BATCH_SEND_COUNT).Take(BATCH_SEND_COUNT).ToList();
					//			session.Send(message);
					//		}
					//	}
					//	catch (Exception ex)
					//	{
					//		this.LogError(ex.ToString());
					//	}
					//});
				}
				catch (Exception e)
				{


				}
			}
		}


		void ExecuteThread2()
		{
			Console.WriteLine("Thread 2");
			while (true)
			{
				try
				{
					var list = WebSocketFactory.Instance.GetIsSubSessionList(2);

					foreach (var session in list)
					{

						session.SendText("发送消息2");
					}
					Thread.Sleep(1000);
					//list.AsParallel().WithDegreeOfParallelism(5).ForAll((session) =>
					//{
					//	try
					//	{
					//		var count = Math.Ceiling((data.Count * 1.0) / BATCH_SEND_COUNT);
					//		for (int i = 0; i < count; i++)
					//		{
					//			var message = new KlineDataResp();
					//			message.Data = data.Skip(i * BATCH_SEND_COUNT).Take(BATCH_SEND_COUNT).ToList();
					//			session.Send(message);
					//		}
					//	}
					//	catch (Exception ex)
					//	{
					//		this.LogError(ex.ToString());
					//	}
					//});
				}
				catch (Exception e)
				{


				}
			}
		}
	}
}
