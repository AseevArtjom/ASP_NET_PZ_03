using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ASP_NET_PZ_03.Migrations
{
    /// <inheritdoc />
    public partial class UserRoles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Name", "NormalizedName" },
                values: new object[,]
                {
                    {"Admin","ADMIN" },
                    {"User","USER" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
