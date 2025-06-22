using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PersonalSite.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ModelsAdjustment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Translations_Languages_LanguageCode",
                table: "Translations");

            migrationBuilder.DropIndex(
                name: "IX_Translations_LanguageCode",
                table: "Translations");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Languages",
                table: "Languages");

            migrationBuilder.DropColumn(
                name: "LanguageCode",
                table: "Translations");

            migrationBuilder.DropColumn(
                name: "PageKey",
                table: "PageTranslations");

            migrationBuilder.AddColumn<Guid>(
                name: "LanguageId",
                table: "Translations",
                type: "uuid",
                maxLength: 2,
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "PageId",
                table: "PageTranslations",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "Id",
                table: "Languages",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddPrimaryKey(
                name: "PK_Languages",
                table: "Languages",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "Pages",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Key = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pages", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Translations_LanguageId",
                table: "Translations",
                column: "LanguageId");

            migrationBuilder.CreateIndex(
                name: "IX_PageTranslations_PageId",
                table: "PageTranslations",
                column: "PageId");

            migrationBuilder.CreateIndex(
                name: "IX_Languages_Code",
                table: "Languages",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Pages_Key",
                table: "Pages",
                column: "Key",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_PageTranslations_Pages_PageId",
                table: "PageTranslations",
                column: "PageId",
                principalTable: "Pages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Translations_Languages_LanguageId",
                table: "Translations",
                column: "LanguageId",
                principalTable: "Languages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PageTranslations_Pages_PageId",
                table: "PageTranslations");

            migrationBuilder.DropForeignKey(
                name: "FK_Translations_Languages_LanguageId",
                table: "Translations");

            migrationBuilder.DropTable(
                name: "Pages");

            migrationBuilder.DropIndex(
                name: "IX_Translations_LanguageId",
                table: "Translations");

            migrationBuilder.DropIndex(
                name: "IX_PageTranslations_PageId",
                table: "PageTranslations");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Languages",
                table: "Languages");

            migrationBuilder.DropIndex(
                name: "IX_Languages_Code",
                table: "Languages");

            migrationBuilder.DropColumn(
                name: "LanguageId",
                table: "Translations");

            migrationBuilder.DropColumn(
                name: "PageId",
                table: "PageTranslations");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "Languages");

            migrationBuilder.AddColumn<string>(
                name: "LanguageCode",
                table: "Translations",
                type: "character varying(2)",
                maxLength: 2,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PageKey",
                table: "PageTranslations",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Languages",
                table: "Languages",
                column: "Code");

            migrationBuilder.CreateIndex(
                name: "IX_Translations_LanguageCode",
                table: "Translations",
                column: "LanguageCode");

            migrationBuilder.AddForeignKey(
                name: "FK_Translations_Languages_LanguageCode",
                table: "Translations",
                column: "LanguageCode",
                principalTable: "Languages",
                principalColumn: "Code",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
