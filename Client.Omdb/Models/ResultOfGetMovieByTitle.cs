public class ResultOfGetMovieByTitle : Result
{
    public Movie[] Search { get; set; } = Array.Empty<Movie>();

    public int TotalResults {  get; set; }
}
