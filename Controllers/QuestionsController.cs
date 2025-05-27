using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SurveyApi.Data;

[Route("api/[controller]")]
[ApiController]
public class QuestionsController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public QuestionsController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet("{formId}")]
    public async Task<IActionResult> GetQuestionsByForm(Guid formId)
    {
        var questions = await _context.Questions
            .Where(q => q.FormId == formId)
            .Include(q => q.Choices)
            .ToListAsync();

        return Ok(questions);
    }

    [HttpPost]
    public async Task<IActionResult> CreateQuestion([FromBody] Question question)
    {
        _context.Questions.Add(question);
        await _context.SaveChangesAsync();
        return Ok(question);
    }
}
