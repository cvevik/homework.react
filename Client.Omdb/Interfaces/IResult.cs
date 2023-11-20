public interface IResult
{
    string Response { get; set; }

    bool IsSuccessful { get; }

    string Error { get; set; }

}
