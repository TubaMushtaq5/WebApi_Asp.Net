using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DL.DbModels;

[Route("api/[controller]")]
[ApiController]
public class StudentsController : ControllerBase
{
    private readonly StudentDbContext _context;

    public StudentsController(StudentDbContext context)
    {
        _context = context;
    }

    // POST: api/students
    [HttpPost]
    public async Task<IActionResult> CreateStudent([FromBody] StudentDbDto studentDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        _context.Students.Add(studentDto);
        await _context.SaveChangesAsync();
        return Ok(studentDto);
    }

    // PUT: api/students/{student_id}
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateStudent(int id, [FromBody] StudentDbDto updatedStudentDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        if (id != updatedStudentDto.Id)
        {
            return BadRequest("Mismatched ID in the request path and payload.");
        }

        var existingStudent = await _context.Students.FindAsync(id);

        if (existingStudent == null)
        {
            return NotFound();
        }

        // Update properties with the values from the updatedStudentDto
        existingStudent.Name = updatedStudentDto.Name;
        existingStudent.RollNumber = updatedStudentDto.RollNumber;
        existingStudent.PhoneNumber = updatedStudentDto.PhoneNumber;

        _context.Entry(existingStudent).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!StudentExists(id))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }

        return Ok(existingStudent);
    }
    // DELETE: api/students/{student_id}
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteStudent(int id)
    {
        var student = await _context.Students.FindAsync(id);

        if (student == null)
        {
            return NotFound();
        }

        _context.Students.Remove(student);
        await _context.SaveChangesAsync();

        return Ok(student);
    }

    // GET: api/students/{student_id}
    [HttpGet("{id}")]
    public async Task<IActionResult> GetStudent(int id)
    {
        var student = await _context.Students.FindAsync(id);

        if (student == null)
        {
            return NotFound();
        }

        return Ok(student);
    }

    // GET: api/students
    [HttpGet]
    public async Task<IActionResult> GetAllStudents()
    {
        var students = await _context.Students.ToListAsync();

        if (students == null || students.Count == 0)
        {
            return NotFound("No students found.");
        }

        return Ok(students);
    }




    private bool StudentExists(int id)
    {
        return _context.Students.Any(e => e.Id == id);
    }

}
