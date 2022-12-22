using UltraTix2022.API.UltraTix2022.Data.Models.Entities;
using UltraTix2022.API.UltraTix2022.Data.Models.RequestModel.Entities.AppUser;
using UltraTix2022.API.UltraTix2022.Data.Models.ViewModels.Entities.User;
using UltraTix2022.Data.Repositories.GenericRepository;

namespace UltraTix2022.API.UltraTix2022.Data.Repositories.ImplementedRepositories.ShowStaffRepo
{
    public interface IShowStaffRepo : IRepository<ShowStaff>
    {
        public Task<List<ShowStaffViewModel>> GetShowStaffs();

        public Task<List<ShowStaffViewModel>> GetShowStaffsByOrganizer(Guid organizerID);

        public Task<UserProfileModel> GetProfileByID(Guid id);

        public Task<bool> UpdateProfile(UserUpdateRequestModel profile);
    }

}
