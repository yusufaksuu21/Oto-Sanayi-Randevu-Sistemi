using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SanayiRandevu.Migrations
{
    /// <inheritdoc />
    public partial class AddAdminReplyToAppointment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AdminReply",
                table: "Appointments",
                type: "TEXT",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "AdminReplyAt",
                table: "Appointments",
                type: "TEXT",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AdminReply",
                table: "Appointments");

            migrationBuilder.DropColumn(
                name: "AdminReplyAt",
                table: "Appointments");
        }
    }
}
