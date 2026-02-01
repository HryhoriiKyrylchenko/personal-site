using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PersonalSite.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddImagePropertyToPage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PageImage",
                table: "Pages",
                type: "character varying(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PageImage",
                table: "Pages");
        }
    }
}
