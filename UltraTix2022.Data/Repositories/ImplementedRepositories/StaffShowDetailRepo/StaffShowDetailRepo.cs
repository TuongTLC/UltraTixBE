using Microsoft.EntityFrameworkCore;
using UltraTix2022.API.UltraTix2022.Data.Models.Entities;
using UltraTix2022.Data.Repositories.GenericRepository;

namespace UltraTix2022.API.UltraTix2022.Data.Repositories.ImplementedRepositories.StaffShowDetailRepo
{
    public class StaffShowDetailRepo : Repository<StaffShowDetail>, IStaffShowDetailRepo
    {
        public StaffShowDetailRepo(UltraTixDBContext context) : base(context)
        {

        }

        public async Task<List<Guid>> GetListShowIDByStaffID(Guid staffID)
        {
            var query = from s in context.StaffShowDetails
                        where s.ShowStaffId == staffID
                        select new { s.ShowId };

            List<Guid> result = await query.Select(x => x.ShowId).ToListAsync();

            return result;
        }
    }

}
