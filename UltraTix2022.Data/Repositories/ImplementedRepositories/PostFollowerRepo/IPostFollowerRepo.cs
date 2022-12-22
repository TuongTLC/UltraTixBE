using UltraTix2022.API.UltraTix2022.Data.Models.Entities;
using UltraTix2022.Data.Repositories.GenericRepository;

namespace UltraTix2022.API.UltraTix2022.Data.Repositories.ImplementedRepositories.PostFollowerRepo
{
    public interface IPostFollowerRepo : IRepository<PostFollower>
    {
        public Task<List<Guid>> GetPostsLiked(Guid userId);
        public Task<Guid> UnLikePost(Guid postId, Guid userId);
    }
}
