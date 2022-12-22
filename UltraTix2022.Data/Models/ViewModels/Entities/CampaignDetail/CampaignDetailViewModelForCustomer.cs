namespace UltraTix2022.API.UltraTix2022.Data.Models.ViewModels.Entities.CampaignDetail
{
    public class CampaignDetailViewModelForCustomer
    {
        public Guid Id { get; set; }
        public int TicketTypeQuantity { get; set; }
        public int TicketTypeLeft { get; set; }
        public Guid TicketTypeId { get; set; }
        public string TicketTypeName { get; set; } = null!;
        public Guid SaleStageId { get; set; }
        public string SaleStageName { get; set; } = null!;
        public double CustomerDiscount { get; set; } = 0;
        public Guid SaleStageDetailId { get; set; }
    }
}
