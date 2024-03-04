namespace Application.Models;

public sealed record User
{
    public string Email { get; init; } = string.Empty;
    public string Username { get; init; } = string.Empty;
    public string Token { get; init; } = string.Empty;
    public bool IsAdmin { get; init; } = false;
}