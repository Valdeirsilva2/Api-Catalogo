using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace APICatalogo.Model;

[Table("Categorias")]
public class Category
{
    //Boa prativa inicializar uma coleção 
    public Category()
    {
        Products = new Collection<Product>();
    }
    
    [Key]
    public int CategoryId { get; set; }
    
    [Required]
    [StringLength(80)]
    public string? Name { get; set; }
    
    [Required]
    [StringLength(300)]
    public string? ImageUrl { get; set; }
    
    //Adcionando relacionamento de entre categoria e produto
    public ICollection<Product>? Products { get; set; }
}