using Discord.WebSocket;
using GumbyBot.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Reflection;

namespace GumbyBot
{
	public class Program
	{
		private DiscordSocketClient? _client = null;
		private readonly IServiceProvider _serviceProvider;

		public Program()
		{
			_serviceProvider = CreateProvider();
		}

		static IServiceProvider CreateProvider()
		{
			var collection = new ServiceCollection()
				.AddSingleton<DiscordSocketClient>();

			// Get all classes with the "service" attribute
			var services = Assembly.GetExecutingAssembly().GetTypes().Where(t => t.IsDefined(typeof(ServiceAttribute)));
			// Add to our provider
			foreach(var service in services)
				collection = collection.AddSingleton(service);

			return collection.BuildServiceProvider();
		}

		public static Task Main(string[] args) => new Program().MainAsync(args);

		public async Task MainAsync(string[] args)
		{
			// Start bot
			var client = _serviceProvider.GetRequiredService<DiscordService>();
			await client.StartAsync();

			// Wait forever
			await Task.Delay(-1);
		}

	}
}