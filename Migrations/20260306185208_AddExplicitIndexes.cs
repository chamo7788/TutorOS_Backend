using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TutorOS.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddExplicitIndexes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameIndex(
                name: "IX_students_student_code",
                schema: "public",
                table: "students",
                newName: "idx_students_student_code");

            migrationBuilder.RenameIndex(
                name: "IX_students_qr_code_data",
                schema: "public",
                table: "students",
                newName: "idx_students_qr_code");

            migrationBuilder.RenameIndex(
                name: "IX_payments_student_id",
                schema: "public",
                table: "payments",
                newName: "idx_payments_student");

            migrationBuilder.RenameIndex(
                name: "IX_payments_fee_period_id",
                schema: "public",
                table: "payments",
                newName: "idx_payments_period");

            migrationBuilder.RenameIndex(
                name: "IX_exam_results_exam_id",
                schema: "public",
                table: "exam_results",
                newName: "idx_exam_results_exam");

            migrationBuilder.RenameIndex(
                name: "IX_attendance_student_id",
                schema: "public",
                table: "attendance",
                newName: "idx_attendance_student");

            migrationBuilder.CreateIndex(
                name: "idx_fee_status_student",
                schema: "public",
                table: "student_fee_status",
                column: "student_id");

            migrationBuilder.CreateIndex(
                name: "idx_exam_results_student",
                schema: "public",
                table: "exam_results",
                column: "student_id");

            migrationBuilder.CreateIndex(
                name: "idx_attendance_date",
                schema: "public",
                table: "attendance",
                column: "scan_date");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "idx_fee_status_student",
                schema: "public",
                table: "student_fee_status");

            migrationBuilder.DropIndex(
                name: "idx_exam_results_student",
                schema: "public",
                table: "exam_results");

            migrationBuilder.DropIndex(
                name: "idx_attendance_date",
                schema: "public",
                table: "attendance");

            migrationBuilder.RenameIndex(
                name: "idx_students_student_code",
                schema: "public",
                table: "students",
                newName: "IX_students_student_code");

            migrationBuilder.RenameIndex(
                name: "idx_students_qr_code",
                schema: "public",
                table: "students",
                newName: "IX_students_qr_code_data");

            migrationBuilder.RenameIndex(
                name: "idx_payments_student",
                schema: "public",
                table: "payments",
                newName: "IX_payments_student_id");

            migrationBuilder.RenameIndex(
                name: "idx_payments_period",
                schema: "public",
                table: "payments",
                newName: "IX_payments_fee_period_id");

            migrationBuilder.RenameIndex(
                name: "idx_exam_results_exam",
                schema: "public",
                table: "exam_results",
                newName: "IX_exam_results_exam_id");

            migrationBuilder.RenameIndex(
                name: "idx_attendance_student",
                schema: "public",
                table: "attendance",
                newName: "IX_attendance_student_id");
        }
    }
}
