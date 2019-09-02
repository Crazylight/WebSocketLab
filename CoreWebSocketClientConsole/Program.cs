using System;
using System.Threading;
using System.Threading.Tasks;

namespace CoreWebSocketClientConsole
{
	class Program
	{
		//static async Task Main(string[] args)
		//{
		//	System.Net.Http.HttpClient http = new System.Net.Http.HttpClient();

		//	string text = await http.GetStringAsync("http://www.baidu.com");

		//	Console.WriteLine(text);
		//	Console.Read();
		//}
		static void Main(string[] args)
		{
			Sword.WebSocket.WebSocketClient client = new Sword.WebSocket.WebSocketClient();

			client.Connect().GetAwaiter();

			//Task.Run(() =>
			//{
			//	while (true)
			//	{
			var res = client.SendTextAsync("2").IsCompleted;

			//		Console.WriteLine("Send:");
			//		Thread.Sleep(5000);
			//	}
			//});


			//while (true)
			{
				//client.SendTextAsync("1").GetAwaiter();
				//client.ReceiveText();
				//string text = client.ReceiveTextAsync().Result;
				client.Receieve();
				//client.ReceiveTextAsync();
				//text = client.ReceiveText();
				Console.WriteLine("Hello World!");
				//client._clientWebSocket.CloseAsync(System.Net.WebSockets.WebSocketCloseStatus.Empty, "", System.Threading.CancellationToken.None);
				System.Threading.Thread.Sleep(5000);
				//Console.WriteLine("End World!");
				Console.Read();
			}
		}
	}
}
