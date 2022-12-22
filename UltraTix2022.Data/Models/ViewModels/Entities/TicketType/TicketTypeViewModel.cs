namespace UltraTix2022.API.UltraTix2022.Data.Models.ViewModels.Entities.TicketType
{
    public class TicketTypeViewModel
    {
        public Guid Id { get; set; }
        public string TicketTypeName { get; set; } = null!;
        public string TicketTypeDescription { get; set; } = null!;
        public double OriginalPrice { get; set; }
        public int Quantity { get; set; }
        public double TicketTypeDiscount { get; set; }
        public Guid ShowId { get; set; }
        public int UnitSold { get; set; }
    }
}
