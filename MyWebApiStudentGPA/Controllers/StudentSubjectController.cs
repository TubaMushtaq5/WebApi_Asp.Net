using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DL.DbModels;
using System.Threading.Tasks;

[Route("api/[controller]")]
[ApiController]
public class StudentSubjectController : ControllerBase
{
    private readonly StudentDbContext _context;

    public StudentSubjectController(StudentDbContext context)
    {
        _context = context;
    }

    // Other actions for retrieving student-subject assignments can be added as needed.

    //// POST: api/student-subjects
    //[HttpPost]
    //public async Task<IActionResult> AssignSubjectToStudent([FromBody] StudentSubjectDbDto assignmentDto)
    //{
    //    if (!ModelState.IsValid)
    //    {
    //        return BadRequest(ModelState);
    //    }

    //    // Ensure that the provided student_id and subject_id exist in the database
    //    var student = await _context.Students.FindAsync(assignmentDto.StudentId);
    //    var subject = await _context.Subjects.FindAsync(assignmentDto.SubjectId);

    //    if (student == null || subject == null)
    //    {
    //        return NotFound("Student or subject not found.");
    //    }

    //    // Check if the assignment already exists
    //    if (_context.StudentSubjects.Any(ss => ss.StudentId == assignmentDto.StudentId && ss.SubjectId == assignmentDto.SubjectId))
    //    {
    //        return BadRequest("The assignment already exists.");
    //    }

    //    _context.StudentSubjects.Add(assignmentDto);
    //    await _context.SaveChangesAsync();

    //    return Ok(assignmentDto);
    //}


    // POST: api/student-subjects
    [HttpPost]
    public async Task<IActionResult> AssignSubjectToStudent([FromBody] StudentSubjectDbDto assignmentDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        // Ensure that the provided student_id and subject_id exist in the database
        var student = await _context.Students.FindAsync(assignmentDto.StudentId);
        var subject = await _context.Subjects.FindAsync(assignmentDto.SubjectId);

        if (student == null || subject == null)
        {
            return NotFound("Student or subject not found.");
        }

        // Check if the assignment already exists
        if (_context.StudentSubjects.Any(ss => ss.StudentId == assignmentDto.StudentId && ss.SubjectId == assignmentDto.SubjectId))
        {
            return BadRequest("The assignment already exists.");
        }

        _context.StudentSubjects.Add(assignmentDto);
        await _context.SaveChangesAsync();

        return Ok(assignmentDto);
    }


    // PUT: api/student-subjects/{assignment_id}
    [HttpPut("{assignment_id}")]
    public async Task<IActionResult> EditAssignment(int assignment_id, [FromBody] StudentSubjectDbDto updatedAssignmentDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        if (assignment_id != updatedAssignmentDto.Id)
        {
            return BadRequest("Mismatched ID in the request path and payload.");
        }

        // Ensure that the provided student_id and subject_id exist in the database
        var student = await _context.Students.FindAsync(updatedAssignmentDto.StudentId);
        var subject = await _context.Subjects.FindAsync(updatedAssignmentDto.SubjectId);

        if (student == null || subject == null)
        {
            return NotFound("Student or subject not found.");
        }

        var existingAssignment = await _context.StudentSubjects.FindAsync(assignment_id);

        if (existingAssignment == null)
        {
            return NotFound("Assignment not found.");
        }

        // Update properties with the values from the updatedAssignmentDto
        existingAssignment.StudentId = updatedAssignmentDto.StudentId;
        existingAssignment.SubjectId = updatedAssignmentDto.SubjectId;
        existingAssignment.marks = updatedAssignmentDto.marks; // Update the marks property, not GPA

        _context.Entry(existingAssignment).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!AssignmentExists(assignment_id))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }

        return Ok(existingAssignment);

    }
    // DELETE: api/student-subjects/{assignment_id}
    [HttpDelete("{assignment_id}")]
    public async Task<IActionResult> DeleteAssignment(int assignment_id)
    {
        var assignment = await _context.StudentSubjects.FindAsync(assignment_id);

        if (assignment == null)
        {
            return NotFound("Assignment not found.");
        }

        _context.StudentSubjects.Remove(assignment);
        await _context.SaveChangesAsync();

        return NoContent();
    }
    // GET: api/students/{student_id}/subjects
    [HttpGet("{student_id}/subjects")]
    public async Task<IActionResult> GetStudentSubjects(int student_id)
    {
        var student = await _context.Students.FindAsync(student_id);

        if (student == null)
        {
            return NotFound("Student not found.");
        }

        var studentSubjects = await _context.StudentSubjects
            .Where(ss => ss.StudentId == student_id)
            .Select(ss => new
            {
                SubjectId = ss.SubjectId,
                SubjectName = ss.Subject.Name, // Assuming you have a navigation property from StudentSubject to Subject
                GPA = ss.GPA
                // Add other properties as needed
            })
            .ToListAsync();

        return Ok(studentSubjects);
    }
    private bool AssignmentExists(int id)
    {
        return _context.StudentSubjects.Any(e => e.Id == id);
    }
}
