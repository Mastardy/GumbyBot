using Discord.WebSocket;

namespace GumbyBot
{
	public class Program
	{
		private DiscordSocketClient? _client = null;

		public static Task Main(string[] args) => new Program().MainAsync(args);

		public async Task MainAsync(string[] args)
		{
			// Load our .env file & fetch the token
			// Gotta use TraversePath so then we can look backwards in folders too! Helps for debugging
			DotNetEnv.Env.TraversePath().Load();
			var token = Environment.GetEnvironmentVariable("token");

			// Failed to find .env or it doesn't have a token
			if(token == null)
			{
				Console.Error.WriteLine("Failed to fetch discord app token!");
				return;
			}

			try
			{
				_client = new();

				// When we connect to discord, just verify shit works
				_client.Connected += Task () => {
					Console.WriteLine($"Logged in as user {_client.CurrentUser.Username}({_client.CurrentUser.Id})");
					return Task.CompletedTask;
				};

				// Login
				await _client.LoginAsync(Discord.TokenType.Bot, token);
				await _client.StartAsync();
			}
			catch (Exception ex) {
				Console.Error.WriteLine("Failed to start/login to discord!");
				Console.Error.WriteLine(ex.Message);
				Console.Error.WriteLine(ex?.StackTrace?.ToString());
			}

			// Wait forever
			await Task.Delay(-1);
		}

	}
}