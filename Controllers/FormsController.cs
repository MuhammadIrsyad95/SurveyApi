using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SurveyApi.Data;

namespace SurveyApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FormsController : ControllerBase
    {
        private readonly ApplicationDbContext _db;

        public FormsController(ApplicationDbContext context)
        {
            _db = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetForms()
        {
            var forms = await _db.Forms
                .Include(f => f.Questions)
                .ToListAsync();
            return Ok(forms);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetForm(Guid id)
        {
            var form = await _db.Forms
                .Include(f => f.Questions)
                    .ThenInclude(q => q.Choices)
                .Include(f => f.Responses)
                    .ThenInclude(r => r.Answers)
                        .ThenInclude(a => a.Choice)
                .FirstOrDefaultAsync(f => f.Id == id);

            if (form == null)
                return NotFound();

            return Ok(form);
        }

        [HttpPost]
        public async Task<IActionResult> CreateForm([FromBody] Form form)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            form.Id = Guid.NewGuid();
            form.CreatedAt = DateTime.Now;

            _db.Forms.Add(form);
            await _db.SaveChangesAsync();
            return Ok(form);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateForm(Guid id, [FromBody] Form form)
        {
            if (id != form.Id)
                return BadRequest("ID tidak cocok.");

            var existing = await _db.Forms
                .Include(f => f.Questions)
                    .ThenInclude(q => q.Choices)
                .FirstOrDefaultAsync(f => f.Id == id);

            if (existing == null)
                return NotFound();

            // Update header form
            existing.Title = form.Title;
            existing.Description = form.Description;

            // Sinkronisasi questions
            // 1. Hapus questions yang sudah tidak ada
            var questionIdsInput = form.Questions.Select(q => q.Id).ToList();
            var questionsToRemove = existing.Questions.Where(q => !questionIdsInput.Contains(q.Id)).ToList();
            foreach (var q in questionsToRemove)
                _db.Questions.Remove(q);

            // 2. Update atau tambah questions baru
            foreach (var qInput in form.Questions)
            {
                var qExisting = existing.Questions.FirstOrDefault(q => q.Id == qInput.Id);

                if (qExisting == null)
                {
                    // Question baru
                    var newQuestion = new Question
                    {
                        Id = Guid.NewGuid(),
                        Text = qInput.Text,
                        Type = qInput.Type,
                        IsRequired = qInput.IsRequired,
                        FormId = id,
                        Choices = new List<Choice>()
                    };
                    // Tambah choices
                    foreach (var c in qInput.Choices)
                    {
                        newQuestion.Choices.Add(new Choice
                        {
                            Id = Guid.NewGuid(),
                            Text = c.Text
                        });
                    }
                    existing.Questions.Add(newQuestion);
                }
                else
                {
                    // Update question lama
                    qExisting.Text = qInput.Text;
                    qExisting.Type = qInput.Type;
                    qExisting.IsRequired = qInput.IsRequired;

                    // Sinkronisasi choices
                    var choiceIdsInput = qInput.Choices.Select(c => c.Id).ToList();
                    var choicesToRemove = qExisting.Choices.Where(c => !choiceIdsInput.Contains(c.Id)).ToList();
                    foreach (var c in choicesToRemove)
                        _db.Choices.Remove(c);

                    foreach (var cInput in qInput.Choices)
                    {
                        var cExisting = qExisting.Choices.FirstOrDefault(c => c.Id == cInput.Id);
                        if (cExisting == null)
                        {
                            qExisting.Choices.Add(new Choice
                            {
                                Id = Guid.NewGuid(),
                                Text = cInput.Text
                            });
                        }
                        else
                        {
                            cExisting.Text = cInput.Text;
                        }
                    }
                }
            }

            await _db.SaveChangesAsync();
            return Ok(existing);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteForm(Guid id)
        {
            var form = await _db.Forms.FindAsync(id);
            if (form == null)
                return NotFound();

            _db.Forms.Remove(form);
            await _db.SaveChangesAsync();
            return NoContent();
        }
    }
}
