using Microsoft.EntityFrameworkCore;
using UltraTix2022.API.UltraTix2022.Data.Models.Entities;
using UltraTix2022.API.UltraTix2022.Data.Models.RequestModel.Entities.SaleStage;
using UltraTix2022.API.UltraTix2022.Data.Models.ViewModels.Entities.SaleStage;
using UltraTix2022.Data.Repositories.GenericRepository;

namespace UltraTix2022.API.UltraTix2022.Data.Repositories.ImplementedRepositories.SaleStageRepo
{
    public class SaleStageRepo : Repository<SaleStage>, ISaleStageRepo
    {
        public SaleStageRepo(UltraTixDBContext context) : base(context)
        {

        }

        public async Task<string> GetNameByID(Guid ID)
        {
            var saleStage = await Get(ID);
            return saleStage.SaleStageOrder;
        }

        public async Task<List<SaleStageViewModel>> GetSaleStagesByShowID(Guid ShowID)
        {
            var query = from t in context.SaleStages
                        where t.ShowId.Equals(ShowID)
                        select new { t };

            List<SaleStageViewModel> result = await query.Select(x => new SaleStageViewModel()
            {
                Id = x.t.Id,
                SaleStageOrder = x.t.SaleStageOrder,
                SaleStageDescription = x.t.SaleStageDescription,
                SaleStageDiscount = x.t.SaleStageDiscount,
                SaleStageStartDate = x.t.SaleStageStartDate,
                SaleStageEndDate = x.t.SaleStageEndDate,
                ShowId = x.t.ShowId,
                startDate = x.t.SaleStageStartDate.ToShortDateString() + " " + x.t.SaleStageStartDate.ToShortTimeString(),
                endDate = x.t.SaleStageEndDate.ToShortDateString() + " " + x.t.SaleStageEndDate.ToShortTimeString(),
            }).ToListAsync();

            return result;
        }

        /*
        public async Task<bool> UpdateSaleStage(SaleStageRequestUpdateModel saleStage)
        {
            var saleStageEnt = await Get(saleStage.sale);
            if (saleStageEnt != null)
            {
                saleStageEnt.SaleStageOrder = saleStage.SaleStageOrder;
                saleStageEnt.SaleStageDescription = saleStage.SaleStageDescription;
                saleStageEnt.SaleStageDiscount = saleStage.SaleStageDiscount;
                saleStageEnt.SaleStageStartDate = saleStage.SaleStageStartDate;
                saleStageEnt.SaleStageEndDate = saleStage.SaleStageEndDate;
                await Update();
                return true;
            }
            return false;
        }
        */
        public async Task<bool> RemoveSaleStage(Guid saleStageId)
        {
            var saleStage = await Get(saleStageId);
            if (saleStage != null)
            {
                context.SaleStages.Remove(saleStage);
                await Update();
                return true;
            }
            return false;
        }
    }

}
