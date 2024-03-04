namespace Application.Services;

public enum ServiceErrorType
{
    None,
    IoError,
    ValidationError,
    NotFound,
    Unauthorized,
    Conflict,
    ServerError
}

public record ApiReply<T>
{
    const string MESSAGE_RESPONSE_ERROR = "Failed to produce a proper response message!";

    public ApiReply()
    {

    }

    public ApiReply( ServiceErrorType errorType, string? message = null )
    {
        Data = default;
        Success = false;
        ErrorType = errorType;
        Message = message ?? GetDefaultMessage( errorType );
    }

    public ApiReply( T data )
    {
        Data = data;
        Success = true;
        Message = string.Empty;
    }

    public bool Success { get; init; }
    public string Message { get; init; } = string.Empty;
    public T? Data { get; init; }
    public ServiceErrorType ErrorType { get; init; } = ServiceErrorType.None;

    public string Details()
    {
        return $"{ErrorType} : {Message}";
    }

    static string GetDefaultMessage( ServiceErrorType errorType )
    {
        return errorType switch
        {
            ServiceErrorType.IoError => "An IO error occured!",
            ServiceErrorType.Unauthorized => "Unauthorized!",
            ServiceErrorType.NotFound => "Data not found!",
            ServiceErrorType.ServerError => "Internal Server Error!",
            ServiceErrorType.ValidationError => "Validation failed!",
            _ => MESSAGE_RESPONSE_ERROR
        };
    }
}