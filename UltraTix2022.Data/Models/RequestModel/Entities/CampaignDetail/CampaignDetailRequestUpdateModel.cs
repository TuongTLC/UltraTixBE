namespace UltraTix2022.API.UltraTix2022.Data.Models.RequestModel.Entities.CampaignDetail
{
    public class CampaignDetailRequestUpdateModelOld
    {
        public Guid ID { get; set; }
        public string CampaignName { get; set; } = null!;
        public string CampaignDescription { get; set; } = null!;
        public double ArtistDiscount { get; set; }
        public DateTime CampaignStartDate { get; set; }
        public DateTime CampaignEndDate { get; set; }
        public string TicketBookingPageLink { get; set; } = null!;
        public int TicketTypeQuantity { get; set; }
        public int TicketTypeSold { get; set; }
        public Guid TicketTypeID { get; set; }
        public string TicketTypeName { get; set; } = null!;
        public Guid CampaignId { get; set; }
        public Guid SaleStageID { get; set; }
        public string SaleStageName { get; set; } = null!;
        public double CustomerDiscount { get; set; } = 0;
        public Guid? SaleStageDetailId { get; set; }
    }
}
