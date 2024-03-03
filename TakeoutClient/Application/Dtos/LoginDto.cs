using System.ComponentModel.DataAnnotations;

namespace Application.Dtos;

public sealed record LoginDto
{
    [Required] public string EmailOrUsername { get; set; } = string.Empty;
    [Required] public string Password { get; set; } = string.Empty;
}