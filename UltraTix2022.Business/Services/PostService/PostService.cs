using UltraTix2022.API.UltraTix2022.Business.Utilities;
using UltraTix2022.API.UltraTix2022.Data.Models.Commons;
using UltraTix2022.API.UltraTix2022.Data.Models.Entities;
using UltraTix2022.API.UltraTix2022.Data.Models.ViewModels.Entities.Post;
using UltraTix2022.API.UltraTix2022.Data.Repositories.ImplementedRepositories.ArtistFollowerRepo;
using UltraTix2022.API.UltraTix2022.Data.Repositories.ImplementedRepositories.ArtistRepo;
using UltraTix2022.API.UltraTix2022.Data.Repositories.ImplementedRepositories.CampaignRepo;
using UltraTix2022.API.UltraTix2022.Data.Repositories.ImplementedRepositories.PostCommentRepo;
using UltraTix2022.API.UltraTix2022.Data.Repositories.ImplementedRepositories.PostFollowerRepo;
using UltraTix2022.API.UltraTix2022.Data.Repositories.ImplementedRepositories.PostRepo;

namespace UltraTix2022.API.UltraTix2022.Business.Services.PostService
{
    public class PostService : IPostService
    {

        private readonly IPostRepo _postRepo;
        private readonly DecodeToken _decodeToken;
        private readonly ICampaignRepo _campaignRepo;
        private readonly IArtistRepo _artistRepo;
        private readonly IPostCommentRepo _postCommentRepo;
        private readonly IPostFollowerRepo _postFollowerRepo;
        private readonly IArtistFollowerRepo _artistFollowerRepo;

        public PostService(
            IPostRepo postRepo,
            ICampaignRepo campaignRepo,
            IArtistRepo artistRepo,
            IPostCommentRepo postCommentRepo,
            IPostFollowerRepo postFollowerRepo,
            IArtistFollowerRepo artistFollowerRepo
           )
        {
            _postRepo = postRepo;
            _decodeToken = new DecodeToken();
            _campaignRepo = campaignRepo;
            _artistRepo = artistRepo;
            _postCommentRepo = postCommentRepo;
            _postFollowerRepo = postFollowerRepo;
            _artistFollowerRepo = artistFollowerRepo;
        }

        public async Task<bool> AddComment(string token, PostCommentInputModel postAddComment)
        {
            try
            {
                int roleID = _decodeToken.Decode(token, Commons.JWTClaimRoleID);
                if (!roleID.Equals(Commons.ADMIN) && !roleID.Equals(Commons.ORGANIZER) &&
                    !roleID.Equals(Commons.STAFF) && !roleID.Equals(Commons.ARTIST) &&
                    !roleID.Equals(Commons.CUSTOMER)) throw new ArgumentException(Commons.ERROR_403_FORBIDDEN_MSG);

                PostComment pc = new()
                {
                    PostId = postAddComment.PostId,
                    CommentorId = postAddComment.CommentorId,
                    Content = postAddComment.Content,

                };
                var result = await _postCommentRepo.Insert(pc);
                return (true);
            }
            catch (Exception e)
            {
                throw new ArgumentException(e.Message);
            }
        }

        public async Task<bool> AddCommentMobile(string token, PostCommentInputModelMobile postAddComment)
        {
            try
            {
                int roleID = _decodeToken.Decode(token, Commons.JWTClaimRoleID);
                if (!roleID.Equals(Commons.ADMIN) && !roleID.Equals(Commons.ORGANIZER) &&
                    !roleID.Equals(Commons.STAFF) && !roleID.Equals(Commons.ARTIST) &&
                    !roleID.Equals(Commons.CUSTOMER)) throw new ArgumentException(Commons.ERROR_403_FORBIDDEN_MSG);

                Guid userId = _decodeToken.DecodeID(token, Commons.JWTClaimID);

                PostComment pc = new()
                {
                    PostId = postAddComment.PostId,
                    CommentorId = userId,
                    Content = postAddComment.Content,

                };
                var result = await _postCommentRepo.Insert(pc);
                return (true);
            }
            catch (Exception e)
            {
                throw new ArgumentException(e.Message);
            }
        }

