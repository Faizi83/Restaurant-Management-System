using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Resturant.Migrations
{
    /// <inheritdoc />
    public partial class FoodsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "allfoods",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    foodname = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    foodprice = table.Column<int>(type: "int", nullable: false),
                    fooddesc = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    foodimage = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_allfoods", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "allfoods");
        }
    }
}
