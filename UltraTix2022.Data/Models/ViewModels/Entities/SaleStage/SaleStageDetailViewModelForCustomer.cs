namespace UltraTix2022.API.UltraTix2022.Data.Models.ViewModels.Entities.SaleStage
{
    public class SaleStageDetailViewModelForCustomer
    {
        public Guid Id { get; set; }
        public Guid TicketTypeId { get; set; }
        public string TicketTypeName { get; set; } = string.Empty;
        public int TicketTypeQuantity { get; set; } = 0;
        public int TicketTypeLeft { get; set; } = 0;
        public double OriginalPrice { get; set; } = 0;
        public double TicketTypeDiscount { get; set; } = 0;
    }
}
