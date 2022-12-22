using UltraTix2022.API.UltraTix2022.Data.Models.Entities;
using UltraTix2022.API.UltraTix2022.Data.Models.ViewModels.Entities.User;
using UltraTix2022.Data.Repositories.GenericRepository;

namespace UltraTix2022.API.UltraTix2022.Data.Repositories.ImplementedRepositories.SystemAdminRepo
{
    public interface ISystemAdminRepo : IRepository<SystemAdmin>
    {
        public Task<List<AdminAccountViewModel>> GetAdminAccounts();

        public Task<UserProfileModel> GetProfileByID(Guid id);
    }
}
