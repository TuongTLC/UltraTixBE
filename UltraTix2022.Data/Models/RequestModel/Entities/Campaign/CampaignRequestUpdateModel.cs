using UltraTix2022.API.UltraTix2022.Data.Models.RequestModel.Entities.CampaignDetail;

namespace UltraTix2022.API.UltraTix2022.Data.Models.RequestModel.Entities.Campaign
{
    public class CampaignRequestUpdateModelOld
    {
        public Guid ID { get; set; }
        public double MinDiscount { get; set; }
        public double MaxDiscount { get; set; }
        public Guid ArtistId { get; set; }
        public Guid ShowId { get; set; }
        //public List<CampaignDetailRequestUpdateModel> CampaignDetails { get; set; } = new();
    }
}
