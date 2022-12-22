namespace UltraTix2022.API.UltraTix2022.Data.Models.RequestModel.Entities.CampaignDetail
{
    public class CampaignDetailRequestInsertModel
    {
        public string CampaignName { get; set; } = null!;
        public string CampaignDescription { get; set; } = null!;
        public double ArtistDiscount { get; set; } = 0;
        public DateTime CampaignStartDate { get; set; }
        public DateTime CampaignEndDate { get; set; }
        public double TicketTypeQuantity { get; set; }
        public Guid SaleStageId { get; set; }
        public double CustomerDiscount { get; set; } = 0;
    }

    public class CampaignDetailRequestUpdateModel
    {
        public Guid CampaignDetailId { get; set; }
        public string CampaignName { get; set; } = null!;
        public string CampaignDescription { get; set; } = null!;
        public double ArtistDiscount { get; set; } = 0;
        public DateTime CampaignStartDate { get; set; }
        public DateTime CampaignEndDate { get; set; }
        public int TicketTypeQuantity { get; set; }
        public Guid SaleStageDetailId { get; set; }
        public double CustomerDiscount { get; set; } = 0;
    }
}
