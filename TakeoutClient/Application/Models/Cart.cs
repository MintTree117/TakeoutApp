namespace Application.Models;

public class Cart
{
    public List<string> Items { get; set; } = [ ];

    public decimal GetPrice()
    {
        return 0;
    }
}