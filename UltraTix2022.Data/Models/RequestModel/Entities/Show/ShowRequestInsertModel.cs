using UltraTix2022.API.UltraTix2022.Data.Models.RequestModel.Entities.Campaign;
using UltraTix2022.API.UltraTix2022.Data.Models.RequestModel.Entities.Location;
using UltraTix2022.API.UltraTix2022.Data.Models.RequestModel.Entities.SaleStage;
using UltraTix2022.API.UltraTix2022.Data.Models.RequestModel.Entities.TicketType;

namespace UltraTix2022.API.UltraTix2022.Data.Models.RequestModel.Entities.Show
{
    public class ShowRequestInsertModel
    {
        public string ShowName { get; set; } = string.Empty;
        public string ShowDescription { get; set; } = string.Empty;
        public string ShowDescriptionDetail { get; set; } = string.Empty;
        public DateTime ShowStartDate { get; set; }
        public DateTime ShowEndDate { get; set; }
        public Guid ShowType { get; set; }
        public string ImgUrl { get; set; } = null!;
        public string DescriptionImageUrl { get; set; } = null!;
        public Guid? CategoryID { get; set; }

        public LocationRequestInsertModel Location { get; set; } = new();

        public List<TicketTypeRequestInsertModel> TicketTypes { get; set; } = new();
    }

    public class ShowSaleStagesRequestInsertModel
    {
        public Guid ShowID { get; set; }
        public List<SaleStageRequestInsertModel>? SaleStages { get; set; } = new();
    }

    public class ShowCampaignsRequestInsertModel
    {
        public Guid ShowID { get; set; }
        public List<CampaignRequestInsertModel>? Campaigns { get; set; } = new();
    }

    public class ShowRequestUpdateModel
    {
        public Guid ShowID { get; set; }
        public string ShowName { get; set; } = string.Empty;
        public string ShowDescription { get; set; } = string.Empty;
        public string ShowDescriptionDetail { get; set; } = string.Empty;
        public DateTime ShowStartDate { get; set; }
        public DateTime ShowEndDate { get; set; }
        public Guid ShowType { get; set; }
        public string ImgUrl { get; set; } = null!;
        public string DescriptionImageUrl { get; set; } = null!;
        public Guid? CategoryID { get; set; }

        public LocationRequestUpdateModel Location { get; set; } = new();

        public List<TicketTypeRequestUpdateModel> TicketTypes { get; set; } = new();
    }

    public class LocationRequestUpdateModel
    {
        public Guid LocationID { get; set; }
        public string LocationDescription { get; set; } = string.Empty;
    }

    public class TicketTypeRequestUpdateModel
    {
        public Guid? TicketTypeID { get; set; }
        public string TicketTypeName { get; set; } = null!;
        public string TicketTypeDescription { get; set; } = null!;
        public double OriginalPrice { get; set; }
        public int Quantity { get; set; }
        public double TicketTypeDiscount { get; set; }
        public bool IsDeleted { get; set; } = false;
    }

    public class ShowSaleStagesRequestUpdateModel
    {
        public Guid ShowId { get; set; }
        public List<SaleStageRequestUpdateModel>? SaleStages { get; set; } = new();
    }

    public class ShowCampaignsRequestUpdateModel
    {
        public Guid ShowId { get; set; }
        public List<CampaignRequestUpdateModel>? Campaigns { get; set; } = new();
    }
}
