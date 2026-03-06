using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TutorOS.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddExamsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "exams",
                schema: "public",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    class_id = table.Column<Guid>(type: "uuid", nullable: false),
                    title = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    exam_type = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    exam_date = table.Column<DateOnly>(type: "date", nullable: true),
                    total_marks = table.Column<int>(type: "integer", nullable: false),
                    pass_marks = table.Column<int>(type: "integer", nullable: true),
                    duration_minutes = table.Column<int>(type: "integer", nullable: true),
                    mcq_count = table.Column<int>(type: "integer", nullable: true),
                    theory_count = table.Column<int>(type: "integer", nullable: true),
                    answer_key = table.Column<string>(type: "jsonb", nullable: true),
                    topic_mapping = table.Column<string>(type: "jsonb", nullable: true),
                    created_at = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false, defaultValueSql: "NOW()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_exams", x => x.id);
                    table.ForeignKey(
                        name: "FK_exams_classes_class_id",
                        column: x => x.class_id,
                        principalSchema: "public",
                        principalTable: "classes",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_exams_class_id",
                schema: "public",
                table: "exams",
                column: "class_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "exams",
                schema: "public");
        }
    }
}
