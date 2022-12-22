using UltraTix2022.API.UltraTix2022.Data.Models.ViewModels.Entities.Post;
namespace UltraTix2022.API.UltraTix2022.Business.Services.PostService
{
    public interface IPostService
    {
        public Task<bool> InsertPost(string token, PostInsertModel postModel);
        public Task<List<PostViewModel>> GetPosts(string token);
        public Task<List<PostViewModel>> GetAllPosts();
        public Task<List<PostViewModel>> GetPostsByArtist(Guid arttistID);
        public Task<List<PostViewModel>> GetPostsByArtistToken(string token);
        public Task<bool> UpdatePost(string token, PostUpdateModel postModel);
        public Task<bool> AddPostLike(string token, Guid postID);
        public Task<bool> RemovePostLike(string token, Guid postID);
        public Task<bool> AddComment(string token, PostCommentInputModel postAddComment);
        public Task<bool> UpdatePostStatus(string token, Guid postID, string status);
        public Task<List<PostCommentViewModel>> GetPostComment(Guid postID);
        public Task<List<Guid>> GetPostsLiked(string token);
        public Task<bool> AddCommentMobile(string token, PostCommentInputModelMobile postAddComment);

    }
}
