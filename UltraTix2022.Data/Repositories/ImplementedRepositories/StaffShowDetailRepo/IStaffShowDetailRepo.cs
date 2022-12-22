using UltraTix2022.API.UltraTix2022.Data.Models.Entities;
using UltraTix2022.Data.Repositories.GenericRepository;

namespace UltraTix2022.API.UltraTix2022.Data.Repositories.ImplementedRepositories.StaffShowDetailRepo
{
    public interface IStaffShowDetailRepo : IRepository<StaffShowDetail>
    {
        public Task<List<Guid>> GetListShowIDByStaffID(Guid staffID);

        //public Task<List<ShowType>> GetShowTypeByStaff(Guid organizerID);
    }

}
