using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SurveyApi.Data;
using SurveyApi.DTO;

namespace SurveyApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ResponsesController : ControllerBase
    {
        private readonly ApplicationDbContext _db;

        public ResponsesController(ApplicationDbContext context)
        {
            _db = context;
        }

        [HttpPost]
        public async Task<IActionResult> SubmitResponse([FromBody] CreateResponseDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var response = new Response
            {
                Id = Guid.NewGuid(),
                FormId = dto.FormId,
                RespondentName = dto.RespondentName,
                SubmittedAt = DateTime.UtcNow,
                Answers = dto.Answers.Select(a => new Answer
                {
                    Id = Guid.NewGuid(),
                    QuestionId = a.QuestionId,
                    AnswerText = a.AnswerText,
                    ChoiceId = a.ChoiceId
                }).ToList()
            };

            _db.Responses.Add(response);
            await _db.SaveChangesAsync();

            return Ok(new { response.Id });
        }
        [HttpGet("{formId}")]
        public async Task<IActionResult> GetResponses(Guid formId)
        {
            var responses = await _db.Responses
                .Where(r => r.FormId == formId)
                .Include(r => r.Answers)
                    .ThenInclude(a => a.Question)
                .Include(r => r.Answers)
                    .ThenInclude(a => a.Choice)
                .ToListAsync();

            return Ok(responses);
        }
    }
}
