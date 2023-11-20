using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("[controller]")]
public class MoviesController : ControllerBase
{
    private readonly ILogger<MoviesController> _logger;
    private readonly OmdbClient _omdbClient;
    private readonly static List<Movie> _lastMovies = new();

    public MoviesController(
        ILogger<MoviesController> logger,
        OmdbClient omdbClient)
    {
        _logger = logger;
        _omdbClient = omdbClient;
    }

    [HttpGet]
    public async Task<Movie[]> GetByTitle(string? title)
    {
        if (string.IsNullOrWhiteSpace(title))
            return _lastMovies.ToArray();

        // TODO: Use separate models for responses
        var movies = await _omdbClient.GetMoviesByTitle(title);

        return movies.Search;
    }

    [HttpGet("{id}")]
    public async Task<MovieWithDescription?> GetById(string id)
    {
        // TODO: Use separate models for responses
        var movie = await _omdbClient.GetMoviesById(id);

        if (movie != null)
        {
            if (_lastMovies.All(x => x.ImdbId != movie.ImdbId))
            {
                _lastMovies.Add(movie);

                if (_lastMovies.Count > 5)
                    _lastMovies.RemoveAt(0);
            }
        }

        return movie;
    }
}
