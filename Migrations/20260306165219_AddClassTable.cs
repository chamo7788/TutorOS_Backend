using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TutorOS.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddClassTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "classes",
                schema: "public",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    subject = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    grade = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    stream = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    teacher_name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    day_of_week = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    start_time = table.Column<TimeOnly>(type: "time without time zone", nullable: true),
                    end_time = table.Column<TimeOnly>(type: "time without time zone", nullable: true),
                    monthly_fee = table.Column<decimal>(type: "numeric", nullable: false),
                    half_month_fee = table.Column<decimal>(type: "numeric", nullable: true),
                    location = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    max_students = table.Column<int>(type: "integer", nullable: true),
                    status = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_classes", x => x.id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "classes",
                schema: "public");
        }
    }
}
