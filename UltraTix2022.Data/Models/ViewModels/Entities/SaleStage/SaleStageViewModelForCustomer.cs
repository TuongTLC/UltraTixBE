namespace UltraTix2022.API.UltraTix2022.Data.Models.ViewModels.Entities.SaleStage
{
    public class SaleStageViewModelForCustomer
    {
        public Guid Id { get; set; }
        public double SaleStageDiscount { get; set; }
        public DateTime SaleStageStartDate { get; set; }
        public DateTime SaleStageEndDate { get; set; }

        public List<SaleStageDetailViewModelForCustomer> SaleStageDetailViewModels { get; set; } = new();
    }

    /*
    public class SaleStageViewModelForCustomerForMobile
    {
        public Guid Id { get; set; }
        public double SaleStageDiscount { get; set; }
        public DateTime SaleStageStartDate { get; set; }
        public DateTime SaleStageEndDate { get; set; }

        public List<SaleStageDetailViewModelForCustomer> SaleStageDetailViewModels { get; set; } = new();
    }
    */
}
