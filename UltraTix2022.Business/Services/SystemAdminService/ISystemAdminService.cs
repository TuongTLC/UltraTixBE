using UltraTix2022.API.UltraTix2022.Data.Models.Entities;
using UltraTix2022.API.UltraTix2022.Data.Models.ViewModels.Entities.User;

namespace UltraTix2022.API.UltraTix2022.Business.Services.SystemAdminService
{
    public interface ISystemAdminService
    {
        //public Task<string> LoginAccount(UserLoginModel model);
        public Task<bool> InsertAdmin(string token, UserAccountInsertModel account);
        public Task<List<AdminAccountViewModel>> GetAdminAccounts(string token);
        public Task<List<AppUser>> GetAccounts(string token);
        public Task<bool> UpdateAccoutnStatus(string token, string status, Guid accountID);
    }
}
