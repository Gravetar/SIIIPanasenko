using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SIIILab2.Migrations
{
    /// <inheritdoc />
    public partial class InitMigr : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Traffics",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Density = table.Column<int>(type: "int", nullable: false),
                    PathFile = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Traffics", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Roads",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PathFile = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CoorLatitude1 = table.Column<double>(type: "float", nullable: false),
                    CoorLongitude1 = table.Column<double>(type: "float", nullable: false),
                    CoorLatitude2 = table.Column<double>(type: "float", nullable: false),
                    CoorLongitude2 = table.Column<double>(type: "float", nullable: false),
                    trafficid = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roads", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Roads_Traffics_trafficid",
                        column: x => x.trafficid,
                        principalTable: "Traffics",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RequestRoads",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Costumer = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Reques_date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Result = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    roadid = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RequestRoads", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RequestRoads_Roads_roadid",
                        column: x => x.roadid,
                        principalTable: "Roads",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Traffics",
                columns: new[] { "Id", "Density", "PathFile" },
                values: new object[] { 1, 1, "\\Traffics\\1.xml" });

            migrationBuilder.InsertData(
                table: "Roads",
                columns: new[] { "Id", "Address", "CoorLatitude1", "CoorLatitude2", "CoorLongitude1", "CoorLongitude2", "PathFile", "trafficid" },
                values: new object[] { 1, "г. Волгоград пр. Ленина 1", 48.7194, 49.7194, 44.501800000000003, 49.501800000000003, "\\Roads\\1.xml", 1 });

            migrationBuilder.InsertData(
                table: "RequestRoads",
                columns: new[] { "Id", "Costumer", "Reques_date", "Result", "Status", "roadid" },
                values: new object[] { 1, "ООО ДОРОГА", new DateTime(2023, 10, 17, 17, 27, 54, 997, DateTimeKind.Local).AddTicks(3809), "NONE", "В работе", 1 });

            migrationBuilder.CreateIndex(
                name: "IX_RequestRoads_roadid",
                table: "RequestRoads",
                column: "roadid");

            migrationBuilder.CreateIndex(
                name: "IX_Roads_trafficid",
                table: "Roads",
                column: "trafficid");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RequestRoads");

            migrationBuilder.DropTable(
                name: "Roads");

            migrationBuilder.DropTable(
                name: "Traffics");
        }
    }
}
