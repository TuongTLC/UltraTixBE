using UltraTix2022.API.UltraTix2022.Data.Models.Entities;
using UltraTix2022.API.UltraTix2022.Data.Models.ViewModels.Entities.Post;
using UltraTix2022.Data.Repositories.GenericRepository;

namespace UltraTix2022.API.UltraTix2022.Data.Repositories.ImplementedRepositories.PostCommentRepo
{
    public interface IPostCommentRepo : IRepository<PostComment>
    {
        public Task<List<PostCommentViewModel>> getPostComment(Guid postId);
    }
}

