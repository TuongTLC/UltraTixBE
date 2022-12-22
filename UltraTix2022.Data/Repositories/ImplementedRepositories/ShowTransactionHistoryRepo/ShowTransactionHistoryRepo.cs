using Microsoft.EntityFrameworkCore;
using UltraTix2022.API.UltraTix2022.Data.Models.Entities;
using UltraTix2022.API.UltraTix2022.Data.Models.ViewModels.Entities.ShowTransaction;
using UltraTix2022.Data.Repositories.GenericRepository;

namespace UltraTix2022.API.UltraTix2022.Data.Repositories.ImplementedRepositories.ShowTransactionHistoryRepo
{
    public class ShowTransactionHistoryRepo : Repository<ShowTransactionHisotry>, IShowTransactionHistoryRepo
    {
        public ShowTransactionHistoryRepo(UltraTixDBContext context) : base(context)
        {
        }

        public async Task<List<ShowTransactionRequestViewModel>> GetShowTransactionsByArtist(Guid artistId)
        {
            var query = from s in context.ShowTransactionHisotries
                        join sh in context.Shows
                        on s.ShowId equals sh.Id
                        join cmp in context.Campaigns
                        on sh.Id equals cmp.ShowId
                        where cmp.ArtistId.Equals(artistId)
                        where s.CampaignId != null
                        select new { s, sh, cmp };

            List<ShowTransactionRequestViewModel> result = await query.Select(x => new ShowTransactionRequestViewModel()
            {
                Id = x.s.Id,
                CustomerId = x.s.CustomerId,
                CampaignId = x.s.CampaignId,
                ShowId = x.s.ShowId,
                Amount = x.s.Amount,
                ArtistCommission = x.s.ArtistCommission ?? 0,
                IsBuyViaArtistLink = x.s.IsBuyViaArtistLink,
                Revenue = x.s.Revenue ?? 0,
                ShowName = x.sh.ShowName ?? string.Empty,
                ShowOrderId = x.s.ShowOrderId,
                TotalTicketsBuy = x.s.TotalTicketsBuy
            }).ToListAsync();

            return result;
        }

        public async Task<List<ShowTransactionRequestViewModel>> GetShowTransactionsByOrganizer(Guid organizerId)
        {
            var query = from s in context.ShowTransactionHisotries
                        join sh in context.Shows
                        on s.ShowId equals sh.Id
                        join cmp in context.Campaigns
                        on sh.Id equals cmp.ShowId
                        where sh.ShowOrganizerId.Equals(organizerId)
                        select new { s, sh, cmp };

            List<ShowTransactionRequestViewModel> result = await query.Select(x => new ShowTransactionRequestViewModel()
            {
                Id = x.s.Id,
                CustomerId = x.s.CustomerId,
                CampaignId = x.s.CampaignId,
                ShowId = x.s.ShowId,
                Amount = x.s.Amount,
                ArtistCommission = x.s.ArtistCommission ?? 0,
                IsBuyViaArtistLink = x.s.IsBuyViaArtistLink,
                Revenue = x.s.Revenue ?? 0,
                ShowName = x.sh.ShowName ?? string.Empty,
                ShowOrderId = x.s.ShowOrderId,
                TotalTicketsBuy = x.s.TotalTicketsBuy
            }).ToListAsync();

            return result;
        }

        public async Task<List<ShowTransactionRequestViewModel>> GetShowTransactionsByAdmin()
        {
            var query = from s in context.ShowTransactionHisotries
                        join sh in context.Shows
                        on s.ShowId equals sh.Id
                        join cmp in context.Campaigns
                        on sh.Id equals cmp.ShowId
                        select new { s, sh, cmp };

            List<ShowTransactionRequestViewModel> result = await query.Select(x => new ShowTransactionRequestViewModel()
            {
                Id = x.s.Id,
                CustomerId = x.s.CustomerId,
                CampaignId = x.s.CampaignId,
                ShowId = x.s.ShowId,
                Amount = x.s.Amount,
                ArtistCommission = x.s.ArtistCommission ?? 0,
                IsBuyViaArtistLink = x.s.IsBuyViaArtistLink,
                Revenue = x.s.Revenue ?? 0,
                ShowName = x.sh.ShowName ?? string.Empty,
                ShowOrderId = x.s.ShowOrderId,
                TotalTicketsBuy = x.s.TotalTicketsBuy
            }).ToListAsync();

            return result;
        }
    }
}
