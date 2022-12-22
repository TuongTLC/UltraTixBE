namespace UltraTix2022.API.UltraTix2022.Data.Models.RequestModel.Entities.SaleStageDetail
{
    public class SaleStageDetailRequestUpdateModelOld : SaleStageDetailInsertRequestModel
    {
        public Guid SaleStageId { get; set; }
        public Guid TicketTypeId { get; set; }
    }
}
