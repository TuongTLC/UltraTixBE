namespace UltraTix2022.API.UltraTix2022.Data.Models.ViewModels.Entities.Show
{
    public class DashBoardOverviewModel
    {
        public double TotalRevenueFromAllShows { get; set; } = 0.0;
        public int TotalPublicShows { get; set; } = 0;
        public int TotalTicketsSold { get; set; } = 0;
    }

    public class ShowAndRevenueViewModel
    {
        public Guid ShowId { get; set; }
        public string ShowName { get; set; } = string.Empty;
        public double TotalCustomerPaid { get; set; } = 0.0;
        public double ArtistCommission { get; set; } = 0.0;
        public double Revenue { get; set; } = 0.0;
        public bool IsAdminTransfer { get; set; } = false;
    }

    public class RevenueByMonthModel
    {
        public string Month { get; set; } = string.Empty;
        public double Revenue { get; set; } = 0.0;
    }
}
