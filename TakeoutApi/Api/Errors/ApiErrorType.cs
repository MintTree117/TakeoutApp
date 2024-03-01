namespace Api.Errors;

public enum ApiErrorType
{
    None,
    IoError,
    ValidationError,
    NotFound,
    Unauthorized,
    Conflict,
    ServerError
}