using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CoreWebSocket.Middlewares;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;
using Microsoft.Extensions.Options;
using Sword.WebSocket;
using NLog.Web;

namespace CoreWebSocket
{
	public class Startup
	{
		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{
			services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory logger)
		{
			logger.AddNLog();
			env.ConfigureNLog("Config/nlog.config"); //注册日志

			if (env.IsDevelopment())

			{
				app.UseDeveloperExceptionPage();
			}
			//else
			//{
			//	// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
			//	app.UseHsts();
			//}

			app.UseWebSockets();
			app.UseMiddleware<WebSocketMiddleware>();

			app.UseHttpsRedirection();
			app.UseMvc();
		}

		//private async Task Echo(WebSocket webSocket)
		//{
		//	var buffer = new byte[1024 * 4];
		//	WebSocketReceiveResult result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
		//	while (!result.CloseStatus.HasValue)
		//	{
		//		var content = Encoding.UTF8.GetString(buffer);
		//		Console.WriteLine(result.MessageType);
		//		await webSocket.SendAsync(new ArraySegment<byte>(buffer, 0, result.Count), result.MessageType, result.EndOfMessage, CancellationToken.None);

		//		result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
		//	}
		//	await webSocket.CloseAsync(result.CloseStatus.Value, result.CloseStatusDescription, CancellationToken.None);
		//}
	}
}
