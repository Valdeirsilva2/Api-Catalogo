using System.Collections.ObjectModel;

namespace APICatalogo.Model;

public class Category
{
    //Boa prativa inicializar uma coleção 
    public Category()
    {
        Products = new Collection<Product>();
    }
    
    public int CategoriaId { get; set; }
    public string? Nome { get; set; }
    public string? ImagemUrl { get; set; }
    
    //Adcionando relacionamento de entre categoria e produto
    public ICollection<Product>? Products { get; set; }
}