        public async Task<bool> AddPostLike(string token, Guid postID)
        {
            try
            {
                int roleID = _decodeToken.Decode(token, Commons.JWTClaimRoleID);
                if (!roleID.Equals(Commons.ADMIN) && !roleID.Equals(Commons.ORGANIZER) &&
                    !roleID.Equals(Commons.STAFF) && !roleID.Equals(Commons.ARTIST) &&
                    !roleID.Equals(Commons.CUSTOMER)) throw new ArgumentException(Commons.ERROR_403_FORBIDDEN_MSG);
                var result = await _postRepo.AddPostLike(postID);

                Guid userId = _decodeToken.DecodeID(token, Commons.JWTClaimID);

                PostFollower postFollowerEntity = new()
                {
                    Id = Guid.NewGuid(),
                    PostId = postID,
                    CustomerId = userId
                };

                var test = await _postFollowerRepo.GetPostsLiked(userId);

                bool isLiked = false;

                foreach(var postId in test)
                {
                    if (postId.Equals(postID))
                    {
                        isLiked = true;
                    }
                }

                if (isLiked)
                {
                    throw new ArgumentException("Post with ID: " + postID + "is already liked by user: " + userId);
                }

                await _postFollowerRepo.Insert(postFollowerEntity);

                return result;
            }
            catch (Exception e)
            {
                throw new ArgumentException(e.Message);
            }
        }

        public async Task<List<PostViewModel>> GetAllPosts()
        {
            try
            {
                var posts = await _postRepo.getAllPost();
                for (int index = 0; index < posts.Count; index++)
                {
                    var postComments = await _postCommentRepo.getPostComment(posts[index].Id);
                    postComments = postComments.OrderByDescending(x => x.CommentTime).ToList();
                    posts[index].PostComments = postComments;
                }
                return posts;

            }
            catch (Exception e)
            {
                throw new ArgumentException(e.Message);
            }
        }

        public async Task<List<PostCommentViewModel>> GetPostComment(Guid postID)
        {
            try
            {
                var posts = await _postCommentRepo.getPostComment(postID);

                return posts;

            }
            catch (Exception e)
            {
                throw new ArgumentException(e.Message);
            }
        }

        public async Task<List<PostViewModel>> GetPosts(string token)
        {
            try
            {
                var posts = await _postRepo.getPublicPost();
                for (int index = 0; index < posts.Count; index++)
                {
                    posts[index].PostComments = await _postCommentRepo.getPostComment(posts[index].Id);
                }

                if (!string.IsNullOrEmpty(token))
                {
                    Guid userId = _decodeToken.DecodeID(token, Commons.JWTClaimID);

                    var postsLiked = await _postFollowerRepo.GetPostsLiked(userId);
                    var artFollowed = await _artistFollowerRepo.GetArtistsFollowed(userId);

                    foreach (var post in posts)
                    {
                        foreach(var postLiked in postsLiked)
                        {
                            if (post.Id.Equals(postLiked))
                            {
                                post.IsLiked = true;
                            }
                        }
                    }

                    foreach (var post in posts)
                    {
                        foreach (var artFol in artFollowed)
                        {
                            if (post.ArtistId.Equals(artFol))
                            {
                                post.IsFollowed = true;
                            }
                        }
                    }
                }           

                return posts;

            }
            catch (Exception e)
            {
                throw new ArgumentException(e.Message);
            }
        }

        public async Task<List<PostViewModel>> GetPostsByArtist(Guid artistID)
        {
            var Artist = await _artistRepo.Get(artistID);
            if (Artist == null)
            {
                throw new ArgumentException("Artist not exist");
            }
            try
            {
                var posts = await _postRepo.getPostsByArtist(artistID);
                for (int index = 0; index < posts.Count; index++)
                {
                    posts[index].PostComments = await _postCommentRepo.getPostComment(posts[index].Id);
                }
                return posts;
            }
            catch (Exception ex)
            {
                throw new ArgumentException(ex.Message);
            }
        }

