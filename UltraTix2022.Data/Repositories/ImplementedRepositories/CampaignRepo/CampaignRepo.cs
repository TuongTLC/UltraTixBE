using Microsoft.EntityFrameworkCore;
using UltraTix2022.API.UltraTix2022.Data.Models.Entities;
using UltraTix2022.API.UltraTix2022.Data.Models.RequestModel.Entities.Campaign;
using UltraTix2022.API.UltraTix2022.Data.Models.ViewModels.Entities.Campaign;
using UltraTix2022.Data.Repositories.GenericRepository;

namespace UltraTix2022.API.UltraTix2022.Data.Repositories.ImplementedRepositories.CampaignRepo
{
    public class CampaignRepo : Repository<Campaign>, ICampaignRepo
    {
        public CampaignRepo(UltraTixDBContext context) : base(context)
        {

        }

        public async Task<bool> CheckCampaignExist(Guid camID)
        {
            return (await Get(camID) != null);
        }

        public async Task<List<CampaignViewModel>> GetCampaignsByShowID(Guid ShowID)
        {
            var query = from t in context.Campaigns
                        where t.ShowId.Equals(ShowID)
                        select new { t };

            List<CampaignViewModel> result = await query.Select(x => new CampaignViewModel()
            {
                Id = x.t.Id,
                ArtistId = x.t.ArtistId,
                MaxDiscount = x.t.MaxDiscount,
                MinDiscount = x.t.MinDiscount,
                ShowId = x.t.ShowId,
                BookingLink = x.t.BookingLink ?? string.Empty
            }).ToListAsync();

            return result;
        }

        /*
        public async Task<bool> UpdateCampaign(CampaignRequestUpdateModel campaign)
        {
            var campaignEnt = await Get(campaign.CampaignId);
            if (campaignEnt != null)
            {
                campaignEnt.ArtistId = campaign.ArtistId;
                campaignEnt.MaxDiscount = campaign.MaxDiscount;
                campaignEnt.MinDiscount = campaign.MinDiscount;
                await Update();
                return true;
            }
            return false;
        }
        */
        public async Task<string> GetArtistImgURLByCampaignID(Guid campaignID)
        {
            var query = from c in context.Campaigns
                        join a in context.Artists
                        on c.ArtistId equals a.Id
                        join s in context.AppUsers
                        on a.Id equals s.Id
                        where c.Id.Equals(campaignID)
                        select new { c, a, s };

            var result = await query.Select(camp => camp.s.AvatarImageUrl).FirstAsync();

            return result ?? "";
        }

        public async Task<string> GetArtistNameByCampaignID(Guid campaignID)
        {
            var query = from c in context.Campaigns
                        join a in context.Artists
                        on c.ArtistId equals a.Id
                        join s in context.AppUsers
                        on a.Id equals s.Id
                        where c.Id.Equals(campaignID)
                        select new { c, a, s };

            var result = await query.Select(camp => camp.s.FullName).FirstAsync();

            return result ?? "";
        }

        public async Task<bool> UpdateBookingLink(Guid campaignId, string bookingLink)
        {
            var campaign = await Get(campaignId);

            if (campaign != null)
            {
                campaign.BookingLink = bookingLink;
                await Update();
                return true;
            }

            return false;
        }

        public async Task<bool> RemoveCampaign(Guid campaignId)
        {
            var campaign = await Get(campaignId);
            if(campaign != null)
            {
                context.Campaigns.Remove(campaign);
                await Update();
                return true;
            }
            return false;
        }
    }

}
