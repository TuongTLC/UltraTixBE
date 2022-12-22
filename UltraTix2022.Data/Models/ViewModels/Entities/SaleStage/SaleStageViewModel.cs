namespace UltraTix2022.API.UltraTix2022.Data.Models.ViewModels.Entities.SaleStage
{
    public class SaleStageViewModel
    {
        public Guid Id { get; set; }
        public string SaleStageOrder { get; set; } = null!;
        public string SaleStageDescription { get; set; } = null!;
        public DateTime SaleStageStartDate { get; set; }
        public DateTime SaleStageEndDate { get; set; }
        public double SaleStageDiscount { get; set; }
        public Guid ShowId { get; set; }
        public string startDate { get; set; } = string.Empty;
        public string endDate { get; set; } = string.Empty;

        public List<SaleStageDetailViewModel> SaleStageDetailViewModels { get; set; } = new();
    }
}
