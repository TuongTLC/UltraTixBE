namespace UltraTix2022.API.UltraTix2022.Data.Models.ViewModels.Entities.Feedback
{
    public class FeedbackViewModel
    {
        public Guid? Id { get; set; }
        public string? ReporterPhone { get; set; }
        public string? ReporterEmail { get; set; }
        public string? ProblemBrief { get; set; }
        public string? ProblemDetail { get; set; }
        public string? Status { get; set; }
        public string? FeedbackTypeName { get; set; }
    }
}

