using UltraTix2022.API.UltraTix2022.Data.Models.Entities;
using UltraTix2022.API.UltraTix2022.Data.Models.RequestModel.Entities.AppUser;
using UltraTix2022.API.UltraTix2022.Data.Models.ViewModels.Entities.User;
using UltraTix2022.Data.Repositories.GenericRepository;

namespace UltraTix2022.API.UltraTix2022.Data.Repositories.ImplementedRepositories.UserRepo
{
    public interface IUserRepo : IRepository<AppUser>
    {
        public Task<List<AppUser>> getList();

        public Task<UserTokenModel?> GetByEmail(string email);

        public Task<string> GetEmailByUsername(string username);

        public Task<UserTokenModel?> GetByUsernamePassword(string username, string password);

        public Task<string> GetNameByID(Guid ID);

        public Task<bool> UpdateProfile(UserUpdateRequestModel profile);

        public Task<bool> DeactiveAccount(Guid accountId);
        public Task<bool> UpdateToArtist(Guid accountId);

        public Task<bool> UpdateAccountStatus(Guid accountId, string status);

        public Task<bool> IsEmailUsed(string email);
    }
}
