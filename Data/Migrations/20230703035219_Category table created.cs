using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ecommerce9am.Migrations
{
    /// <inheritdoc />
    public partial class Categorytablecreated : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Catagories",
                columns: table => new
                {
                    CategoryID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CategoryName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Catagories", x => x.CategoryID);
                });

            migrationBuilder.InsertData(
                table: "Catagories",
                columns: new[] { "CategoryID", "CategoryName" },
                values: new object[] { 1, "Test" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Catagories");
        }
    }
}
