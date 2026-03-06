using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TutorOS.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddExamResultsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "exam_results",
                schema: "public",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    student_id = table.Column<Guid>(type: "uuid", nullable: false),
                    exam_id = table.Column<Guid>(type: "uuid", nullable: false),
                    mcq_answers = table.Column<string>(type: "jsonb", nullable: true),
                    mcq_score = table.Column<int>(type: "integer", nullable: true),
                    theory_score = table.Column<int>(type: "integer", nullable: true),
                    total_score = table.Column<int>(type: "integer", nullable: true),
                    percentage = table.Column<decimal>(type: "numeric(5,2)", nullable: true),
                    class_rank = table.Column<int>(type: "integer", nullable: true),
                    island_rank = table.Column<int>(type: "integer", nullable: true),
                    weak_areas = table.Column<string>(type: "jsonb", nullable: true),
                    strong_areas = table.Column<string>(type: "jsonb", nullable: true),
                    graded_at = table.Column<DateTimeOffset>(type: "timestamptz", nullable: true, defaultValueSql: "NOW()"),
                    graded_by = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    notes = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_exam_results", x => x.id);
                    table.ForeignKey(
                        name: "FK_exam_results_exams_exam_id",
                        column: x => x.exam_id,
                        principalSchema: "public",
                        principalTable: "exams",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_exam_results_students_student_id",
                        column: x => x.student_id,
                        principalSchema: "public",
                        principalTable: "students",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_exam_results_exam_id",
                schema: "public",
                table: "exam_results",
                column: "exam_id");

            migrationBuilder.CreateIndex(
                name: "IX_exam_results_student_id_exam_id",
                schema: "public",
                table: "exam_results",
                columns: new[] { "student_id", "exam_id" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "exam_results",
                schema: "public");
        }
    }
}
