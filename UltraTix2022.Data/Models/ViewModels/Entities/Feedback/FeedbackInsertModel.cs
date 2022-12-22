namespace UltraTix2022.API.UltraTix2022.Data.Models.ViewModels.Entities.Feedback
{
    public class FeedbackInsertModel
    {
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? ProblemBrief { get; set; }
        public string? ProblemDetail { get; set; }
        public Guid FeedbackTypeID { get; set; }
    }
}

