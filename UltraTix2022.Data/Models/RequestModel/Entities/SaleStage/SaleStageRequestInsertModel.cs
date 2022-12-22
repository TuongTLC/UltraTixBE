using UltraTix2022.API.UltraTix2022.Data.Models.RequestModel.Entities.SaleStageDetail;

namespace UltraTix2022.API.UltraTix2022.Data.Models.RequestModel.Entities.SaleStage
{
    public class SaleStageRequestInsertModel
    {
        public string SaleStageOrder { get; set; } = null!;
        public string SaleStageDescription { get; set; } = null!;
        public DateTime SaleStageStartDate { get; set; }
        public DateTime SaleStageEndDate { get; set; }
        public double SaleStageDiscount { get; set; }

        public List<SaleStageDetailInsertRequestModel> SaleStageDetails { get; set; } = new();
    }

    public class SaleStageRequestUpdateModel
    {
        public Guid? SaleStageId { get; set; }
        public string SaleStageOrder { get; set; } = null!;
        public string SaleStageDescription { get; set; } = null!;
        public DateTime SaleStageStartDate { get; set; }
        public DateTime SaleStageEndDate { get; set; }
        public double SaleStageDiscount { get; set; }
        public bool IsDeleted { get; set; } = false;

        public List<SaleStageDetailUpdateRequestModel> SaleStageDetails { get; set; } = new();
    }
}
