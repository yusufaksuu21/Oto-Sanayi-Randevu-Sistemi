using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SanayiRandevu.Migrations
{
    public partial class EnsureDoubledServicePrices : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("""
                UPDATE Services
                SET Price = CASE Name
                    WHEN 'Yağ Değişimi' THEN 1000
                    WHEN 'Lastik Değişimi' THEN 600
                    WHEN 'Genel Bakım' THEN 2400
                    WHEN 'Fren Kontrolü' THEN 800
                    WHEN 'Arıza Tespiti' THEN 700
                END
                WHERE (Name = 'Yağ Değişimi' AND Price = 500)
                   OR (Name = 'Lastik Değişimi' AND Price = 300)
                   OR (Name = 'Genel Bakım' AND Price = 1200)
                   OR (Name = 'Fren Kontrolü' AND Price = 400)
                   OR (Name = 'Arıza Tespiti' AND Price = 350);
                """);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("""
                UPDATE Services
                SET Price = CASE Name
                    WHEN 'Yağ Değişimi' THEN 500
                    WHEN 'Lastik Değişimi' THEN 300
                    WHEN 'Genel Bakım' THEN 1200
                    WHEN 'Fren Kontrolü' THEN 400
                    WHEN 'Arıza Tespiti' THEN 350
                END
                WHERE (Name = 'Yağ Değişimi' AND Price = 1000)
                   OR (Name = 'Lastik Değişimi' AND Price = 600)
                   OR (Name = 'Genel Bakım' AND Price = 2400)
                   OR (Name = 'Fren Kontrolü' AND Price = 800)
                   OR (Name = 'Arıza Tespiti' AND Price = 700);
                """);
        }
    }
}
