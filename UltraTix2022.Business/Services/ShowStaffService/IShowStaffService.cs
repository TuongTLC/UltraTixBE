using UltraTix2022.API.UltraTix2022.Data.Models.RequestModel.Entities.AppUser;
using UltraTix2022.API.UltraTix2022.Data.Models.ViewModels.Entities.User;

namespace UltraTix2022.API.UltraTix2022.Business.Services.ShowStaffService
{
    public interface IShowStaffService
    {
        public Task<bool> InsertShowStaff(string token, ShowStaffAccountInsertModel account);
        public Task<List<ShowStaffViewModel>> GetShowStaffAccounts(string token);
        public Task<ShowStaffViewModel> GetShowStaffDetailById(string token, Guid staffId);
        public Task<bool> UpdateUserProfile(string token, UserUpdateRequestModel profile);
        public Task<bool> RemoveStaffAccount(string token, Guid staffId);
    }
}
