using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Question
{
    [Key]
    public Guid Id { get; set; }

    [Required]
    public Guid FormId { get; set; }

    [ForeignKey("FormId")]
    public Form? Form { get; set; }

    public string? Text { get; set; }

    public string? Type { get; set; } // e.g., text, multiple_choice, checkbox

    public bool IsRequired { get; set; } = false;

    public ICollection<Choice>? Choices { get; set; }
    public ICollection<Answer>? Answers { get; set; }
}
