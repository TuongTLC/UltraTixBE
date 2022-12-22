namespace UltraTix2022.API.UltraTix2022.Data.Models.Entities
{
    public partial class Ticket
    {
        public Guid Id { get; set; }
        public string TicketDescription { get; set; } = null!;
        public double TicketPrice { get; set; }
        public int Quantity { get; set; }
        public Guid TicketTypeId { get; set; }
        public Guid OrderId { get; set; }
        public Guid? CampaignDetailId { get; set; }

        public virtual CampaignDetail? CampaignDetail { get; set; }
        public virtual ShowOrder Order { get; set; } = null!;
        public virtual TicketType TicketType { get; set; } = null!;
    }
}
