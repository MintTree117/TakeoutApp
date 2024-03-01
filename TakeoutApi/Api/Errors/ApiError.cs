namespace Api.Errors;

public record ApiError
{
    public ApiError( ApiErrorType errorType, string? message = null )
    {
        ErrorType = errorType;
        Message = message ?? GetDefaultMessage();
    }

    public ApiErrorType ErrorType { get; init; }
    public string Message { get; init; }

    string GetDefaultMessage()
    {
        return ErrorType switch
        {
            ApiErrorType.IoError => "An IO error occured!",
            ApiErrorType.Unauthorized => "Unauthorized!",
            ApiErrorType.NotFound => "Data not found!",
            ApiErrorType.ServerError => "Internal Server Error!",
            ApiErrorType.ValidationError => "Validation failed!",
            _ => "No message."
        };
    }
}