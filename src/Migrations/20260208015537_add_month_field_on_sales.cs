using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IKT_BACKEND.Migrations
{
    /// <inheritdoc />
    public partial class add_month_field_on_sales : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Month",
                table: "Sales",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Month",
                table: "Sales");
        }
    }
}
