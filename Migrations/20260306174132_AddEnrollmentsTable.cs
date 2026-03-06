using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TutorOS.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddEnrollmentsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "enrollments",
                schema: "public",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    student_id = table.Column<Guid>(type: "uuid", nullable: false),
                    class_id = table.Column<Guid>(type: "uuid", nullable: false),
                    enrolled_date = table.Column<DateOnly>(type: "date", nullable: false, defaultValueSql: "CURRENT_DATE"),
                    status = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false, defaultValue: "active"),
                    created_at = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false, defaultValueSql: "NOW()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_enrollments", x => x.id);
                    table.ForeignKey(
                        name: "FK_enrollments_classes_class_id",
                        column: x => x.class_id,
                        principalSchema: "public",
                        principalTable: "classes",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_enrollments_students_student_id",
                        column: x => x.student_id,
                        principalSchema: "public",
                        principalTable: "students",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_enrollments_class_id",
                schema: "public",
                table: "enrollments",
                column: "class_id");

            migrationBuilder.CreateIndex(
                name: "IX_enrollments_student_id_class_id",
                schema: "public",
                table: "enrollments",
                columns: new[] { "student_id", "class_id" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "enrollments",
                schema: "public");
        }
    }
}
