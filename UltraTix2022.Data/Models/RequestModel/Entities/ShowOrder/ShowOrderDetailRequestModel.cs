namespace UltraTix2022.API.UltraTix2022.Data.Models.RequestModel.Entities.ShowOrder
{
    public class ShowOrderDetailRequestModel
    {
        public Guid TicketTypeId { get; set; }
        public int QuantityBuy { get; set; }
        public Guid? SaleStageId { get; set; }
        public double SubTotal { get; set; }
        public Guid? CampaignDetailId { get; set; }
        public Guid? SaleStageDetailId { get; set; }
    }
}
