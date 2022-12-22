using Microsoft.EntityFrameworkCore;
using UltraTix2022.API.UltraTix2022.Data.Models.Entities;
using UltraTix2022.API.UltraTix2022.Data.Models.ViewModels.Entities.ShowOrder;
using UltraTix2022.Data.Repositories.GenericRepository;

namespace UltraTix2022.API.UltraTix2022.Data.Repositories.ImplementedRepositories.ShowOrderDetailRepo
{
    public class ShowOrderDetailRepo : Repository<ShowOrderDetail>, IShowOrderDetailRepo
    {
        public ShowOrderDetailRepo(UltraTixDBContext context) : base(context)
        {
        }

        public async Task<List<ShowOrderDetailViewRequestModel>> GetShowOrdersDetailByShowOrderId(Guid showOrderId)
        {
            var query = from s in context.ShowOrderDetails
                        where s.ShowOrderId.Equals(showOrderId)
                        select new { s };

            List<ShowOrderDetailViewRequestModel> result = await query.Select(x => new ShowOrderDetailViewRequestModel()
            {
                Id = x.s.Id,
                ShowOrderId = x.s.ShowOrderId,
                CampaignDetailId = x.s.CampaignDetailId,
                QuantityBuy = x.s.QuantityBuy,
                Description = x.s.Description,
                SaleStageDetailId = x.s.SaleStageDetailId,
                SubTotal = x.s.SubTotal
            }).ToListAsync();

            return result;
        }
    }
}
