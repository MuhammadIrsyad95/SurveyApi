namespace SurveyApi.DTO
{
    public class CreateResponseDto
    {
        public Guid FormId { get; set; }
        public string RespondentName { get; set; } = string.Empty;
        public List<AnswerDto> Answers { get; set; } = new();
    }

    public class AnswerDto
    {
        public Guid QuestionId { get; set; }
        public string? AnswerText { get; set; }
        public Guid? ChoiceId { get; set; }
    }
}
