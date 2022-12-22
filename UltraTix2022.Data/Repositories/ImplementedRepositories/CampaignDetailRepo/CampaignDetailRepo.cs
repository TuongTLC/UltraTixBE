using Microsoft.EntityFrameworkCore;
using UltraTix2022.API.UltraTix2022.Data.Models.Entities;
using UltraTix2022.API.UltraTix2022.Data.Models.RequestModel.Entities.CampaignDetail;
using UltraTix2022.API.UltraTix2022.Data.Models.ViewModels.Entities.CampaignDetail;
using UltraTix2022.Data.Repositories.GenericRepository;

namespace UltraTix2022.API.UltraTix2022.Data.Repositories.ImplementedRepositories.CampaignDetailRepo
{
    public class CampaignDetailRepo : Repository<CampaignDetail>, ICampaignDetailRepo
    {
        public CampaignDetailRepo(UltraTixDBContext context) : base(context)
        {

        }

        public async Task<List<CampaignDetail>> GetCampaignDetailBySaleStageDetailId(Guid saleStageDetailId)
        {
            var query = from t in context.CampaignDetails
                        where t.SaleStageDetailId.Equals(saleStageDetailId)
                        select new { t };

            List<CampaignDetail> result = await query.Select(x => new CampaignDetail()
            {
                Id = x.t.Id,
            }).ToListAsync();

            return result;
        }

        public async Task<List<CampaignDetailViewModel>> GetCampaignDetailsByCampaignID(Guid campaignID)
        {
            var query = from t in context.CampaignDetails
                        where t.CampaignId.Equals(campaignID)
                        select new { t };

            List<CampaignDetailViewModel> result = await query.Select(x => new CampaignDetailViewModel()
            {
                Id = x.t.Id,
                ArtistDiscount = x.t.ArtistDiscount,
                CampaignDescription = x.t.CampaignDescription,
                CampaignName = x.t.CampaignName,
                CampaignStartDate = x.t.CampaignStartDate,
                CampaignEndDate = x.t.CampaignEndDate,
                CampaignId = x.t.CampaignId,
                TicketTypeQuantity = x.t.TicketTypeQuantity,
                TicketTypeSold = x.t.TicketTypeSold,
                CustomerDiscount = x.t.CustomerDiscount ?? 0,
                SaleStageDetailId = x.t.SaleStageDetailId ?? new Guid(),
            }).ToListAsync();

            return result;
        }

        public async Task<List<CampaignDetail>> GetCampaignDetailsByCampaignId(Guid campaignId)
        {
            var query = from t in context.CampaignDetails
                        where t.CampaignId.Equals(campaignId)
                        select new { t };

            List<CampaignDetail> result = await query.Select(x => new CampaignDetail()
            {
                Id = x.t.Id,
            }).ToListAsync();

            return result;
        }

        public async Task<int> GetTotalTicketsBySaleStageDetailId(Guid saleStageDetailId)
        {
            var totalTicketsSoldViaCampaign = 0;

            var query = from t in context.CampaignDetails
                        where t.SaleStageDetailId.Equals(saleStageDetailId)
                        select new { t };

            List<CampaignDetailViewModel> result = await query.Select(x => new CampaignDetailViewModel()
            {
                Id = x.t.Id,
                ArtistDiscount = x.t.ArtistDiscount,
                CampaignDescription = x.t.CampaignDescription,
                CampaignName = x.t.CampaignName,
                CampaignStartDate = x.t.CampaignStartDate,
                CampaignEndDate = x.t.CampaignEndDate,
                CampaignId = x.t.CampaignId,
                TicketTypeQuantity = x.t.TicketTypeQuantity,
                TicketTypeSold = x.t.TicketTypeSold,
                CustomerDiscount = x.t.CustomerDiscount ?? 0,
                SaleStageDetailId = x.t.SaleStageDetailId ?? new Guid(),
            }).ToListAsync();

            foreach (var campaignDetail in result)
            {
                totalTicketsSoldViaCampaign += campaignDetail.TicketTypeQuantity;
            }

            return totalTicketsSoldViaCampaign;
        }

        public async Task<bool> RemoveCampaignDetails(List<CampaignDetail> campaignDetails)
        {
            foreach (var campaignDetail in campaignDetails)
            {
                var detail = await Get(campaignDetail.Id);
                if (detail != null)
                {
                    context.CampaignDetails.Remove(detail);
                    await Update();
                }
            }
            return true;
        }

        public Task<bool> UpdateCampaignDetail(CampaignDetailRequestUpdateModel campaignDetail)
        {
            throw new NotImplementedException();
        }

        /*
public async Task<bool> UpdateCampaignDetail(CampaignDetailRequestUpdateModel campaignDetail)
{
   var campaignEnt = await Get(campaignDetail.ID);
   if (campaignEnt != null)
   {
       campaignEnt.CampaignStartDate = campaignDetail.CampaignStartDate;
       campaignEnt.CampaignEndDate = campaignDetail.CampaignEndDate;
       campaignEnt.CampaignDescription = campaignDetail.CampaignDescription;
       campaignEnt.CampaignName = campaignDetail.CampaignName;
       campaignEnt.TicketTypeQuantity = campaignDetail.TicketTypeQuantity;
       campaignEnt.ArtistDiscount = campaignDetail.ArtistDiscount;
       campaignEnt.CustomerDiscount = campaignDetail.CustomerDiscount;
       campaignEnt.SaleStageDetailId = campaignDetail.SaleStageDetailId;
       await Update();
       return true;
   }
   return false;
}
*/
        public async Task<int> UpdateUnitSold(Guid campaignDetailId, int unitSold)
        {
            var campaignDetailEntity = await Get(campaignDetailId);
            if (campaignDetailEntity != null)
            {
                campaignDetailEntity.TicketTypeSold += unitSold;
                await Update();
                return campaignDetailEntity.TicketTypeSold;
            }
            return -1;
        }

        public async Task<int> ReOpenUnitSold(Guid campaignDetailId, int unitSold)
        {
            var campaignDetailEntity = await Get(campaignDetailId);
            if (campaignDetailEntity != null)
            {
                campaignDetailEntity.TicketTypeSold -= unitSold;
                await Update();
                return campaignDetailEntity.TicketTypeSold;
            }
            return -1;
        }
    }
}
