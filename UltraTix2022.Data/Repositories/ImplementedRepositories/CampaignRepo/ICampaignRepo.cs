using UltraTix2022.API.UltraTix2022.Data.Models.Entities;
using UltraTix2022.API.UltraTix2022.Data.Models.RequestModel.Entities.Campaign;
using UltraTix2022.API.UltraTix2022.Data.Models.ViewModels.Entities.Campaign;
using UltraTix2022.Data.Repositories.GenericRepository;

namespace UltraTix2022.API.UltraTix2022.Data.Repositories.ImplementedRepositories.CampaignRepo
{
    public interface ICampaignRepo : IRepository<Campaign>
    {
        public Task<List<CampaignViewModel>> GetCampaignsByShowID(Guid ShowID);
        public Task<bool> CheckCampaignExist(Guid camID);
        //public Task<bool> UpdateCampaign(CampaignRequestUpdateModel campaign);
        public Task<string> GetArtistImgURLByCampaignID(Guid campaignID);
        public Task<string> GetArtistNameByCampaignID(Guid campaignID);
        public Task<bool> UpdateBookingLink(Guid campaignId, string bookingLink);
        public Task<bool> RemoveCampaign(Guid campaignId);
    }
}
