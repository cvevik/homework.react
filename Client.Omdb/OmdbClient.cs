using Microsoft.Extensions.Options;
using System.Text.Json;

public class OmdbClient
{
    private readonly OmdbClientOptions _options;
    private readonly IHttpClientFactory _httpClientFactory;

    public OmdbClient(
        IOptions<OmdbClientOptions> options,
        IHttpClientFactory httpClientFactory) =>
        (_options, _httpClientFactory) = (options.Value, httpClientFactory);

    public Task<ResultOfGetMovieByTitle> GetMoviesByTitle(string? title)
    {
        if (string.IsNullOrWhiteSpace(title))
            return Task.FromResult(new ResultOfGetMovieByTitle());

        return Get<ResultOfGetMovieByTitle>("s", title);
    }

    public Task<ResultOfGetMovieById?> GetMoviesById(string? id)
    {
        if (string.IsNullOrWhiteSpace(id))
            return Task.FromResult(default(ResultOfGetMovieById?));

        return Get<ResultOfGetMovieById?>("i", id);
    }

    private async Task<T> Get<T>(string param, string? query)
    {
        var client = _httpClientFactory.CreateClient();

        using var response = await client.GetAsync($"{_options.Url}&{param}={query}");

        // TODO: Handle errors and throw named exceptions
        response.EnsureSuccessStatusCode();

        var stream = await response.Content.ReadAsStreamAsync();
        var data = await JsonSerializer.DeserializeAsync<T>(stream);

        return data!;
    }
}