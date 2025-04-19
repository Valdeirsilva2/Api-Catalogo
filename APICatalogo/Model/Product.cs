namespace APICatalogo.Model;

public class Product
{
    public int ProductId { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public decimal Value { get; set; }
    public string? ImageUrl { get; set; }
    public float Stock { get; set; }
    public DateTime RegistryDate { get; set; }

    public int CategoryId { get; set; }
    public Category? Category { get; set; }
}