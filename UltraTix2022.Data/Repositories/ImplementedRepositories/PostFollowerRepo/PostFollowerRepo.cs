using Microsoft.EntityFrameworkCore;
using UltraTix2022.API.UltraTix2022.Data.Models.Entities;
using UltraTix2022.Data.Repositories.GenericRepository;

namespace UltraTix2022.API.UltraTix2022.Data.Repositories.ImplementedRepositories.PostFollowerRepo
{
    public class PostFollowerRepo : Repository<PostFollower>, IPostFollowerRepo
    {
        public PostFollowerRepo(UltraTixDBContext context) : base(context)
        {
        }

        public async Task<List<Guid>> GetPostsLiked(Guid userId)
        {
            var query = from p in context.PostFollowers                  
                        where p.CustomerId.Equals(userId)
                        select new { p };
            List<Guid> list = await query.Select(x => x.p.PostId).ToListAsync();
            return list;
        }

        public async Task<Guid> UnLikePost(Guid postId, Guid userId)
        {
            var query = from p in context.PostFollowers
                        where p.CustomerId.Equals(userId)
                        && p.PostId.Equals(postId)
                        select new { p };
            var result = await query.Select(x => x.p).FirstAsync();   
            context.PostFollowers.Remove(result);
            await Update();
            return result.Id;
        }

    }
}
