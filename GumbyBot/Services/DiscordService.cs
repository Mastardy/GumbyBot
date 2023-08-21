using Discord.WebSocket;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace GumbyBot.Services
{
	[Service]
	public class DiscordService
	{
		private readonly DiscordSocketClient _client;

		public DiscordService(DiscordSocketClient client)
		{
			_client = client;
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

				_client.Ready += client_Ready;
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
			await _client.LogoutAsync();
			await _client.StopAsync();
			return true;
		}

		private Task client_Ready()
		{
			Console.WriteLine($"Logged in as user {_client.CurrentUser.Username}({_client.CurrentUser.Id})");
			return Task.CompletedTask;
		}
	}
}
