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
				var token = Environment.GetEnvironmentVariable("TOKEN");

				// Failed to find the token
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
				await Console.Error.WriteLineAsync("Failed to start/login to discord!");
				await Console.Error.WriteLineAsync(ex.Message);
				await Console.Error.WriteLineAsync(ex?.StackTrace);
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
