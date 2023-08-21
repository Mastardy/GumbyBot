using Discord.WebSocket;

namespace GumbyBot.Services
{
	[Service]
	public class DiscordService
	{
		private readonly DiscordSocketClient _client;
		private readonly CommandHandler _commandHandler;

		public DiscordService(DiscordSocketClient client, CommandHandler commandHandler)
		{
			_client = client;
			_commandHandler = commandHandler;
		}

		public async Task<bool> StartAsync()
		{
			try
			{
				// Load our .env file & fetch the token
				// Gotta use TraversePath so then we can look backwards in folders too! Helps for debugging
				DotNetEnv.Env.TraversePath().Load();
				var token = Environment.GetEnvironmentVariable("token");

				// Failed to find .env or it doesn't have a token
				if (token == null)
				{
					Console.Error.WriteLine("Failed to fetch discord app token!");
					return false;
				}

				_client.Ready += Ready;
				await _client.LoginAsync(Discord.TokenType.Bot, token);
				await _client.StartAsync();
			}
			catch (Exception ex)
			{
				Console.Error.WriteLine("Failed to start/login to discord!");
				Console.Error.WriteLine(ex.Message);
				Console.Error.WriteLine(ex?.StackTrace?.ToString());
				return false;
			}

			return true;
		}

		public async Task<bool> StopAsync()
		{
			// Shut us down
			await _client.LogoutAsync();
			await _client.StopAsync();
			return true;
		}

		private async Task Ready()
		{
			Console.WriteLine($"Logged in as user {_client.CurrentUser.Username}({_client.CurrentUser.Id})");
			await _commandHandler.RegisterCommands();
		}
	}
}
