using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PMS_Data_Extraction_from_Machinery_Manual.Migrations
{
    /// <inheritdoc />
    public partial class vs : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ExcelValidate",
                columns: table => new
                {
                    ExcelId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClientName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    VesselName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ManualPath = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ManualName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TotalPages = table.Column<int>(type: "int", nullable: false),
                    EquipmentName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Maker = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ModelType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NoOfUnit = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SparePageNo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    JobPageNo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TechnicalData = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Remarks = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExcelValidate", x => x.ExcelId);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ExcelValidate");
        }
    }
}
