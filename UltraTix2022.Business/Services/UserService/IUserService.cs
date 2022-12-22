using UltraTix2022.API.UltraTix2022.Data.Models.RequestModel.Entities.AppUser;
using UltraTix2022.API.UltraTix2022.Data.Models.ViewModels.Entities.User;

namespace UltraTix2022.API.UltraTix2022.Business.Services.UserService
{
    public interface IUserService
    {
        //public Task<List<T>> GetAlls();
        public Task<UserTokenModel> LoginWithEmail(UserLoginModel model);
        //public Task<bool> Insert(string token, UserAccountInsertModel account);
        public Task<UserTokenModel> LoginWithUsername(UserLoginModel model);
        public Task<UserProfileModel> GetUserProfileByToken(string token);
        public Task<List<ArtistViewModel>> GetArtists();
        public Task<bool> UpdateUserProfile(string token, UserUpdateRequestModel profile);
        public Task<bool> RemoveAccount(string token, Guid userId);
        public Task<bool> SignUp(RegisterAccountModel accout);
        public Task<string> GetEmailById(Guid id);
        public Task<bool> IsEmailUsed(String email);
        public Task<string> GetNameById(Guid id);
    }
}
