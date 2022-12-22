namespace UltraTix2022.API.UltraTix2022.Data.Models.ViewModels.Entities.ShowOrder
{
    public class ShowOrderDetailViewRequestModel
    {
        public Guid Id { get; set; }
        public int QuantityBuy { get; set; } = 0;
        public double SubTotal { get; set; } = 0;
        public Guid ShowOrderId { get; set; }
        public string? Description { get; set; } = string.Empty;
        public Guid? CampaignDetailId { get; set; } = new Guid();
        public string CampaignName { get; set; } = string.Empty;
        public Guid? SaleStageDetailId { get; set; } = new Guid();
        public string SaleStageName { get; set; } = string.Empty;
        public string TicketTypeName { get; set; } = string.Empty;
        public double TicketTypeDiscount { get; set; } = 0;
        public double SaleStageDiscount { get; set; } = 0;
        public double CampaignDiscount { get; set; } = 0;
        public double OriginalPrice { get; set; } = 0;
    }
}
