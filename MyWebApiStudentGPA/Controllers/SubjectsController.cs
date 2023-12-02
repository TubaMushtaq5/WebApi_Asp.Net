using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DL.DbModels;

[Route("api/[controller]")]
[ApiController]
public class SubjectsController : ControllerBase
{
    private readonly StudentDbContext _context;

    public SubjectsController(StudentDbContext context)
    {
        _context = context;
    }

    // POST: api/subjects
    [HttpPost]
    public async Task<IActionResult> AddSubject([FromBody] SubjectDbDto subjectDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        _context.Subjects.Add(subjectDto);
        await _context.SaveChangesAsync();

        return Ok(subjectDto);
    }

    // GET: api/subjects/{id}
    [HttpGet("{id}")]
    public async Task<ActionResult<SubjectDbDto>> GetSubject(int id)
    {
        var subject = await _context.Subjects.FindAsync(id);

        if (subject == null)
        {
            return NotFound();
        }

        return subject;
    }
    // PUT: api/subjects/{subject_id}
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateSubject(int id, [FromBody] SubjectDbDto updatedSubjectDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        if (id != updatedSubjectDto.Id)
        {
            return BadRequest("Mismatched ID in the request path and payload.");
        }

        var existingSubject = await _context.Subjects.FindAsync(id);

        if (existingSubject == null)
        {
            return NotFound();
        }

        // Update properties with the values from the updatedSubjectDto
        existingSubject.Name = updatedSubjectDto.Name;
        // Add other properties to update as needed

        _context.Entry(existingSubject).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!SubjectExists(id))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }

        return Ok(existingSubject);
    }

    private bool SubjectExists(int id)
    {
        return _context.Subjects.Any(e => e.Id == id);
    }

    // DELETE: api/subjects/{subject_id}
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteSubject(int id)
    {
        var subject = await _context.Subjects.FindAsync(id);

        if (subject == null)
        {
            return NotFound();
        }

        _context.Subjects.Remove(subject);
        await _context.SaveChangesAsync();

        return Ok(subject);
    }
    // GET: api/subjects
    [HttpGet]
    public async Task<IActionResult> GetAllSubjects()
    {
        var subjects = await _context.Subjects.ToListAsync();

        if (subjects == null || subjects.Count == 0)
        {
            return NotFound("No subjects found.");
        }

        return Ok(subjects);
    }



    // Other actions for retrieving subjects, updating, and deleting can be added as needed.
}