        public async Task<bool> InsertPost(string token, PostInsertModel postModel)
        {
            int roleID = _decodeToken.Decode(token, Commons.JWTClaimRoleID);
            if (!roleID.Equals(0) && !roleID.Equals(4)) throw new ArgumentException(Commons.ERROR_403_FORBIDDEN_MSG);
            if (postModel == null) throw new ArgumentNullException("Post Null");
            try
            {
                if (postModel != null)
                {
                    Post postEntity = new()
                    {
                        Id = new Guid(),
                        PostContent = postModel.PostContent,
                        PostImageUrl = postModel.PostImageUrl,
                        TicketBookingPageLink = postModel.TicketBookingPageLink,
                        ArtistId = postModel.ArtistID

                    };
                    await _postRepo.Insert(postEntity);
                    return (postEntity != null);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return false;
        }

        public async Task<bool> RemovePostLike(string token, Guid postID)
        {
            try
            {
                int roleID = _decodeToken.Decode(token, Commons.JWTClaimRoleID);
                if (!roleID.Equals(Commons.ADMIN) && !roleID.Equals(Commons.ORGANIZER) &&
                    !roleID.Equals(Commons.STAFF) && !roleID.Equals(Commons.ARTIST) &&
                    !roleID.Equals(Commons.CUSTOMER)) throw new ArgumentException(Commons.ERROR_403_FORBIDDEN_MSG);
                var result = await _postRepo.RemovePostLike(postID);

                Guid userId = _decodeToken.DecodeID(token, Commons.JWTClaimID);

                var id = await _postFollowerRepo.UnLikePost(postID, userId);

                return result;
            }
            catch (Exception e)
            {
                throw new ArgumentException(e.Message);
            }
        }

        public async Task<bool> UpdatePost(string token, PostUpdateModel postModel)
        {

            int roleID = _decodeToken.Decode(token, Commons.JWTClaimRoleID);
            if (!roleID.Equals(0) && !roleID.Equals(4)) throw new ArgumentException(Commons.ERROR_403_FORBIDDEN_MSG);
            if (!await _postRepo.checkPostExist(postModel.Id)) throw new ArgumentException("Post not Exist");
            try
            {
                if (await _postRepo.updatePost(postModel))
                {
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                throw new ArgumentException(ex.Message);
            }
        }

        public async Task<bool> UpdatePostStatus(string token, Guid postID, string status)
        {

            int roleID = _decodeToken.Decode(token, Commons.JWTClaimRoleID);
            if (!roleID.Equals(0)) throw new ArgumentException(Commons.ERROR_403_FORBIDDEN_MSG);

            if (!await _postRepo.checkPostExist(postID)) throw new ArgumentException("Post not Exist");
            try
            {
                if (await _postRepo.updatePostStatus(postID, status))
                {
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                throw new ArgumentException(ex.Message);
            }
        }

        public async Task<List<Guid>> GetPostsLiked(string token)
        {
            List<Guid> postsLiked = new();

            int roleID = _decodeToken.Decode(token, Commons.JWTClaimRoleID);
            if (!roleID.Equals(Commons.CUSTOMER)) throw new ArgumentException(Commons.ERROR_403_FORBIDDEN_MSG);
            Guid userID = _decodeToken.DecodeID(token, Commons.JWTClaimID);

            postsLiked = await _postFollowerRepo.GetPostsLiked(userID);

            if (postsLiked == null) postsLiked = new();

            return postsLiked;
        }

        public async Task<List<PostViewModel>> GetPostsByArtistToken(string token)
        {
            int roleID = _decodeToken.Decode(token, Commons.JWTClaimRoleID);
            if (!roleID.Equals(Commons.ADMIN) && !roleID.Equals(Commons.ARTIST)) throw new ArgumentException(Commons.ERROR_403_FORBIDDEN_MSG);
            Guid userID = _decodeToken.DecodeID(token, Commons.JWTClaimID);
            var Artist = await _artistRepo.Get(userID);
            if (Artist == null)
            {
                throw new ArgumentException("Artist not exist");
            }
            try
            {
                var posts = await _postRepo.getPostsByArtist(userID);
                for (int index = 0; index < posts.Count; index++)
                {
                    posts[index].PostComments = await _postCommentRepo.getPostComment(posts[index].Id);
                }
                return posts;
            }
            catch (Exception ex)
            {
                throw new ArgumentException(ex.Message);
            }
        }
    }
}
