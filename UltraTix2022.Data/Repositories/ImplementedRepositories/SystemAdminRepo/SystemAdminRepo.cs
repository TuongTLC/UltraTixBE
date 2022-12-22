using Microsoft.EntityFrameworkCore;
using UltraTix2022.API.UltraTix2022.Data.Models.Entities;
using UltraTix2022.API.UltraTix2022.Data.Models.ViewModels.Entities.User;
using UltraTix2022.Data.Repositories.GenericRepository;

namespace UltraTix2022.API.UltraTix2022.Data.Repositories.ImplementedRepositories.SystemAdminRepo
{
    public class SystemAdminRepo : Repository<SystemAdmin>, ISystemAdminRepo
    {
        public SystemAdminRepo(UltraTixDBContext context) : base(context)
        {

        }

        public async Task<List<AdminAccountViewModel>> GetAdminAccounts()
        {
            var query = from u in context.AppUsers
                        join a in context.SystemAdmins
                        on u.Id equals a.Id
                        where u.RoleId.Equals(0)
                        select new { u, a };

            List<AdminAccountViewModel> result = await query.Select(x => new AdminAccountViewModel()
            {
                ID = x.a.Id,
                Name = x.u.Username,
                Email = x.u.Email,
                IsActive = x.u.IsActive,
                RoleID = x.u.RoleId,
            }).ToListAsync();

            return result;
        }

        public async Task<UserProfileModel> GetProfileByID(Guid id)
        {
            var query = from u in context.AppUsers
                        join s in context.SystemAdmins
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
                Phone = string.Empty,
                Address = string.Empty,
            }).FirstOrDefaultAsync();

            if (user == null) throw new ArgumentException("User Profile Not Found In DB");

            return user;
        }
    }
}
