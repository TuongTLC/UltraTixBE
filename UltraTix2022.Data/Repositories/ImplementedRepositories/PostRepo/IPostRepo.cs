using UltraTix2022.API.UltraTix2022.Data.Models.Entities;
using UltraTix2022.API.UltraTix2022.Data.Models.ViewModels.Entities.Post;
using UltraTix2022.Data.Repositories.GenericRepository;

namespace UltraTix2022.API.UltraTix2022.Data.Repositories.ImplementedRepositories.PostRepo
{
    public interface IPostRepo : IRepository<Post>
    {
        public Task<List<PostViewModel>> getPublicPost();
        public Task<List<PostViewModel>> getAllPost();
        public Task<bool> checkPostExist(Guid postID);
        public Task<bool> updatePost(PostUpdateModel postModel);
        public Task<List<PostViewModel>> getPostsByArtist(Guid artistID);
        public Task<bool> updatePostStatus(Guid PostID, string status);
        public Task<bool> RemovePostLike(Guid PostID);
        public Task<bool> AddPostLike(Guid PostID);
    }
}
