namespace Application.Models;

public sealed class MenuOptions
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public Dictionary<int,MenuOption> Options { get; set; } = [ ];
}