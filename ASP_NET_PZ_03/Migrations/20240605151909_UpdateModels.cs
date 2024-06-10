using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ASP_NET_PZ_03.Migrations
{
    /// <inheritdoc />
    public partial class UpdateModels : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Infos_Professions_ProfessionId",
                table: "Infos");

            migrationBuilder.DropColumn(
                name: "ImageSrc",
                table: "Infos");

            migrationBuilder.AlterColumn<int>(
                name: "ProfessionId",
                table: "Infos",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AddForeignKey(
                name: "FK_Infos_Professions_ProfessionId",
                table: "Infos",
                column: "ProfessionId",
                principalTable: "Professions",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Infos_Professions_ProfessionId",
                table: "Infos");

            migrationBuilder.AlterColumn<int>(
                name: "ProfessionId",
                table: "Infos",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ImageSrc",
                table: "Infos",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Infos_Professions_ProfessionId",
                table: "Infos",
                column: "ProfessionId",
                principalTable: "Professions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
