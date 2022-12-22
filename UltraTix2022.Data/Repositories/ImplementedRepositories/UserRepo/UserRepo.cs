using Microsoft.EntityFrameworkCore;
using UltraTix2022.API.UltraTix2022.Data.Models.Entities;
using UltraTix2022.API.UltraTix2022.Data.Models.RequestModel.Entities.AppUser;
using UltraTix2022.API.UltraTix2022.Data.Models.ViewModels.Entities.User;
using UltraTix2022.Data.Repositories.GenericRepository;

namespace UltraTix2022.API.UltraTix2022.Data.Repositories.ImplementedRepositories.UserRepo
{
    public class UserRepo : Repository<AppUser>, IUserRepo
    {
        public UserRepo(UltraTixDBContext context) : base(context)
        {

        }

        public async Task<List<AppUser>> getList()
        {
            var query = from AppUser u in context.AppUsers

                        select u;

            List<AppUser> list = await query.ToListAsync();

            return list;
        }

        public async Task<UserTokenModel?> GetByEmail(string email)
        {
            var query = from u in context.AppUsers
                        where u.Email.ToLower().Trim().Equals(email.ToLower().Trim())
                        select new { u };

            var user = await query.Select(selector => new UserTokenModel()
            {
                Id = selector.u.Id,
                Name = selector.u.Username,
                Email = email,
                IsActive = selector.u.IsActive,
                Role = selector.u.Role.RoleDescription,
                RoleId = selector.u.RoleId,
                ImageURL = selector.u.AvatarImageUrl ?? string.Empty,
            }).FirstOrDefaultAsync();

            return user;
        }

        public async Task<string> GetEmailByUsername(string username)
        {
            var query = from u in context.AppUsers
                        where u.Username.ToLower().Trim().Equals(username.ToLower().Trim())
                        select new { u };

            var userEmail = await query.Select(selector => selector.u.Email).FirstOrDefaultAsync();

            return userEmail ?? string.Empty;
        }

        public async Task<UserTokenModel?> GetByUsernamePassword(string username, string password)
        {
            var query = from u in context.AppUsers
                        where u.Username.ToLower().Trim().Equals(username.ToLower().Trim())
                        && u.Password.Equals(password.Trim())
                        select new { u };

            var user = await query.Select(selector => new UserTokenModel()
            {
                Id = selector.u.Id,
                Name = selector.u.Username,
                Email = selector.u.Email,
                IsActive = selector.u.IsActive,
                Role = selector.u.Role.RoleDescription,
                RoleId = selector.u.RoleId,
                ImageURL = selector.u.AvatarImageUrl ?? string.Empty,
            }).FirstOrDefaultAsync();

            return user;
        }

        public async Task<string> GetNameByID(Guid ID)
        {
            var appUser = await Get(ID);
            return appUser.FullName ?? string.Empty;
        }

        public async Task<bool> UpdateProfile(UserUpdateRequestModel profile)
        {
            var appUser = await Get(profile.Id ?? new Guid());

            if (appUser == null) return false;

            appUser.FullName = profile.FullName;
            appUser.AvatarImageUrl = profile.ImageURL;

            await Update();
            return true;
        }

        public async Task<bool> DeactiveAccount(Guid accountId)
        {
            var user = await Get(accountId);
            user.IsActive = false;
            await Update();
            return true;
        }

        public async Task<bool> UpdateAccountStatus(Guid accountId, string status)
        {
            try
            {
                var user = await Get(accountId);
                if (status.Equals("enable"))
                {
                    user.IsActive = true;
                }
                if (status.Equals("disable"))
                {
                    user.IsActive = false;
                }
                await Update();

                return true;
            }
            catch (Exception e)
            {
                return false;
                throw new ArgumentException(e.ToString());
            }
        }

        public async Task<bool> IsEmailUsed(string email)
        {
            var query = from u in context.AppUsers
                        where u.Email.ToLower().Trim().Equals(email.ToLower().Trim())
                        select new { u };

            var user = await query.Select(selector => new UserTokenModel()
            {
                Id = selector.u.Id,
                Name = selector.u.Username,
                Email = email,
                IsActive = selector.u.IsActive,
                Role = selector.u.Role.RoleDescription,
                RoleId = selector.u.RoleId,
                ImageURL = selector.u.AvatarImageUrl ?? string.Empty,
            }).FirstOrDefaultAsync();

            if (user != null)
                return true;

            return false;
        }

        public async Task<bool> UpdateToArtist(Guid accountId)
        {
            var user = await Get(accountId);
            user.RoleId = 4;
            await Update();
            return true;
        }
    }
}
