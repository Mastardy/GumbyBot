using Discord;
using Discord.Interactions;
using GumbyBot.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace GumbyBot.Modules
{
	public class CatApi : InteractionModuleBase<SocketInteractionContext>
	{
		private readonly AsyncHttpWebRequest _httpClient;
		public CatApi(AsyncHttpWebRequest httpClient)
		{
			_httpClient = httpClient;
		}

		[SlashCommand("cat", "Get a picture of a cat")]
		public async Task CatCommand([
			Choice("Persian", "pers"),
			Choice("Siamese", "siam"),
			Choice("Maine Coon", "mcoo"),
			Choice("Ragdoll", "ragd"),
			Choice("Shorthair", "asho"),
			Choice("Bengal", "beng"),
			Choice("Sphynx", "sphy"),
			Choice("Scottish Fold", "sfol"),
			Choice("Russian Blue", "rblu"),
			Choice("Siberian", "sibe"),
			Choice("Himalayan", "hima"),
			Choice("Japanese Bobtail", "jbob"),
			] string breed = "any")
		{
			CatModel? cat;
			if (breed == "any" || breed == null) cat = await GetCatByBreed();
			else cat = await GetCatByBreed(breed);

			// TODO: Gracefully exit
			if (cat == null) return;

			await RespondAsync(embed: new EmbedBuilder
			{
				Title = "Meow! I'm a cat",
				ImageUrl = cat.URL
			}.Build());
		}

		private async Task<CatModel?> GetCatByBreed(string? breed_id = null)
		{
			try
			{
				string paramsString = breed_id != null ? $"?breed_ids={breed_id}" : string.Empty;
				var responseText = await _httpClient.String(HttpMethod.Get, $"https://api.thecatapi.com/v1/images/search{paramsString}");
				return JsonSerializer.Deserialize<List<CatModel>>(responseText)?.First();
			}
			catch (Exception _)
			{
				// TODO
				// Gracefully handle this
			}
			return null;
		}
	}

	public class CatModel
	{
		[JsonPropertyName("id")]
		public string? ID { get; set; }

		[JsonPropertyName("url")]
		public string? URL { get; set; }

		[JsonPropertyName("width")]
		public int Width { get; set; }

		[JsonPropertyName("height")]
		public int Height { get; set; }

	}
}
