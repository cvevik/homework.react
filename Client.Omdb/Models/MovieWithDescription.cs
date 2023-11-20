using System.Text.Json.Serialization;

public class MovieWithDescription : Movie
{
    public string Rated { get; set; } = string.Empty;

    public string Released { get; set; } = string.Empty;

    public string Runtime { get; set; } = string.Empty;

    public string Genre { get; set; } = string.Empty;

    public string Director { get; set; } = string.Empty;

    public string Writer { get; set; } = string.Empty;

    public string Actors { get; set; } = string.Empty;

    public string Plot { get; set; } = string.Empty;

    public string Language { get; set; } = string.Empty;

    public string Country { get; set; } = string.Empty;

    public string Awards { get; set; } = string.Empty;

    public Rating[] Ratings { get; set; } = Array.Empty<Rating>();

    public string Metascore { get; set; } = string.Empty;

    [JsonPropertyName("imdbRating")]
    public string ImdbRating { get; set; } = string.Empty;

    [JsonPropertyName("imdbVotes")]
    public string ImdbVotes { get; set; } = string.Empty;

    [JsonPropertyName("DVD")]
    public string Dvd { get; set; } = string.Empty;

    public string BoxOffice { get; set; } = string.Empty;

    public string Production { get; set; } = string.Empty;

    public string Website { get; set; } = string.Empty;
}
