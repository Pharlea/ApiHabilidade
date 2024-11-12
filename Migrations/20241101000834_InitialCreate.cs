using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RPG_API.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TabHabilidades",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Nome = table.Column<string>(type: "TEXT", nullable: false),
                    Categoria = table.Column<string>(type: "TEXT", nullable: false),
                    Rank = table.Column<char>(type: "TEXT", nullable: false),
                    CustoDeMana = table.Column<int>(type: "INTEGER", nullable: false),
                    TipoDeAcao = table.Column<string>(type: "TEXT", nullable: false),
                    Cooldown = table.Column<string>(type: "TEXT", nullable: false),
                    Conjuracao = table.Column<string>(type: "TEXT", nullable: false),
                    DT = table.Column<int>(type: "INTEGER", nullable: false),
                    Efeito = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TabHabilidades", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TabUsers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Nome = table.Column<string>(type: "TEXT", nullable: false),
                    Email = table.Column<string>(type: "TEXT", nullable: false),
                    Senha = table.Column<string>(type: "TEXT", nullable: false),
                    Role = table.Column<string>(type: "TEXT", nullable: false),
                    FotoPerfil = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TabUsers", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TabHabilidades");

            migrationBuilder.DropTable(
                name: "TabUsers");
        }
    }
}
