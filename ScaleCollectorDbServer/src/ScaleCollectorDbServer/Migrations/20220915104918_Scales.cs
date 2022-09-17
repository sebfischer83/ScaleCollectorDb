using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ScaleCollectorDbServer.Migrations
{
    public partial class Scales : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Scales",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RatioFrom = table.Column<int>(type: "int", nullable: false),
                    RatioTo = table.Column<int>(type: "int", nullable: false),
                    RatioText = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    Tenant = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Scales", x => x.Id)
                        .Annotation("SqlServer:Clustered", true);
                });

            migrationBuilder.CreateTable(
                name: "ModelKits",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ScaleId = table.Column<long>(type: "bigint", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    Tenant = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ModelKits", x => x.Id)
                        .Annotation("SqlServer:Clustered", true);
                    table.ForeignKey(
                        name: "FK_ModelKits_Scales_ScaleId",
                        column: x => x.ScaleId,
                        principalTable: "Scales",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ModelKits_ScaleId",
                table: "ModelKits",
                column: "ScaleId");

            migrationBuilder.CreateIndex(
                name: "IX_ModelKits_Tenant",
                table: "ModelKits",
                column: "Tenant");

            migrationBuilder.CreateIndex(
                name: "IX_Scales_Tenant_RatioText",
                table: "Scales",
                columns: new[] { "Tenant", "RatioText" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ModelKits");

            migrationBuilder.DropTable(
                name: "Scales");
        }
    }
}
