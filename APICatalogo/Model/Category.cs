using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace APICatalogo.Model;

[Table("Categorias")]
public class Category
{
    //Boa pratica inicializar uma coleção 
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
    ////O JsonIgnore não será apresentado no objeto do swagger
    [JsonIgnore]
    public ICollection<Product>? Products { get; set; }
}