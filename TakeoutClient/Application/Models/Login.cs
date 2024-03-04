using System.ComponentModel.DataAnnotations;

namespace Application.Models;

public sealed record Login
{
    [Required] public string EmailOrUsername { get; set; } = string.Empty;
    [Required] public string Password { get; set; } = string.Empty;
}