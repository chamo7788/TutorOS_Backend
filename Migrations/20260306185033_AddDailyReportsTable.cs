using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TutorOS.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddDailyReportsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "daily_reports",
                schema: "public",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    report_date = table.Column<DateOnly>(type: "date", nullable: false),
                    total_cash = table.Column<decimal>(type: "numeric(10,2)", nullable: true, defaultValue: 0m),
                    total_online = table.Column<decimal>(type: "numeric(10,2)", nullable: true, defaultValue: 0m),
                    total_students_attended = table.Column<int>(type: "integer", nullable: true, defaultValue: 0),
                    unpaid_students_count = table.Column<int>(type: "integer", nullable: true, defaultValue: 0),
                    late_students_count = table.Column<int>(type: "integer", nullable: true, defaultValue: 0),
                    materials_issued_count = table.Column<int>(type: "integer", nullable: true, defaultValue: 0),
                    notes = table.Column<string>(type: "text", nullable: true),
                    generated_at = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false, defaultValueSql: "NOW()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_daily_reports", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_daily_reports_report_date",
                schema: "public",
                table: "daily_reports",
                column: "report_date",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "daily_reports",
                schema: "public");
        }
    }
}
