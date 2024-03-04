using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Application.Models;

public sealed record Register
{
    [Required, EmailAddress]
    public string Email { get; set; } = string.Empty;
    [Required, StringLength( 25, MinimumLength = 6 )]
    public string Username { get; set; } = string.Empty;
    [Required, StringLength( 100, MinimumLength = 6 )]
    public string Password { get; set; } = string.Empty;
    [JsonIgnore, Compare( "Password", ErrorMessage = "Passwords do not match!" )]
    public string PasswordConfirm { get; set; } = string.Empty;
}