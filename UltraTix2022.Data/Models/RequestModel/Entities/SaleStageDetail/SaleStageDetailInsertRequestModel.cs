namespace UltraTix2022.API.UltraTix2022.Data.Models.RequestModel.Entities.SaleStageDetail
{
    public class SaleStageDetailInsertRequestModel
    {
        public Guid TicketTypeId { get; set; }
        public int TicketTypeQuantity { get; set; } = 0;
    }

    public class SaleStageDetailUpdateRequestModel
    {
        public Guid SaleStageDetailId { get; set; }
        public Guid TicketTypeId { get; set; }
        public int TicketTypeQuantity { get; set; } = 0;
    }
}
