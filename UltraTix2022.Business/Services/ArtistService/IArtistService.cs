using UltraTix2022.API.UltraTix2022.Data.Models.ViewModels.Entities.User;

namespace UltraTix2022.API.UltraTix2022.Business.Services.ArtistService
{
    public interface IArtistService
    {
        public Task<bool> InsertArtist(string token, ArtistInsertModel account);
        public Task<List<ArtistAccountViewModel>> GetArtistAccounts(string token);
        public Task<List<ArtistListViewModel>> GetArtistList(string token);
        public Task<bool> FollowArtist(string token, Guid artistId);
        public Task<List<Guid>> GetArtistsFollowed(string token);
    }
}
