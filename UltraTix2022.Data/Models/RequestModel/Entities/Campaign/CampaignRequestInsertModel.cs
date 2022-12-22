using UltraTix2022.API.UltraTix2022.Data.Models.RequestModel.Entities.CampaignDetail;

namespace UltraTix2022.API.UltraTix2022.Data.Models.RequestModel.Entities.Campaign
{
    public class CampaignRequestInsertModel
    {
        public double MinDiscount { get; set; }
        public double MaxDiscount { get; set; }
        public Guid ArtistId { get; set; }
        public List<CampaignDetailRequestInsertModel> CampaignDetails { get; set; } = new();
    }

    public class CampaignRequestUpdateModel
    {
        public Guid? CampaignId { get; set; }
        public double MinDiscount { get; set; }
        public double MaxDiscount { get; set; }
        public Guid ArtistId { get; set; }
        public List<CampaignDetailRequestUpdateModel> CampaignDetails { get; set; } = new();
        public bool IsDeleted { get; set; } = false;
    }
}
