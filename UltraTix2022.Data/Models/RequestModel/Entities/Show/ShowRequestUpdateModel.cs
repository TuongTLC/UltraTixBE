using UltraTix2022.API.UltraTix2022.Data.Models.RequestModel.Entities.Campaign;
using UltraTix2022.API.UltraTix2022.Data.Models.RequestModel.Entities.Location;
using UltraTix2022.API.UltraTix2022.Data.Models.RequestModel.Entities.SaleStage;
using UltraTix2022.API.UltraTix2022.Data.Models.RequestModel.Entities.TicketType;

namespace UltraTix2022.API.UltraTix2022.Data.Models.RequestModel.Entities.Show
{
    public class ShowRequestUpdateModelOld
    {
        public Guid Id { get; set; }
        public string ShowName { get; set; } = string.Empty;
        public string ShowDescription { get; set; } = string.Empty;
        public string ShowDescriptionDetail { get; set; } = string.Empty;
        public DateTime ShowStartDate { get; set; }
        public DateTime ShowEndDate { get; set; }
        public Guid ShowTypeID { get; set; }
        public string ImgUrl { get; set; } = null!;
        public string DescriptionImageUrl { get; set; } = null!;
        public Guid? CategoryID { get; set; }

        public LocationRequestUpdateModel LocationUpdateModel { get; set; } = new();
        public List<TicketTypeRequestUpdateModel> TicketTypeUpdateModels { get; set; } = new();
        //public List<SaleStageRequestUpdateModel> SaleStageUpdateModels { get; set; } = new();
        //public List<CampaignRequestUpdateModel> CampaignUpdateModels { get; set; } = new();

    }
}
