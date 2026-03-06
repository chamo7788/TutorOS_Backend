using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TutorOS.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddStudentFeeStatusTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "student_fee_status",
                schema: "public",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    student_id = table.Column<Guid>(type: "uuid", nullable: false),
                    class_id = table.Column<Guid>(type: "uuid", nullable: false),
                    fee_period_id = table.Column<Guid>(type: "uuid", nullable: false),
                    is_paid = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    amount_due = table.Column<decimal>(type: "numeric(10,2)", nullable: true),
                    amount_paid = table.Column<decimal>(type: "numeric(10,2)", nullable: false, defaultValue: 0m),
                    due_date = table.Column<DateOnly>(type: "date", nullable: true),
                    updated_at = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false, defaultValueSql: "NOW()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_student_fee_status", x => x.id);
                    table.ForeignKey(
                        name: "FK_student_fee_status_classes_class_id",
                        column: x => x.class_id,
                        principalSchema: "public",
                        principalTable: "classes",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_student_fee_status_fee_periods_fee_period_id",
                        column: x => x.fee_period_id,
                        principalSchema: "public",
                        principalTable: "fee_periods",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_student_fee_status_students_student_id",
                        column: x => x.student_id,
                        principalSchema: "public",
                        principalTable: "students",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_student_fee_status_class_id",
                schema: "public",
                table: "student_fee_status",
                column: "class_id");

            migrationBuilder.CreateIndex(
                name: "IX_student_fee_status_fee_period_id",
                schema: "public",
                table: "student_fee_status",
                column: "fee_period_id");

            migrationBuilder.CreateIndex(
                name: "IX_student_fee_status_student_id_class_id_fee_period_id",
                schema: "public",
                table: "student_fee_status",
                columns: new[] { "student_id", "class_id", "fee_period_id" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "student_fee_status",
                schema: "public");
        }
    }
}
