using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TutorOS.Api.Migrations
{
    /// <inheritdoc />
    public partial class CreateStudentsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("CREATE EXTENSION IF NOT EXISTS \"pgcrypto\";");

            migrationBuilder.EnsureSchema(
                name: "public");

            migrationBuilder.CreateTable(
                name: "students",
                schema: "public",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    student_code = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    first_name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    last_name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    email = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    phone = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    parent_phone = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    parent_name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    address = table.Column<string>(type: "text", nullable: true),
                    grade = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    stream = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    school = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    qr_code_data = table.Column<string>(type: "text", nullable: true),
                    profile_image_url = table.Column<string>(type: "text", nullable: true),
                    status = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false, defaultValue: "active"),
                    special_note = table.Column<string>(type: "text", nullable: true),
                    joined_date = table.Column<DateOnly>(type: "date", nullable: false, defaultValueSql: "CURRENT_DATE"),
                    created_at = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false, defaultValueSql: "NOW()"),
                    updated_at = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false, defaultValueSql: "NOW()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_students", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_students_qr_code_data",
                schema: "public",
                table: "students",
                column: "qr_code_data",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_students_student_code",
                schema: "public",
                table: "students",
                column: "student_code",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "students",
                schema: "public");
        }
    }
}
