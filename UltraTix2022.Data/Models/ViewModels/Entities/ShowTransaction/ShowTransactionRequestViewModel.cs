namespace UltraTix2022.API.UltraTix2022.Data.Models.ViewModels.Entities.ShowTransaction
{
    public class ShowTransactionRequestViewModel
    {
        public Guid Id { get; set; }
        public Guid ShowId { get; set; }
        public string ShowName { get; set; } = string.Empty;
        public Guid ShowOrderId { get; set; }
        public Guid CustomerId { get; set; }
        public string CustomerName { get; set; } = string.Empty;
        public Guid? CampaignId { get; set; }
        public int TotalTicketsBuy { get; set; } = 0;
        public double Amount { get; set; } = 0;
        public bool IsBuyViaArtistLink { get; set; } = false;
        public double ArtistCommission { get; set; } = 0;
        public double Revenue { get; set; } = 0;
        public string ArtistName { get; set; } = string.Empty;
        public DateTime TransactionDate { get; set; } = DateTime.Now;
    }
}
