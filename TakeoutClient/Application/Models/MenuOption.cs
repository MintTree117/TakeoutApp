namespace Application.Models;

public sealed class MenuOption
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
}