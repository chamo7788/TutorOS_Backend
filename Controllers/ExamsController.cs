using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TutorOS.Api.DTOs;
using TutorOS.Api.Models;

namespace TutorOS.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ExamsController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public ExamsController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: api/exams
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ExamResponse>>> GetExams([FromQuery] Guid? classId)
    {
        var query = _context.Exams.Include(e => e.Class).AsQueryable();

        if (classId.HasValue)
        {
            query = query.Where(e => e.ClassId == classId.Value);
        }

        var exams = await query
            .OrderByDescending(e => e.ExamDate)
            .Select(e => new ExamResponse
            {
                Id = e.Id,
                ClassId = e.ClassId,
                ClassName = e.Class.Name,
                Title = e.Title,
                ExamType = e.ExamType,
                ExamDate = e.ExamDate,
                TotalMarks = e.TotalMarks,
                PassMarks = e.PassMarks,
                DurationMinutes = e.DurationMinutes
            })
            .ToListAsync();

        return Ok(exams);
    }

    // POST: api/exams
    [HttpPost]
    public async Task<ActionResult<ExamResponse>> CreateExam([FromBody] CreateExamRequest request)
    {
        var cls = await _context.Classes.FindAsync(request.ClassId);
        if (cls == null)
        {
            return NotFound(new { Message = "Class not found." });
        }

        var exam = new Exam
        {
            Id = Guid.NewGuid(),
            ClassId = request.ClassId,
            Title = request.Title,
            ExamType = request.ExamType,
            ExamDate = request.ExamDate,
            TotalMarks = request.TotalMarks,
            PassMarks = request.PassMarks,
            DurationMinutes = request.DurationMinutes,
            McqCount = request.McqCount,
            TheoryCount = request.TheoryCount,
            AnswerKey = request.AnswerKey,
            TopicMapping = request.TopicMapping,
            CreatedAt = DateTimeOffset.UtcNow
        };

        _context.Exams.Add(exam);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetExams), new { id = exam.Id }, new ExamResponse
        {
            Id = exam.Id,
            ClassId = exam.ClassId,
            ClassName = cls.Name,
            Title = exam.Title,
            ExamType = exam.ExamType,
            ExamDate = exam.ExamDate,
            TotalMarks = exam.TotalMarks,
            PassMarks = exam.PassMarks,
            DurationMinutes = exam.DurationMinutes
        });
    }

    // GET: api/exams/{examId}/results
    [HttpGet("{examId}/results")]
    public async Task<ActionResult<IEnumerable<ExamResultResponse>>> GetExamResults(Guid examId)
    {
        var exam = await _context.Exams.FindAsync(examId);
        if (exam == null)
        {
            return NotFound(new { Message = "Exam not found." });
        }

        var results = await _context.ExamResults
            .Include(er => er.Student)
            .Where(er => er.ExamId == examId)
            .OrderByDescending(er => er.TotalScore)
            .Select(er => new ExamResultResponse
            {
                Id = er.Id,
                StudentId = er.StudentId,
                StudentName = $"{er.Student.FirstName} {er.Student.LastName}",
                StudentCode = er.Student.StudentCode,
                McqScore = er.McqScore,
                TheoryScore = er.TheoryScore,
                TotalScore = er.TotalScore,
                Percentage = er.Percentage,
                ClassRank = er.ClassRank,
                Notes = er.Notes
            })
            .ToListAsync();

        return Ok(results);
    }

    // POST: api/exams/{examId}/results
    [HttpPost("{examId}/results")]
    public async Task<ActionResult> SubmitResult(Guid examId, [FromBody] SubmitResultRequest request)
    {
        var exam = await _context.Exams.FindAsync(examId);
        if (exam == null)
        {
            return NotFound(new { Message = "Exam not found." });
        }

        var student = await _context.Students.FindAsync(request.StudentId);
        if (student == null)
        {
            return NotFound(new { Message = "Student not found." });
        }

        var result = await _context.ExamResults
            .FirstOrDefaultAsync(er => er.ExamId == examId && er.StudentId == request.StudentId);

        if (result == null)
        {
            result = new ExamResult
            {
                Id = Guid.NewGuid(),
                ExamId = examId,
                StudentId = request.StudentId
            };
            _context.ExamResults.Add(result);
        }

        result.McqScore = request.McqScore;
        result.TheoryScore = request.TheoryScore;
        result.McqAnswers = request.McqAnswers;
        result.WeakAreas = request.WeakAreas;
        result.StrongAreas = request.StrongAreas;
        result.Notes = request.Notes;
        result.GradedBy = request.GradedBy;
        result.GradedAt = DateTimeOffset.UtcNow;

        // Calculate TotalScore and Percentage
        result.TotalScore = (result.McqScore ?? 0) + (result.TheoryScore ?? 0);
        result.Percentage = exam.TotalMarks > 0 
            ? Math.Round((decimal)result.TotalScore / exam.TotalMarks * 100, 2) 
            : 0;

        await _context.SaveChangesAsync();

        // Recalculate rankings for this exam
        await RecalculateRankings(examId);

        return Ok(new { Message = "Result submitted successfully." });
    }

    private async Task RecalculateRankings(Guid examId)
    {
        var results = await _context.ExamResults
            .Where(er => er.ExamId == examId)
            .OrderByDescending(er => er.TotalScore)
            .ToListAsync();

        for (int i = 0; i < results.Count; i++)
        {
            results[i].ClassRank = i + 1;
        }

        await _context.SaveChangesAsync();
    }
}
