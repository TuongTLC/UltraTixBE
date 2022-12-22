using UltraTix2022.API.UltraTix2022.Data.Models.Entities;
using UltraTix2022.API.UltraTix2022.Data.Models.RequestModel.Entities.AppUser;
using UltraTix2022.API.UltraTix2022.Data.Models.ViewModels.Entities.User;
using UltraTix2022.Data.Repositories.GenericRepository;

namespace UltraTix2022.API.UltraTix2022.Data.Repositories.ImplementedRepositories.ArtistRepo
{
    public interface IArtistRepo : IRepository<Artist>
    {
        public Task<List<ArtistAccountViewModel>> getList();

        public Task<Artist> getArtistByID(Guid ID);

        public Task<UserProfileModel> GetProfileByID(Guid id);

        public Task<List<ArtistViewModel>> GetArtists();

        public Task<bool> UpdateProfile(UserUpdateRequestModel profile);

        public  Task<List<ArtistListViewModel>> getListActive();
    }

}
