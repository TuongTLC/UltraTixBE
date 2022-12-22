using UltraTix2022.API.UltraTix2022.Data.Models.Entities;
using UltraTix2022.API.UltraTix2022.Data.Models.RequestModel.Entities.CampaignDetail;
using UltraTix2022.API.UltraTix2022.Data.Models.ViewModels.Entities.CampaignDetail;
using UltraTix2022.Data.Repositories.GenericRepository;

namespace UltraTix2022.API.UltraTix2022.Data.Repositories.ImplementedRepositories.CampaignDetailRepo
{
    public interface ICampaignDetailRepo : IRepository<CampaignDetail>
    {
        public Task<List<CampaignDetailViewModel>> GetCampaignDetailsByCampaignID(Guid CampaignID);
        public Task<bool> UpdateCampaignDetail(CampaignDetailRequestUpdateModel campaignDetail);
        public Task<int> UpdateUnitSold(Guid campaignDetailId, int unitSold);
        public Task<int> GetTotalTicketsBySaleStageDetailId(Guid saleStageDetailId);
        public Task<List<CampaignDetail>> GetCampaignDetailBySaleStageDetailId(Guid saleStageDetailId);
        public Task<List<CampaignDetail>> GetCampaignDetailsByCampaignId(Guid campaignId);
        public Task<bool> RemoveCampaignDetails(List<CampaignDetail> campaignDetails);
        public Task<int> ReOpenUnitSold(Guid campaignDetailId, int unitSold);
    }

}
