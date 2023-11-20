public class ResultOfGetMovieById : MovieWithDescription, IResult
{
    public string Response { get; set; } = string.Empty;

    public bool IsSuccessful => Response.ToLower() == "true";

    public string Error { get; set; } = string.Empty;
}
