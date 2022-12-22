using UltraTix2022.API.UltraTix2022.Data.Models.Entities;
using UltraTix2022.Data.Repositories.GenericRepository;

namespace UltraTix2022.API.UltraTix2022.Data.Repositories.ImplementedRepositories.ArtistFollowerRepo
{
    public interface IArtistFollowerRepo : IRepository<ArtistFollower>
    {
        public Task<bool> IsFollowedArtist(Guid userId, Guid artistId);
        public Task<bool> UnFollowArtist(Guid userId, Guid artistId);
        public Task<List<Guid>> GetArtistsFollowed(Guid userId);
    }
}
