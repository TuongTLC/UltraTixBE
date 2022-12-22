using Microsoft.EntityFrameworkCore;
using UltraTix2022.API.UltraTix2022.Data.Models.Entities;
using UltraTix2022.API.UltraTix2022.Data.Models.RequestModel.Entities.SaleStageDetail;
using UltraTix2022.API.UltraTix2022.Data.Models.ViewModels.Entities.SaleStage;
using UltraTix2022.Data.Repositories.GenericRepository;

namespace UltraTix2022.API.UltraTix2022.Data.Repositories.ImplementedRepositories.SaleStageDetailRepo
{
    public class SaleStageDetailRepo : Repository<SaleStageDetail>, ISaleStageDetailRepo
    {
        public SaleStageDetailRepo(UltraTixDBContext context) : base(context)
        {

        }

        /*
        public async Task<string> GetNameByID(Guid ID)
        {
            var saleStageDet = await Get(ID);
            return saleStageDetail.SaleStageOrder;
        }
        */

        public async Task<List<SaleStageDetailViewModel>> GetSaleStageDetailBySaleStageID(Guid SaleStageID)
        {
            var query = from t in context.SaleStageDetails
                        where t.SaleStageId.Equals(SaleStageID)
                        select new { t };

            List<SaleStageDetailViewModel> result = await query.Select(x => new SaleStageDetailViewModel()
            {
                Id = x.t.Id,
                SaleStageId = x.t.SaleStageId,
                TicketTypeId = x.t.TicketTypeId,
                TicketTypeQuantity = x.t.TicketTypeQuantity,
                TicketTypeLeft = x.t.TicketTypeQuantity - x.t.TicketTypeQuantitySold,
            }).ToListAsync();

            return result;
        }

        public async Task<List<SaleStageDetail>> GetSaleStageDetailBySaleStageId(Guid SaleStageID)
        {
            var query = from t in context.SaleStageDetails
                        where t.SaleStageId.Equals(SaleStageID)
                        select new { t };

            List<SaleStageDetail> result = await query.Select(x => new SaleStageDetail()
            {
                Id = x.t.Id,
                SaleStageId = x.t.SaleStageId,
                TicketTypeId = x.t.TicketTypeId,
                TicketTypeQuantity = x.t.TicketTypeQuantity,
            }).ToListAsync();

            return result;
        }

        public async Task<List<SaleStageDetail>> GetSaleStageDetailByTicketTypeId(Guid ticketTypeId)
        {
            var query = from t in context.SaleStageDetails
                        where t.TicketTypeId.Equals(ticketTypeId)
                        select new { t };

            List<SaleStageDetail> result = await query.Select(x => new SaleStageDetail()
            {
                Id = x.t.Id,
                SaleStageId = x.t.SaleStageId,
                TicketTypeId = x.t.TicketTypeId,
                TicketTypeQuantity = x.t.TicketTypeQuantity,
                TicketTypeViaLinkUnitSold = x.t.TicketTypeViaLinkUnitSold,
                TicketTypeNormalUnitSold = x.t.TicketTypeNormalUnitSold
            }).ToListAsync();

            return result;
        }

        public async Task<bool> RemoveSaleStageDetails(List<SaleStageDetail> saleStageDetails)
        {
            foreach(var saleStageDetail in saleStageDetails)
            {
                var detail = await Get(saleStageDetail.Id);
                if(detail != null)
                {
                    context.SaleStageDetails.Remove(detail);
                    await Update();
                }           
            }
            return true;
        }

        public async Task<bool> UpdateSaleStageDetail(SaleStageDetailUpdateRequestModel saleStageDetail)
        {
            var saleStageDetailEnt = await Get(saleStageDetail.SaleStageDetailId);
            if (saleStageDetailEnt != null)
            {
                saleStageDetailEnt.TicketTypeId = saleStageDetail.TicketTypeId;
                saleStageDetailEnt.TicketTypeQuantity = saleStageDetail.TicketTypeQuantity;
                await Update();
                return true;
            }
            return false;
        }

        public async Task<int> UpdateUnitSold(Guid saleStageDetailId, int unitSold, bool isBuyViaLink)
        {
            var saleStageDetailEntity = await Get(saleStageDetailId);
            if (saleStageDetailEntity != null)
            {
                saleStageDetailEntity.TicketTypeQuantitySold += unitSold;

                switch (isBuyViaLink)
                {
                    case true:
                        {
                            if (saleStageDetailEntity.TicketTypeViaLinkUnitSold == null)
                            {
                                saleStageDetailEntity.TicketTypeViaLinkUnitSold = 0;
                            }
                            saleStageDetailEntity.TicketTypeViaLinkUnitSold += unitSold;
                            break;
                        }
                    case false:
                        {
                            if (saleStageDetailEntity.TicketTypeNormalUnitSold == null)
                            {
                                saleStageDetailEntity.TicketTypeNormalUnitSold = 0;
                            }
                            saleStageDetailEntity.TicketTypeNormalUnitSold += unitSold;
                            break;
                        }
                }

                await Update();
                return saleStageDetailEntity.TicketTypeQuantitySold;
            }
            return -1;
        }

        public async Task<int> ReOpenUnitSold(Guid saleStageDetailId, int unitSold, bool isBuyViaLink)
        {
            var saleStageDetailEntity = await Get(saleStageDetailId);
            if (saleStageDetailEntity != null)
            {
                saleStageDetailEntity.TicketTypeQuantitySold -= unitSold;

                switch (isBuyViaLink)
                {
                    case true:
                        {
                            if (saleStageDetailEntity.TicketTypeViaLinkUnitSold == null)
                            {
                                saleStageDetailEntity.TicketTypeViaLinkUnitSold = 0;
                            }
                            saleStageDetailEntity.TicketTypeViaLinkUnitSold -= unitSold;
                            break;
                        }
                    case false:
                        {
                            if (saleStageDetailEntity.TicketTypeNormalUnitSold == null)
                            {
                                saleStageDetailEntity.TicketTypeNormalUnitSold = 0;
                            }
                            saleStageDetailEntity.TicketTypeNormalUnitSold -= unitSold;
                            break;
                        }
                }

                await Update();
                return saleStageDetailEntity.TicketTypeQuantitySold;
            }
            return -1;
        }
    }
}
