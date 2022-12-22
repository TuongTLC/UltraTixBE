namespace UltraTix2022.API.UltraTix2022.Data.Models.ViewModels.Entities.CampaignDetail
{
    public class CampaignDetailViewModel
    {
        public Guid Id { get; set; }
        public string CampaignName { get; set; } = null!;
        public string CampaignDescription { get; set; } = null!;
        public double ArtistDiscount { get; set; }
        public DateTime CampaignStartDate { get; set; }
        public DateTime CampaignEndDate { get; set; }
        public int TicketTypeQuantity { get; set; }
        public int TicketTypeSold { get; set; }
        public Guid TicketTypeId { get; set; }
        public string TicketTypeName { get; set; } = null!;
        public Guid CampaignId { get; set; }
        public Guid SaleStageId { get; set; }
        public string SaleStageName { get; set; } = null!;
        public double CustomerDiscount { get; set; } = 0;
        public Guid SaleStageDetailId { get; set; }
    }
}
