using UltraTix2022.API.UltraTix2022.Data.Models.Entities;
using UltraTix2022.API.UltraTix2022.Data.Models.RequestModel.Entities.AppUser;
using UltraTix2022.API.UltraTix2022.Data.Models.ViewModels.Entities.User;
using UltraTix2022.Data.Repositories.GenericRepository;

namespace UltraTix2022.API.UltraTix2022.Data.Repositories.ImplementedRepositories.OrganizerRepo
{
    public interface IOrganizerRepo : IRepository<Organizer>
    {
        public Task<List<OrganizerViewModel>> GetOrganizers();
        public Task<List<OrganizerInfoModel>> GetOrganizersInfo();
        public Task<OrganizerViewModel> GetOrganizerbyStaffID(Guid StaffID);
        public Task<UserProfileModel> GetProfileByID(Guid id);
        public Task<bool> UpdateProfile(UserUpdateRequestModel profile);
    }
}
