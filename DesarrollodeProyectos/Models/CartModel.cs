using DesarrollodeProyectos.Models;

public class CartItem
{
    public Guid ProductId { get; set; }
    public string ProductType { get; set; }
    public int Quantity { get; set; }
    public decimal Price { get; set; }
    public string Name { get; set; }
}