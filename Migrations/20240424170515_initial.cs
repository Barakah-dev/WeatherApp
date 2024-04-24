using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WeatherApp.Migrations
{
    /// <inheritdoc />
    public partial class initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LocationWeatherData",
                columns: table => new
                {
                    id = table.Column<string>(type: "text", nullable: false),
                    lon = table.Column<double>(type: "double precision", nullable: false),
                    lat = table.Column<double>(type: "double precision", nullable: false),
                    weather = table.Column<string>(type: "jsonb", nullable: false),
                    @base = table.Column<string>(name: "base", type: "text", nullable: false),
                    main = table.Column<string>(type: "jsonb", nullable: false),
                    visibility = table.Column<int>(type: "integer", nullable: false),
                    wind = table.Column<string>(type: "jsonb", nullable: false),
                    clouds = table.Column<string>(type: "jsonb", nullable: false),
                    dt = table.Column<int>(type: "integer", nullable: false),
                    sys = table.Column<string>(type: "jsonb", nullable: false),
                    timezone = table.Column<int>(type: "integer", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    cod = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LocationWeatherData", x => x.id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LocationWeatherData");
        }
    }
}
