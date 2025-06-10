using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace APICatalogo.Migrations
{
    /// <inheritdoc />
    public partial class PopulaProdutos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder mb)
        {
            mb.Sql("Insert into Products (Name, Description, Value, ImageUrl, Stock, RegistryDate, CategoryId) values ('Coca-Cola Diet', 'Refrigerante de cola', 5.00, 'coca-cola.jpg', 100, now(), 1)");
            
            mb.Sql("Insert into Products (Name, Description, Value, ImageUrl, Stock, RegistryDate, CategoryId) values ('X-Burguer', 'Sanduíche com hambúrguer, queijo e molho especial', 15.00, 'x-burguer.jpg', 50, now(), 2)");
            
            mb.Sql("Insert into Products (Name, Description, Value, ImageUrl, Stock, RegistryDate, CategoryId) values ('Pudim', 'Sobremesa clássica de leite condensado', 10.00, 'pudim.jpg', 30, now(), 3)");
    
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("Delete from Products");
        }
    }
}
