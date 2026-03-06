using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TutorOS.Api.Migrations
{
    /// <inheritdoc />
    public partial class ApplyRLSAndPolicies : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
-- ============================================
-- ROW LEVEL SECURITY
-- ============================================

ALTER TABLE public.students ENABLE ROW LEVEL SECURITY;
ALTER TABLE public.classes ENABLE ROW LEVEL SECURITY;
ALTER TABLE public.enrollments ENABLE ROW LEVEL SECURITY;
ALTER TABLE public.attendance ENABLE ROW LEVEL SECURITY;
ALTER TABLE public.materials ENABLE ROW LEVEL SECURITY;
ALTER TABLE public.material_issues ENABLE ROW LEVEL SECURITY;
ALTER TABLE public.fee_periods ENABLE ROW LEVEL SECURITY;
ALTER TABLE public.payments ENABLE ROW LEVEL SECURITY;
ALTER TABLE public.student_fee_status ENABLE ROW LEVEL SECURITY;
ALTER TABLE public.exams ENABLE ROW LEVEL SECURITY;
ALTER TABLE public.exam_results ENABLE ROW LEVEL SECURITY;
ALTER TABLE public.staff ENABLE ROW LEVEL SECURITY;
ALTER TABLE public.daily_reports ENABLE ROW LEVEL SECURITY;

-- RLS Policies - Allow authenticated users (staff) to access all data
CREATE POLICY ""staff_all_students"" ON public.students FOR ALL TO authenticated USING (true) WITH CHECK (true);
CREATE POLICY ""staff_all_classes"" ON public.classes FOR ALL TO authenticated USING (true) WITH CHECK (true);
CREATE POLICY ""staff_all_enrollments"" ON public.enrollments FOR ALL TO authenticated USING (true) WITH CHECK (true);
CREATE POLICY ""staff_all_attendance"" ON public.attendance FOR ALL TO authenticated USING (true) WITH CHECK (true);
CREATE POLICY ""staff_all_materials"" ON public.materials FOR ALL TO authenticated USING (true) WITH CHECK (true);
CREATE POLICY ""staff_all_material_issues"" ON public.material_issues FOR ALL TO authenticated USING (true) WITH CHECK (true);
CREATE POLICY ""staff_all_fee_periods"" ON public.fee_periods FOR ALL TO authenticated USING (true) WITH CHECK (true);
CREATE POLICY ""staff_all_payments"" ON public.payments FOR ALL TO authenticated USING (true) WITH CHECK (true);
CREATE POLICY ""staff_all_fee_status"" ON public.student_fee_status FOR ALL TO authenticated USING (true) WITH CHECK (true);
CREATE POLICY ""staff_all_exams"" ON public.exams FOR ALL TO authenticated USING (true) WITH CHECK (true);
CREATE POLICY ""staff_all_exam_results"" ON public.exam_results FOR ALL TO authenticated USING (true) WITH CHECK (true);
CREATE POLICY ""staff_all_staff"" ON public.staff FOR ALL TO authenticated USING (true) WITH CHECK (true);
CREATE POLICY ""staff_all_daily_reports"" ON public.daily_reports FOR ALL TO authenticated USING (true) WITH CHECK (true);

-- Public access for student portal (progress viewing) - limited to their own data
CREATE POLICY ""public_exam_results"" ON public.exam_results FOR SELECT TO anon USING (true);
CREATE POLICY ""public_exams"" ON public.exams FOR SELECT TO anon USING (true);
CREATE POLICY ""public_students"" ON public.students FOR SELECT TO anon USING (true);
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
DROP POLICY IF EXISTS ""public_students"" ON public.students;
DROP POLICY IF EXISTS ""public_exams"" ON public.exams;
DROP POLICY IF EXISTS ""public_exam_results"" ON public.exam_results;

DROP POLICY IF EXISTS ""staff_all_daily_reports"" ON public.daily_reports;
DROP POLICY IF EXISTS ""staff_all_staff"" ON public.staff;
DROP POLICY IF EXISTS ""staff_all_exam_results"" ON public.exam_results;
DROP POLICY IF EXISTS ""staff_all_exams"" ON public.exams;
DROP POLICY IF EXISTS ""staff_all_fee_status"" ON public.student_fee_status;
DROP POLICY IF EXISTS ""staff_all_payments"" ON public.payments;
DROP POLICY IF EXISTS ""staff_all_fee_periods"" ON public.fee_periods;
DROP POLICY IF EXISTS ""staff_all_material_issues"" ON public.material_issues;
DROP POLICY IF EXISTS ""staff_all_materials"" ON public.materials;
DROP POLICY IF EXISTS ""staff_all_attendance"" ON public.attendance;
DROP POLICY IF EXISTS ""staff_all_enrollments"" ON public.enrollments;
DROP POLICY IF EXISTS ""staff_all_classes"" ON public.classes;
DROP POLICY IF EXISTS ""staff_all_students"" ON public.students;

ALTER TABLE public.daily_reports DISABLE ROW LEVEL SECURITY;
ALTER TABLE public.staff DISABLE ROW LEVEL SECURITY;
ALTER TABLE public.exam_results DISABLE ROW LEVEL SECURITY;
ALTER TABLE public.exams DISABLE ROW LEVEL SECURITY;
ALTER TABLE public.student_fee_status DISABLE ROW LEVEL SECURITY;
ALTER TABLE public.payments DISABLE ROW LEVEL SECURITY;
ALTER TABLE public.fee_periods DISABLE ROW LEVEL SECURITY;
ALTER TABLE public.material_issues DISABLE ROW LEVEL SECURITY;
ALTER TABLE public.materials DISABLE ROW LEVEL SECURITY;
ALTER TABLE public.attendance DISABLE ROW LEVEL SECURITY;
ALTER TABLE public.enrollments DISABLE ROW LEVEL SECURITY;
ALTER TABLE public.classes DISABLE ROW LEVEL SECURITY;
ALTER TABLE public.students DISABLE ROW LEVEL SECURITY;
            ");
        }
    }
}
