using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ASAssignment2222256B.Migrations
{
    /// <inheritdoc />
    public partial class uiaghroiuaghrfg : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PasswordHistory",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PasswordHistory",
                table: "AspNetUsers");
        }
    }
}
