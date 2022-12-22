using Microsoft.EntityFrameworkCore;
using UltraTix2022.API.UltraTix2022.Data.Models.Entities;
using UltraTix2022.API.UltraTix2022.Data.Models.RequestModel.Entities.AppUser;
using UltraTix2022.API.UltraTix2022.Data.Models.ViewModels.Entities.User;
using UltraTix2022.Data.Repositories.GenericRepository;

namespace UltraTix2022.API.UltraTix2022.Data.Repositories.ImplementedRepositories.ArtistRepo
{
    public class ArtistRepo : Repository<Artist>, IArtistRepo
    {
        public ArtistRepo(UltraTixDBContext context) : base(context)
        {

        }

        public async Task<Artist> getArtistByID(Guid ID)
        {
            var query = from Artist u in context.Artists
                        where u.Id.Equals(ID)
                        select u;

            var artist = await query.Select(u => new Artist()
            {
                Id = u.Id,
                Address = u.Address,
                AvatarImgUrl = u.AvatarImgUrl,
                PhoneNumber = u.PhoneNumber,
                ArtistWallets = u.ArtistWallets,
            }).FirstOrDefaultAsync();

            return artist ?? throw new ArgumentException("Artist Not Found In DB with RequestID: " + ID); ;
        }

        public async Task<List<ArtistAccountViewModel>> getList()
        {
            var query = from u in context.AppUsers
                        join a in context.Artists
                        on u.Id equals a.Id
                        select new { u, a };

            List<ArtistAccountViewModel> result = await query.Select(x => new ArtistAccountViewModel()
            {
                ID = x.a.Id,
                Name = x.u.Username,
                AvatarImgURL = x.a.AvatarImgUrl ?? "",
                Email = x.u.Email,
                Address = x.a.Address,
                Phone = x.a.PhoneNumber,
                IsActive = x.u.IsActive,
                RoleID = x.u.RoleId,
                FullName = x.u.FullName
            }).ToListAsync();

            return result;
        }

        public async Task<List<ArtistListViewModel>> getListActive()
        {
            var query = from u in context.AppUsers
                        join a in context.Artists
                        on u.Id equals a.Id

                        where u.IsActive.Equals(true)

                        select new { u, a };

            List<ArtistListViewModel> result = await query.Select(x => new ArtistListViewModel()
            {
                id = x.a.Id,
                fullName = x.u.FullName,
                avatarImgURL = x.a.AvatarImgUrl

            }).ToListAsync();

            return result;
        }

        public async Task<UserProfileModel> GetProfileByID(Guid id)
        {
            var query = from u in context.AppUsers
                        join s in context.Artists
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

        public async Task<List<ArtistViewModel>> GetArtists()
        {
            var query = from u in context.AppUsers
                        join s in context.Artists
                        on u.Id equals s.Id
                        select new { u, s };

            List<ArtistViewModel> artists = await query.Select(selector => new ArtistViewModel()
            {
                Id = selector.u.Id,
                Name = selector.u.FullName ?? "Lệ Thu",
            }).ToListAsync();

            return artists ?? throw new ArgumentException("Artist Profile Not Found In DB"); ;
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
