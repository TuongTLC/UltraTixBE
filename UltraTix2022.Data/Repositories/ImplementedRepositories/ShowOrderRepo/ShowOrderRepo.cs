using Microsoft.EntityFrameworkCore;
using UltraTix2022.API.UltraTix2022.Data.Models.Entities;
using UltraTix2022.API.UltraTix2022.Data.Models.ViewModels.Entities.ShowOrder;
using UltraTix2022.Data.Repositories.GenericRepository;

namespace UltraTix2022.API.UltraTix2022.Data.Repositories.ImplementedRepositories.ShowOrderRepo
{
    public class ShowOrderRepo : Repository<ShowOrder>, IShowOrderRepo

    {
        public ShowOrderRepo(UltraTixDBContext context) : base(context)
        {
        }

        public async Task<List<ShowOrderRequestViewModel>> GetShowOrders()
        {
            var query = from s in context.ShowOrders
                        select new { s };

            List<ShowOrderRequestViewModel> result = await query.Select(x => new ShowOrderRequestViewModel()
            {
                Id = x.s.Id,
                CustomerId = x.s.CustomerId,
                CampaignId = x.s.CampaignId,
                OrderDate = x.s.OrderDate,
                OrderDescription = x.s.OrderDescription,
                ShowId = x.s.ShowId,
                TotalPay = x.s.TotalPay,
                IsUsed = x.s.IsUsed ?? false
            }).ToListAsync();

            return result;
        }

        public async Task<List<ShowOrderRequestViewModel>> GetShowOrdersByArtistId(Guid ArtistId)
        {
            var query = from s in context.ShowOrders
                        join sh in context.Shows
                        on s.ShowId equals sh.Id
                        join cmp in context.Campaigns
                        on sh.Id equals cmp.ShowId
                        select new { s, sh };

            List<ShowOrderRequestViewModel> result = await query.Select(x => new ShowOrderRequestViewModel()
            {
                Id = x.s.Id,
                CustomerId = x.s.CustomerId,
                CampaignId = x.s.CampaignId,
                OrderDate = x.s.OrderDate,
                OrderDescription = x.s.OrderDescription,
                ShowId = x.s.ShowId,
                IsUsed = x.s.IsUsed ?? false,
                TotalPay = x.s.TotalPay
            }).ToListAsync();

            return result;
        }

        public async Task<List<ShowOrderRequestViewModel>> GetShowOrdersByCustomerId(Guid CustomerId)
        {
            var query = from s in context.ShowOrders
                        where s.CustomerId.Equals(CustomerId)
                        select new { s };

            List<ShowOrderRequestViewModel> result = await query.Select(x => new ShowOrderRequestViewModel()
            {
                Id = x.s.Id,
                CustomerId = x.s.CustomerId,
                CampaignId = x.s.CampaignId,
                OrderDate = x.s.OrderDate,
                OrderDescription = x.s.OrderDescription,
                ShowId = x.s.ShowId,
                TotalPay = x.s.TotalPay,
                IsUsed = x.s.IsUsed ?? false
            }).ToListAsync();

            return result;
        }

        public async Task<List<ShowOrderRequestViewModel>> GetShowOrdersByOrganizerId(Guid OrganizerId)
        {
            var query = from s in context.ShowOrders
                        join sh in context.Shows
                        on s.ShowId equals sh.Id
                        where sh.ShowOrganizerId.Equals(OrganizerId)
                        select new { s, sh };

            List<ShowOrderRequestViewModel> result = await query.Select(x => new ShowOrderRequestViewModel()
            {
                Id = x.s.Id,
                CustomerId = x.s.CustomerId,
                CampaignId = x.s.CampaignId,
                OrderDate = x.s.OrderDate,
                OrderDescription = x.s.OrderDescription,
                ShowId = x.s.ShowId,
                IsUsed = x.s.IsUsed ?? false,
                TotalPay = x.s.TotalPay
            }).ToListAsync();

            return result;
        }

        public async Task<bool> UpdateShowOrderStatusAfterScanned(Guid orderId)
        {
            var showOrder = await Get(orderId);
            if(showOrder != null)
            {
                if(showOrder.IsUsed == true)
                {
                    return false;
                }

                showOrder.IsUsed = true;
                await Update();
                return true;
            }
            return false;
        }
    }
}
