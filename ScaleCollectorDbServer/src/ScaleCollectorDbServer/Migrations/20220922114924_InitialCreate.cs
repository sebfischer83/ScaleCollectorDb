using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ScaleCollectorDbServer.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Brands",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    Tenant = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Brands", x => x.Id)
                        .Annotation("SqlServer:Clustered", true);
                });

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
                    Type = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    ManufacturerArticleNumber = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ScaleId = table.Column<long>(type: "bigint", nullable: false),
                    BrandId = table.Column<long>(type: "bigint", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    Tenant = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ModelKits", x => x.Id)
                        .Annotation("SqlServer:Clustered", true);
                    table.ForeignKey(
                        name: "FK_ModelKits_Brands_BrandId",
                        column: x => x.BrandId,
                        principalTable: "Brands",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ModelKits_Scales_ScaleId",
                        column: x => x.ScaleId,
                        principalTable: "Scales",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Image",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Type = table.Column<int>(type: "int", nullable: false),
                    ModelKitId = table.Column<long>(type: "bigint", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    Tenant = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Image", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Image_ModelKits_ModelKitId",
                        column: x => x.ModelKitId,
                        principalTable: "ModelKits",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ModelKitReference",
                columns: table => new
                {
                    ModelKitId = table.Column<long>(type: "bigint", nullable: false),
                    ReferenceId = table.Column<long>(type: "bigint", nullable: false),
                    Id = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ModelKitReference", x => new { x.ModelKitId, x.ReferenceId });
                    table.ForeignKey(
                        name: "FK_ModelKitReference_ModelKits_ModelKitId",
                        column: x => x.ModelKitId,
                        principalTable: "ModelKits",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ModelKitReference_ModelKits_ReferenceId",
                        column: x => x.ReferenceId,
                        principalTable: "ModelKits",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Image_ModelKitId",
                table: "Image",
                column: "ModelKitId");

            migrationBuilder.CreateIndex(
                name: "IX_ModelKitReference_ReferenceId",
                table: "ModelKitReference",
                column: "ReferenceId");

            migrationBuilder.CreateIndex(
                name: "IX_ModelKits_BrandId",
                table: "ModelKits",
                column: "BrandId");

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
                name: "Image");

            migrationBuilder.DropTable(
                name: "ModelKitReference");

            migrationBuilder.DropTable(
                name: "ModelKits");

            migrationBuilder.DropTable(
                name: "Brands");

            migrationBuilder.DropTable(
                name: "Scales");
        }
    }
}
