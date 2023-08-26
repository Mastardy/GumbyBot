using System.Text.Json.Serialization;

namespace GumbyBot.Models
{
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
