using Discord;
using Discord.Interactions;
using Discord.WebSocket;
using System.Reflection;

namespace GumbyBot.Services
{
	[Service]
	public class CommandHandler
	{
		private readonly DiscordSocketClient _client;
		private readonly InteractionService _interactionService;
		private readonly IServiceProvider _serviceProvider;

		public CommandHandler(DiscordSocketClient client, InteractionService interactionService, IServiceProvider serviceProvider)
		{
			_client = client;
			_interactionService = interactionService;
			_serviceProvider = serviceProvider;
		}

		public async Task<bool> StartAsync()
		{
			// Register all of our commands
			await _interactionService.AddModulesAsync(Assembly.GetEntryAssembly(), _serviceProvider);
			
			// Listen for interactions
			_client.InteractionCreated += HandleInteraction;
			_interactionService.SlashCommandExecuted += SlashCommandExecuted;

			return true;
		}

		public async Task RegisterCommands()
		{
			var guildIdStr = Environment.GetEnvironmentVariable("guild_id");
			if (guildIdStr == null)
			{
				// This can take up to an hour to take effect, so it's probably better to use specific guild for testing :)
				await _interactionService.RegisterCommandsGloballyAsync();
			}
			else
			{
				await _interactionService.RegisterCommandsToGuildAsync(ulong.Parse(guildIdStr));
			}

		}

		private async Task SlashCommandExecuted(SlashCommandInfo arg1, Discord.IInteractionContext arg2, IResult arg3)
		{
			// TODO: Handle error states

		}

		private async Task HandleInteraction(SocketInteraction arg)
		{
			try
			{
				var ctx = new SocketInteractionContext(_client, arg);
				await _interactionService.ExecuteCommandAsync(ctx, _serviceProvider);
			}
			catch (Exception ex)
			{
				Console.Error.WriteLine(ex);
				if (arg.Type == InteractionType.ApplicationCommand)
				{
					await arg.GetOriginalResponseAsync().ContinueWith(async (msg) => await msg.Result.DeleteAsync());
				}
			}
		}
	}
}
