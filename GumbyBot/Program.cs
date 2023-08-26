using Discord.Interactions;
using Discord.WebSocket;
using GumbyBot.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace GumbyBot
{
	public class Program
	{
		private readonly IServiceProvider _serviceProvider;

		public Program()
		{
			_serviceProvider = CreateProvider();
		}

		static IServiceProvider CreateProvider()
		{
			var collection = new ServiceCollection()
				.AddSingleton<DiscordSocketClient>()
				.AddSingleton<InteractionService>()
				.AddDbContextPool<DatabaseContext>(options =>
					options.UseSqlite(DatabaseContext.ConnectionString));

			// Get all classes with the "service/module" attribute
			var services = Assembly.GetExecutingAssembly().GetTypes().Where(t => t.IsDefined(typeof(ServiceAttribute)));

			// Add to our provider
			foreach (var service in services)
				collection = collection.AddSingleton(service);

			return collection.BuildServiceProvider();
		}

		public static Task Main() => new Program().MainAsync();

		public async Task MainAsync()
		{
			var dbContext = _serviceProvider.GetRequiredService<DatabaseContext>();
			await dbContext.Database.MigrateAsync();
			await dbContext.Database.EnsureCreatedAsync();
			try
			{
				await (dbContext?.Database?.GetService<IDatabaseCreator>() as RelationalDatabaseCreator)?.CreateTablesAsync();
			} catch(Exception _)
			{

			}

			// Start bot
			var client = _serviceProvider.GetRequiredService<DiscordService>();
			await client.StartAsync();

			// Setup slash commands
			var commandHandler = _serviceProvider.GetRequiredService<CommandHandler>();
			await commandHandler.StartAsync();

			// Wait forever
			await Task.Delay(-1);
		}

	}
}