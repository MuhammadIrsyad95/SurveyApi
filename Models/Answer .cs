using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Answer
{
    [Key]
    public Guid Id { get; set; }

    [Required]
    public Guid ResponseId { get; set; }

    [ForeignKey(nameof(ResponseId))]
    public Response Response { get; set; } = null!;

    [Required]
    public Guid QuestionId { get; set; }

    [ForeignKey(nameof(QuestionId))]
    public Question Question { get; set; } = null!;

    public Guid? ChoiceId { get; set; }

    [ForeignKey(nameof(ChoiceId))]
    public Choice? Choice { get; set; }

    public string? AnswerText { get; set; }
}
