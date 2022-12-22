using Microsoft.EntityFrameworkCore;
using UltraTix2022.API.UltraTix2022.Data.Models.Entities;
using UltraTix2022.Data.Repositories.GenericRepository;

namespace UltraTix2022.API.UltraTix2022.Data.Repositories.ImplementedRepositories.ArtistFollowerRepo
{
    public class ArtistFollowerRepo : Repository<ArtistFollower>, IArtistFollowerRepo
    {
        public ArtistFollowerRepo(UltraTixDBContext context) : base(context)
        {
        }

        public async Task<List<Guid>> GetArtistsFollowed(Guid userId)
        {
            var query = from p in context.ArtistFollowers
                        where p.FollowerId.Equals(userId)
                        select new { p };
            var result = await query.Select(x => x.p.ArtistId).ToListAsync();

            if (result != null)
            {
                return result;
            }
            return new();
        }

        public async Task<bool> IsFollowedArtist(Guid userId, Guid artistId)
        {
            var query = from p in context.ArtistFollowers
                        where p.FollowerId.Equals(userId)
                        && p.ArtistId.Equals(artistId)
                        select new { p };
            var result = await query.Select(x => x.p).FirstOrDefaultAsync();

            if (result != null)
            {
                return true;
            }
            return false;
        }

        public async Task<bool> UnFollowArtist(Guid userId, Guid artistId)
        {
            var query = from p in context.ArtistFollowers
                        where p.FollowerId.Equals(userId)
                        && p.ArtistId.Equals(artistId)
                        select new { p };
            var result = await query.Select(x => x.p).FirstAsync();

            if(result != null)
            {
                context.ArtistFollowers.Remove(result);
                await Update();
                return true;
            }
            return false;
        }
    }
}
