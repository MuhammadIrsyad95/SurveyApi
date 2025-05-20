using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SurveyApi.Data;

[Route("api/[controller]")]
[ApiController]
public class AnswersController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public AnswersController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet("{responseId}")]
    public async Task<IActionResult> GetAnswersByResponse(Guid responseId)
    {
        var answers = await _context.Answers
            .Where(a => a.ResponseId == responseId)
            .Include(a => a.Question)
            .Include(a => a.Choice)
            .ToListAsync();

        return Ok(answers);
    }

    [HttpPost]
    public async Task<IActionResult> SubmitAnswer([FromBody] Answer answer)
    {
        _context.Answers.Add(answer);
        await _context.SaveChangesAsync();
        return Ok(answer);
    }
}
