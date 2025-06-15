using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PersonalSite.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Logs_Improvements : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Logs_Source",
                table: "Logs");

            migrationBuilder.DropColumn(
                name: "Source",
                table: "Logs");

            migrationBuilder.AlterColumn<string>(
                name: "Message",
                table: "Logs",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(1000)",
                oldMaxLength: 1000);

            migrationBuilder.AlterColumn<string>(
                name: "Level",
                table: "Logs",
                type: "character varying(128)",
                maxLength: 128,
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<string>(
                name: "Exception",
                table: "Logs",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(2000)",
                oldMaxLength: 2000,
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MessageTemplate",
                table: "Logs",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Properties",
                table: "Logs",
                type: "jsonb",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SourceContext",
                table: "Logs",
                type: "character varying(255)",
                maxLength: 255,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MessageTemplate",
                table: "Logs");

            migrationBuilder.DropColumn(
                name: "Properties",
                table: "Logs");

            migrationBuilder.DropColumn(
                name: "SourceContext",
                table: "Logs");

            migrationBuilder.AlterColumn<string>(
                name: "Message",
                table: "Logs",
                type: "character varying(1000)",
                maxLength: 1000,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<int>(
                name: "Level",
                table: "Logs",
                type: "integer",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(128)",
                oldMaxLength: 128);

            migrationBuilder.AlterColumn<string>(
                name: "Exception",
                table: "Logs",
                type: "character varying(2000)",
                maxLength: 2000,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Source",
                table: "Logs",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Logs_Source",
                table: "Logs",
                column: "Source");
        }
    }
}
