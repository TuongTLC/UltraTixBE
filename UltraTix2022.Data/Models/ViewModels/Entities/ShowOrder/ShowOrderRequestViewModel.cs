namespace UltraTix2022.API.UltraTix2022.Data.Models.ViewModels.Entities.ShowOrder
{
    public class ShowOrderRequestViewModel
    {
        public Guid Id { get; set; }
        public string OrderDescription { get; set; } = string.Empty;
        public DateTime OrderDate { get; set; } = DateTime.Now;
        public double TotalPay { get; set; } = 0;
        public Guid CustomerId { get; set; }
        public string CustomerName { get; set; } = string.Empty;
        public Guid ShowId { get; set; }
        public string ShowName { get; set; } = string.Empty;
        public DateTime ShowStartDate { get; set; }
        public DateTime ShowEndDate { get; set; }
        public Guid? CampaignId { get; set; }
        public string BuyFromArtist { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
        public bool IsUsed { get; set; } = false;
        public List<ShowOrderDetailViewRequestModel> ShowOrderDetailViewRequestModels { get; set; } = new();
    }
}
