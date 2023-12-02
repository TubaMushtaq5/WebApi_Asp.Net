using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DL.DbModels;
using System.Linq;
using System.Threading.Tasks;

[Route("api/[controller]")]
[ApiController]
public class MarksController : ControllerBase
{
    private readonly StudentDbContext _context;

    public MarksController(StudentDbContext context)
    {
        _context = context;
    }

    // GET: api/students/{student_id}/subjects/{subject_id}/marks
    [HttpGet("{student_id}/subjects/{subject_id}/marks")]
    public async Task<IActionResult> GetStudentSubjectMarks(int student_id, int subject_id)
    {
        var studentSubject = await _context.StudentSubjects
            .Include(ss => ss.Student)
            .FirstOrDefaultAsync(ss => ss.StudentId == student_id && ss.SubjectId == subject_id);

        if (studentSubject == null)
        {
            return NotFound("Student-Subject assignment not found.");
        }

        // Access Marks directly from the StudentSubjectDbDto
        var marks = studentSubject.marks;

        return Ok(new { Marks = marks });
    }
    // GET: api/students/{student_id}/marks
    [HttpGet("{student_id}/marks")]
    public async Task<IActionResult> GetAllStudentMarks(int student_id)
    {
        var studentSubjects = await _context.StudentSubjects
            .Where(ss => ss.StudentId == student_id)
            .Select(ss => new
            {
                SubjectId = ss.SubjectId,
                SubjectName = ss.Subject.Name, // Assuming you have a navigation property from StudentSubject to Subject
                Marks = ss.marks,
                  GPA = ss.GPA
                // Add other properties as needed
            })
            .ToListAsync();

        return Ok(studentSubjects);
    }
    // GET: api/students/{student_id}/gpa
    [HttpGet("{student_id}/gpa")]
    public async Task<IActionResult> GetStudentGPA(int student_id)
    {
        var studentSubjects = await _context.StudentSubjects
            .Where(ss => ss.StudentId == student_id)
            .ToListAsync();

        if (studentSubjects == null || studentSubjects.Count == 0)
        {
            return NotFound("No subjects found for the student.");
        }

        // Calculate GPA (you need to define your GPA calculation logic here)
        var gpa = studentSubjects.Average(ss => ss.GPA);

        return Ok(new { GPA = gpa });
    }



}
