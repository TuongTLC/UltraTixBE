namespace UltraTix2022.API.UltraTix2022.Data.Models.ViewModels.Entities.ShowReview
{
    public class ShowReviewViewModel
    {
        public Guid Id { get; set; }
        public Guid ShowId { get; set; }
        public Guid ReviewerId { get; set; }
        public string ReviewerName { get; set; } = null!;
        public string ReviewMessage { get; set; } = null!;
        public DateTime DateTimeReview { get; set; }
        public double? Rate { get; set; }
    }
}
