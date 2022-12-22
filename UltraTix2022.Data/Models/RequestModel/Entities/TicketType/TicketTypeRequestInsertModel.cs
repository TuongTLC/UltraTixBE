namespace UltraTix2022.API.UltraTix2022.Data.Models.RequestModel.Entities.TicketType
{
    public class TicketTypeRequestInsertModel
    {
        public string TicketTypeName { get; set; } = null!;
        public string TicketTypeDescription { get; set; } = null!;
        public double OriginalPrice { get; set; }
        public int Quantity { get; set; }
        public double TicketTypeDiscount { get; set; }
    }
}
