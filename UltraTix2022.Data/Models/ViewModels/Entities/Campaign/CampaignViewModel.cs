using UltraTix2022.API.UltraTix2022.Data.Models.ViewModels.Entities.CampaignDetail;

namespace UltraTix2022.API.UltraTix2022.Data.Models.ViewModels.Entities.Campaign
{
    public class CampaignViewModel
    {
        public Guid Id { get; set; }
        public double MinDiscount { get; set; }
        public double MaxDiscount { get; set; }
        public Guid ArtistId { get; set; }
        public Guid ShowId { get; set; }
        public string BookingLink { get; set; } = string.Empty;

        public List<CampaignDetailViewModel> CampaignDetails { get; set; } = new();
    }
}
