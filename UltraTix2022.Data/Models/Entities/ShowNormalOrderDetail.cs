namespace UltraTix2022.API.UltraTix2022.Data.Models.Entities
{
    public partial class ShowNormalOrderDetail
    {
        public Guid Id { get; set; }
        public Guid TicketTypeId { get; set; }
        public int Quantity { get; set; }
        public Guid SaleStageId { get; set; }
        public Guid ShowNormalOrderId { get; set; }
        public double SubTotal { get; set; }
        public string Description { get; set; } = null!;

        public virtual SaleStage SaleStage { get; set; } = null!;
        public virtual ShowNormalOrder ShowNormalOrder { get; set; } = null!;
        public virtual TicketType TicketType { get; set; } = null!;
    }
}
