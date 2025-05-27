using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Choice
{
    [Key]
    public Guid Id { get; set; }

    [Required]
    public Guid QuestionId { get; set; }

    [ForeignKey("QuestionId")]
    public Question? Question { get; set; }

    public string? Text { get; set; }
}
