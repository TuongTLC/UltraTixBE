using UltraTix2022.API.UltraTix2022.Data.Models.RequestModel.Entities.SaleStageDetail;

namespace UltraTix2022.API.UltraTix2022.Data.Models.RequestModel.Entities.SaleStage
{
    public class SaleStageRequestUpdateModelOld
    {
        public Guid ID { get; set; }
        public string SaleStageOrder { get; set; } = string.Empty;
        public string SaleStageDescription { get; set; } = string.Empty;
        public DateTime SaleStageStartDate { get; set; }
        public DateTime SaleStageEndDate { get; set; }
        public double SaleStageDiscount { get; set; } = 0;

        //public List<SaleStageDetailRequestUpdateModel> SaleStageDetails { get; set; } = new();
    }
}
