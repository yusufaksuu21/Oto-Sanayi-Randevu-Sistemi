using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using SanayiRandevu.Data;

namespace SanayiRandevu.Migrations;

[DbContext(typeof(ApplicationDbContext))]
[Migration("20260912190000_DoubleServicePrices")]
public partial class DoubleServicePrices : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.Sql("UPDATE Services SET Price = Price * 2;");
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.Sql("UPDATE Services SET Price = Price / 2;");
    }
}
