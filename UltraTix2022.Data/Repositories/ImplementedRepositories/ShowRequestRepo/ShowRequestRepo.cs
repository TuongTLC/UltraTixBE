using Microsoft.EntityFrameworkCore;
using UltraTix2022.API.UltraTix2022.Data.Models.Commons;
using UltraTix2022.API.UltraTix2022.Data.Models.Entities;
using UltraTix2022.API.UltraTix2022.Data.Models.ViewModels.Entities.ShowRequest;
using UltraTix2022.Data.Repositories.GenericRepository;

namespace UltraTix2022.API.UltraTix2022.Data.Repositories.ImplementedRepositories.ShowRequestRepo
{
    public class ShowRequestRepo : Repository<ShowRequest>, IShowRequestRepo
    {
        public ShowRequestRepo(UltraTixDBContext context) : base(context)
        {
        }

        public async Task<List<ShowRequestViewModel>> GetShowRequests(Guid userId)
        {
            var query = from s in context.ShowRequests
                        where s.OrganizerId.Equals(userId)
                        select new { s };

            List<ShowRequestViewModel> result = await query.Select(x => new ShowRequestViewModel()
            {
                Id = x.s.Id,
                Message = x.s.Message,
                OrganizerId = x.s.OrganizerId,
                ShowId = x.s.ShowId,
                ShowStaffId = x.s.ShowStaffId,
                State = x.s.State,
                RequestDate = x.s.RequestDate
            }).ToListAsync();

            return result;
        }

        public async Task<bool> UpdateShowRequestMessage(Guid showRequestID, string message)
        {
            var showRequestEntity = await Get(showRequestID);
            if (showRequestEntity == null) throw new ArgumentException("Show Not Found");

            showRequestEntity.Message = message;
            showRequestEntity.State = Commons.APPROVE;

            await Update();
            return true;
        }

        public async Task<bool> UpdateShowRequestState(Guid showRequestID, string state)
        {
            var showRequestEntity = await Get(showRequestID);
            if (showRequestEntity == null) throw new ArgumentException("Show Not Found");

            showRequestEntity.State = state;

            await Update();
            return true;
        }
    }
}
