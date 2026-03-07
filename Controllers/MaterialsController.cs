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
public class MaterialsController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public MaterialsController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: api/materials
    [HttpGet]
    public async Task<ActionResult<IEnumerable<MaterialResponse>>> GetMaterials([FromQuery] Guid? classId)
    {
        var query = _context.Materials.Include(m => m.Class).AsQueryable();

        if (classId.HasValue)
        {
            query = query.Where(m => m.ClassId == classId.Value);
        }

        var materials = await query
            .OrderByDescending(m => m.IssueDate)
            .Select(m => new MaterialResponse
            {
                Id = m.Id,
                ClassId = m.ClassId,
                ClassName = m.Class.Name,
                Title = m.Title,
                MaterialType = m.MaterialType,
                IssueDate = m.IssueDate,
                Description = m.Description,
                CreatedAt = m.CreatedAt
            })
            .ToListAsync();

        return Ok(materials);
    }

    // GET: api/materials/{id}
    [HttpGet("{id}")]
    public async Task<ActionResult<MaterialResponse>> GetMaterial(Guid id)
    {
        var material = await _context.Materials
            .Include(m => m.Class)
            .FirstOrDefaultAsync(m => m.Id == id);

        if (material == null)
        {
            return NotFound(new { Message = "Material not found." });
        }

        return Ok(new MaterialResponse
        {
            Id = material.Id,
            ClassId = material.ClassId,
            ClassName = material.Class.Name,
            Title = material.Title,
            MaterialType = material.MaterialType,
            IssueDate = material.IssueDate,
            Description = material.Description,
            CreatedAt = material.CreatedAt
        });
    }

    // POST: api/materials/issue
    [HttpPost("issue")]
    public async Task<ActionResult> IssueMaterial([FromBody] IssueMaterialRequest request)
    {
        var student = await _context.Students.FindAsync(request.StudentId);
        if (student == null || student.Status != "active")
        {
            return NotFound(new { Message = "Active student not found." });
        }

        var material = await _context.Materials.FindAsync(request.MaterialId);
        if (material == null)
        {
            return NotFound(new { Message = "Material not found." });
        }

        // Check if already issued
        var alreadyIssued = await _context.MaterialIssues
            .AnyAsync(mi => mi.StudentId == request.StudentId && mi.MaterialId == request.MaterialId);

        if (alreadyIssued)
        {
            return BadRequest(new { Message = "This material has already been issued to the student." });
        }

        var issue = new MaterialIssue
        {
            Id = Guid.NewGuid(),
            StudentId = request.StudentId,
            MaterialId = request.MaterialId,
            IssuedAt = DateTimeOffset.UtcNow,
            IssuedBy = request.IssuedBy,
            Notes = request.Notes
        };

        _context.MaterialIssues.Add(issue);
        await _context.SaveChangesAsync();

        return Ok(new { Message = "Material issued successfully.", IssueId = issue.Id });
    }
}
