using Microsoft.Extensions.Options;
using RichardSzalay.MockHttp;

namespace Client.Omdb.Tests;

public class OmdbClientTests
{
    [Fact]
    public async Task GetMoviesByTitle_Valid()
    {
        // Arrange
        var client = GetOmdbClient("search-success");

        // Act
        var response = await client.GetMoviesByTitle("spider");

        // Assert
        Assert.True(response.IsSuccessful);
        Assert.Equal(10, response.Search.Length);
        Assert.All(
            response.Search,
            item =>
            {
                Assert.Contains("spider", item.Title.ToLower());
                Assert.True(!string.IsNullOrEmpty(item.ImdbId));
                Assert.True(!string.IsNullOrEmpty(item.Type));
                Assert.True(!string.IsNullOrEmpty(item.Year));
                Assert.True(!string.IsNullOrEmpty(item.Poster));
            });
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public async Task GetMoviesByTitle_Empty(string? searchQuery)
    {
        // Arrange
        var client = GetOmdbClient("search-success");

        // Act
        var response = await client.GetMoviesByTitle(searchQuery);

        // Assert
        Assert.False(response.IsSuccessful);
        Assert.Empty(response.Search);
    }

    [Theory]
    [InlineData("search-error_to-many-results", "aaa", "Too many results.")]
    [InlineData("search-error_movie-not-found", "aaaaa", "Movie not found!")]
    public async Task GetMoviesByTitle_Error(
        string testFile,
        string? searchQuery,
        string errorText)
    {
        // Arrange
        var client = GetOmdbClient(testFile);

        // Act
        var response = await client.GetMoviesByTitle(searchQuery);

        // Assert
        Assert.False(response.IsSuccessful);
        Assert.Equal(errorText, response.Error);
    }

    [Fact]
    public async Task GetMoviesById_Valid()
    {
        // Arrange
        var client = GetOmdbClient("movie-success");

        // Act
        var response = await client.GetMoviesById("tt0145487");

        // Assert
        Assert.Equal("Spider-Man", response!.Title);
        Assert.Equal("2002", response!.Year);
        Assert.Equal("PG-13", response!.Rated);
        Assert.Equal("03 May 2002", response!.Released);
        Assert.Equal("121 min", response!.Runtime);
        Assert.Equal("Action, Adventure, Sci-Fi", response!.Genre);
        Assert.Equal("Sam Raimi", response!.Director);
        Assert.Equal("Stan Lee, Steve Ditko, David Koepp", response!.Writer);
        Assert.Equal("Tobey Maguire, Kirsten Dunst, Willem Dafoe", response!.Actors);
        Assert.Equal(
            "After being bitten by a genetically-modified spider, " +
            "a shy teenager gains spider-like abilities that he " +
            "uses to fight injustice as a masked superhero and face " +
            "a vengeful enemy.", 
            response!.Plot);
        Assert.Equal("English", response!.Language);
        Assert.Equal("United States", response!.Country);
        Assert.Equal("Nominated for 2 Oscars. 17 wins & 64 nominations total", response!.Awards);
        Assert.Equal("https://m.media-amazon.com/images/M/" +
            "MV5BZDEyN2NhMjgtMjdhNi00MmNlLWE5YTgtZGE4MzNjMTR" +
            "lMGEwXkEyXkFqcGdeQXVyNDUyOTg3Njg@._V1_SX300.jpg",
            response!.Poster);
        Assert.All(
            response.Ratings,
            x => Assert.True(!string.IsNullOrEmpty(x.Source) && !string.IsNullOrEmpty(x.Value)));
        Assert.Equal("73", response!.Metascore);
        Assert.Equal("7.4", response!.ImdbRating);
        Assert.Equal("858,136", response!.ImdbVotes);
        Assert.Equal("tt0145487", response!.ImdbId);
        Assert.Equal("movie", response!.Type);
        Assert.Equal("25 Apr 2013", response!.Dvd);
        Assert.Equal("$407,022,860", response!.BoxOffice);
        Assert.Equal("N/A", response!.Production);
        Assert.Equal("N/A", response!.Website);
        Assert.Equal("True", response!.Response);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public async Task GetMoviesById_Empty(
        string? searchQuery)
    {
        // Arrange
        var client = GetOmdbClient("movie-error_wrong-id");

        // Act
        var response = await client.GetMoviesById(searchQuery);

        // Assert
        Assert.Null(response);
    }

    [Fact]
    public async Task GetMoviesById_Error()
    {
        // Arrange
        var client = GetOmdbClient("movie-error_wrong-id");

        // Act
        var response = await client.GetMoviesById("incorrectId");

        // Assert
        Assert.False(response?.IsSuccessful);
        Assert.Equal("Incorrect IMDb ID.", response?.Error);
    }

    private OmdbClient GetOmdbClient(string testDataFileName)
    {
        var mockHttp = new MockHttpMessageHandler();

        mockHttp.When("https://www.omdbapi.com/*")
                .Respond("application/json", File.ReadAllText($"TestData/{testDataFileName}.json"));

        var client = new OmdbClient(
            Options.Create(new OmdbClientOptions()
            {
                Url = "https://www.omdbapi.com/"
            }),
            mockHttp.ToHttpClientFactory());

        return client;
    }
}