namespace UltraTix2022.API.UltraTix2022.Data.Models.RequestModel.ShowReview
{
    public class ShowReviewInsertRequestModel
    {
        public Guid ShowId { get; set; }
        public string ReviewMessage { get; set; } = string.Empty;
        public double? Rate { get; set; } = 1;
    }
}
