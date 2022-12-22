using Microsoft.EntityFrameworkCore;
using UltraTix2022.API.UltraTix2022.Data.Models.Entities;
using UltraTix2022.API.UltraTix2022.Data.Models.RequestModel.Entities.AppUser;
using UltraTix2022.API.UltraTix2022.Data.Models.ViewModels.Entities.User;
using UltraTix2022.Data.Repositories.GenericRepository;

namespace UltraTix2022.API.UltraTix2022.Data.Repositories.ImplementedRepositories.OrganizerRepo
{
    public class OrganizerRepo : Repository<Organizer>, IOrganizerRepo
    {
        public OrganizerRepo(UltraTixDBContext context) : base(context)
        {

        }

        public async Task<List<OrganizerViewModel>> GetOrganizers()
        {
            var query = from u in context.AppUsers
                        join o in context.Organizers
                        on u.Id equals o.Id
                        where u.RoleId.Equals(1)
                        select new { u, o };

            List<OrganizerViewModel> result = await query.Select(x => new OrganizerViewModel()
            {
                ID = x.o.Id,
                Name = x.u.Username,
                AvatarImgUrl = x.o.AvatarImgUrl ?? "",
                Email = x.u.Email,
                Address = x.o.Address,
                PhoneNumber = x.o.PhoneNumber,
                IsActive = x.u.IsActive,
                RoleID = x.u.RoleId,
                FullName = x.u.FullName
            }).ToListAsync();

            return result;
        }
        public async Task<List<OrganizerInfoModel>> GetOrganizersInfo()
        {
            var query = from u in context.AppUsers
                        join o in context.Organizers
                        on u.Id equals o.Id
                        where u.RoleId.Equals(1)
                        select new { u, o };

            List<OrganizerInfoModel> result = await query.Select(x => new OrganizerInfoModel()
            {
                ID = x.o.Id,
                Email = x.u.Email,
                PhoneNumber = x.o.PhoneNumber,
                ImageURL = x.u.AvatarImageUrl,
                FullName = x.u.FullName
            }).ToListAsync();

            return result;
        }

        public async Task<OrganizerViewModel> GetOrganizerbyStaffID(Guid StaffID)
        {
            var query = from s in context.ShowStaffs
                        join o in context.Organizers
                        on s.OrganizerId equals o.Id
                        join a in context.AppUsers
                        on o.Id equals a.Id
                        where s.Id.Equals(StaffID)
                        select new { s, o, a };

            var result = await query.Select(x => new OrganizerViewModel()
            {
                ID = x.o.Id,
                Name = x.a.Username,
                AvatarImgUrl = x.o.AvatarImgUrl ?? "",
                Email = x.a.Email,
                Address = x.o.Address,
                PhoneNumber = x.o.PhoneNumber,
                IsActive = x.a.IsActive,
                RoleID = 1,
            }).FirstOrDefaultAsync();

            return result ?? throw new Exception("No Organizer Found With Staff RequestID: " + StaffID);
        }

        public async Task<UserProfileModel> GetProfileByID(Guid id)
        {
            var query = from u in context.AppUsers
                        join s in context.Organizers
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
