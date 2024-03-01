namespace Api.Errors;

public record ApiException : ApiError
{
    public ApiException( ApiErrorType errorType, string? message = null, string? details = null ) : base( errorType, message )
    {
        Details = details ?? "No exception details were given...";
    }

    public string Details { get; init; }
}