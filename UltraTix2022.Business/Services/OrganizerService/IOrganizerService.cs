using UltraTix2022.API.UltraTix2022.Data.Models.ViewModels.Entities.User;

namespace UltraTix2022.API.UltraTix2022.Business.Services.OrganizerService
{
    public interface IOrganizerService
    {
        public Task<bool> InsertOrganizer(string token, OrganizerAccountInsertModel account);
        public Task<List<OrganizerViewModel>> GetOrganizerAccounts(string token);
        public Task<List<OrganizerInfoModel>> GetOrganizerInfo();
    }
}
