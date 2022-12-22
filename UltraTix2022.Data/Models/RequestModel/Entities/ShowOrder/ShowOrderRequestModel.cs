namespace UltraTix2022.API.UltraTix2022.Data.Models.RequestModel.Entities.ShowOrder
{
    public class ShowOrderRequestModel
    {
        public string OrderDescription { get; set; } = null!;
        public double TotalPay { get; set; }
        public Guid? CampaignId { get; set; }
        public Guid ShowId { get; set; }

        public List<ShowOrderDetailRequestModel> ShowOrderDetail { get; set; } = new();
    }
}
