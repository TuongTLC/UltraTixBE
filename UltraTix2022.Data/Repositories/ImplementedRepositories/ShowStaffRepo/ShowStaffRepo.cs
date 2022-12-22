using Microsoft.EntityFrameworkCore;
using UltraTix2022.API.UltraTix2022.Data.Models.Entities;
using UltraTix2022.API.UltraTix2022.Data.Models.RequestModel.Entities.AppUser;
using UltraTix2022.API.UltraTix2022.Data.Models.ViewModels.Entities.User;
using UltraTix2022.Data.Repositories.GenericRepository;

namespace UltraTix2022.API.UltraTix2022.Data.Repositories.ImplementedRepositories.ShowStaffRepo
{
    public class ShowStaffRepo : Repository<ShowStaff>, IShowStaffRepo
    {
        public ShowStaffRepo(UltraTixDBContext context) : base(context)
        {

        }

        public async Task<List<ShowStaffViewModel>> GetShowStaffs()
        {
            var query = from u in context.AppUsers
                        join s in context.ShowStaffs
                        on u.Id equals s.Id
                        select new { u, s };

            List<ShowStaffViewModel> result = await query.Select(x => new ShowStaffViewModel()
            {
                ID = x.s.Id,
                Name = x.u.Username,
                AvatarImgUrl = x.s.AvatarImgUrl ?? "",
                Email = x.u.Email,
                Address = x.s.Address,
                PhoneNumber = x.s.PhoneNumber,
                IsActive = x.u.IsActive,
                RoleID = x.u.RoleId,
                OrganizerID = x.s.OrganizerId,
                FullName = x.u.FullName ?? string.Empty
            }).ToListAsync();

            return result;
        }

        public async Task<List<ShowStaffViewModel>> GetShowStaffsByOrganizer(Guid organizerID)
        {

            var query = from u in context.AppUsers
                        join a in context.ShowStaffs
                        on u.Id equals a.Id
                        where a.OrganizerId.Equals(organizerID)
                        select new { u, a };

            List<ShowStaffViewModel> result = await query.Select(x => new ShowStaffViewModel()
            {
                ID = x.a.Id,
                Name = x.u.Username,
                AvatarImgUrl = !string.IsNullOrEmpty(x.a.AvatarImgUrl) ? x.a.AvatarImgUrl : "",
                Email = x.u.Email,
                Address = x.a.Address,
                PhoneNumber = x.a.PhoneNumber,
                IsActive = x.u.IsActive,
                RoleID = x.u.RoleId,
                OrganizerID = x.a.OrganizerId,
                FullName = x.u.FullName ?? string.Empty
            }).ToListAsync();

            return result ?? new List<ShowStaffViewModel>();
        }

        public async Task<UserProfileModel> GetProfileByID(Guid id)
        {
            var query = from u in context.AppUsers
                        join s in context.ShowStaffs
                        on u.Id equals s.Id
                        where u.Id.Equals(id)
                        select new { u, s };

            var user = await query.Select(selector => new UserProfileModel()
            {
                Id = selector.u.Id,
                Name = selector.u.Username,
                FullName = selector.u.FullName ?? string.Empty,
                Email = selector.u.Email,
                IsActive = selector.u.IsActive,
                Role = selector.u.Role.RoleDescription,
                RoleId = selector.u.RoleId,
                ImageURL = selector.u.AvatarImageUrl ?? string.Empty,
                Phone = selector.s.PhoneNumber ?? string.Empty,
                Address = selector.s.Address ?? string.Empty,
            }).FirstOrDefaultAsync();

            if (user == null) throw new ArgumentException("User Profile Not Found In DB");

            return user;
        }

        public async Task<bool> UpdateProfile(UserUpdateRequestModel profile)
        {
            var appUser = await Get(profile.Id ?? new Guid());

            if (appUser == null) return false;

            appUser.PhoneNumber = profile.Phone;
            appUser.Address = profile.Address;
            appUser.AvatarImgUrl = profile.ImageURL;

            await Update();
            return true;
        }
    }
}
