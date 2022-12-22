namespace UltraTix2022.API.UltraTix2022.Data.Models.ViewModels.Entities.SaleStage
{
    public class SaleStageDetailViewModel
    {
        public Guid Id { get; set; }
        public Guid SaleStageId { get; set; }
        public Guid TicketTypeId { get; set; }
        public string SaleStageName { get; set; } = string.Empty;
        public string TicketTypeName { get; set; } = string.Empty;
        public int TicketTypeQuantity { get; set; } = 0;
        public int TicketTypeLeft { get; set; } = 0;
    }
}
