using Discord;
using Discord.Interactions;

namespace GumbyBot.Modules
{
	public class Utility : InteractionModuleBase<SocketInteractionContext>
	{
		[SlashCommand("ping", "Respond with a pong to make sure everything works")]
		public async Task PingCommand()
		{
			var emoji = new Emoji("\uD83C\uDFD3");
			await RespondAsync($"{emoji} Pong!");
		}
	}
}
