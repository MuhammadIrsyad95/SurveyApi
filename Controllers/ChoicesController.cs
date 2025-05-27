using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SurveyApi.Data;

[Route("api/[controller]")]
[ApiController]
public class ChoicesController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public ChoicesController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet("{questionId}")]
    public async Task<IActionResult> GetChoicesByQuestion(Guid questionId)
    {
        var choices = await _context.Choices
            .Where(c => c.QuestionId == questionId)
            .ToListAsync();

        return Ok(choices);
    }

    [HttpPost]
    public async Task<IActionResult> CreateChoice([FromBody] Choice choice)
    {
        _context.Choices.Add(choice);
        await _context.SaveChangesAsync();
        return Ok(choice);
    }
}
