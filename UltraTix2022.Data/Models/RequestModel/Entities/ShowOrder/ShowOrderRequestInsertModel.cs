namespace UltraTix2022.API.UltraTix2022.Data.Models.RequestModel.Entities.ShowOrder
{
    public class ShowOrderRequestInsertModel : ShowOrderRequestModel
    {
        public Guid CustomerId { get; set; }
        public int ResultCode { get; set; } = -1;
    }
}
