using UltraTix2022.API.UltraTix2022.Data.Models.ViewModels.Entities.CampaignDetail;

namespace UltraTix2022.API.UltraTix2022.Data.Models.ViewModels.Entities.Campaign
{
    public class CampaignViewModelForCustomer
    {
        public Guid Id { get; set; }
        public Guid ArtistId { get; set; }
        public string ArtistName { get; set; } = string.Empty;

        public List<CampaignDetailViewModelForCustomer> CampaignDetails { get; set; } = new();
    }

    public class CampaignViewModelForCustomerForMobile
    {
        public Guid Id { get; set; }
        public Guid ArtistId { get; set; }
        public string ArtistName { get; set; }

        //public List<CampaignDetailViewModelForCustomer> CampaignDetails { get; set; } = new();
    }
}
