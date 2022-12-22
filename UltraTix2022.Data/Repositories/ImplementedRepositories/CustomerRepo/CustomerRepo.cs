using Microsoft.EntityFrameworkCore;
using UltraTix2022.API.UltraTix2022.Data.Models.Entities;
using UltraTix2022.API.UltraTix2022.Data.Models.RequestModel.Entities.AppUser;
using UltraTix2022.API.UltraTix2022.Data.Models.ViewModels.Entities.User;
using UltraTix2022.Data.Repositories.GenericRepository;

namespace UltraTix2022.API.UltraTix2022.Data.Repositories.ImplementedRepositories.CustomerRepo
{
    public class CustomerRepo : Repository<Customer>, ICustomerRepo
    {
        public CustomerRepo(UltraTixDBContext context) : base(context)
        {

        }

        public async Task<List<Customer>> getList()
        {
            var query = from Customer u in context.Customers

                        select u;

            List<Customer> list = await query.ToListAsync();

            return list;
        }

        public async Task<UserProfileModel> GetProfileByID(Guid id)
        {
            var query = from u in context.AppUsers
                        join s in context.Customers
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

            return user ?? throw new ArgumentException("User Profile Not Found In DB"); ;
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
