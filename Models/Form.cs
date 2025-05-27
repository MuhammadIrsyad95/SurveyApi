using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Form
{
    [Key]
    public Guid Id { get; set; }

    public string? Title { get; set; }
    public string? Description { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public string? ImageUrl { get; set; }  

    public ICollection<Question>? Questions { get; set; }
    public ICollection<Response>? Responses { get; set; }
}
