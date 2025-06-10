using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace APICatalogo.Migrations
{
    /// <inheritdoc />
    public partial class PopulaCategorias : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("Insert into Categorias (Name, ImageUrl) values ('Bebidas', 'bebidas.jpg')");
            migrationBuilder.Sql("Insert into Categorias (Name, ImageUrl) values ('Lanches', 'lanches.jpg')");
            migrationBuilder.Sql("Insert into Categorias (Name, ImageUrl) values ('Sobremesas', 'sobremesas.jpg')");
            
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("Delete from Categorias");
            
        }
    }
}
