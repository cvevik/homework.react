using System.Text.Json.Serialization;

public class Movie
{
    public string Title { get; set; } = string.Empty;

    public string Year { get; set; } = string.Empty;

    public string Poster { get; set; } = string.Empty;

    [JsonPropertyName("imdbID")]
    public string ImdbId { get; set; } = string.Empty;

    public string Type { get; set; } = string.Empty;
}