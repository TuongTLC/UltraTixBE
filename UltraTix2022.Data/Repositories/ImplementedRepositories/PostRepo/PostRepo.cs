using Microsoft.EntityFrameworkCore;
using UltraTix2022.API.UltraTix2022.Data.Models.Entities;
using UltraTix2022.API.UltraTix2022.Data.Models.ViewModels.Entities.Post;
using UltraTix2022.Data.Repositories.GenericRepository;

namespace UltraTix2022.API.UltraTix2022.Data.Repositories.ImplementedRepositories.PostRepo
{
    public class PostRepo : Repository<Post>, IPostRepo
    {
        public PostRepo(UltraTixDBContext context) : base(context)
        {
        }

        public async Task<bool> checkPostExist(Guid postID)
        {
            return (await Get(postID) != null);
        }

        public async Task<List<PostViewModel>> getAllPost()
        {
            var query = from p in context.Posts
                        join a in context.Artists
                        on p.ArtistId equals a.Id
                        join u in context.AppUsers
                        on a.Id equals u.Id
                        select new { p, a, u };

            List<PostViewModel> list = await query.Select(x => new PostViewModel()
            {
                Id = x.p.Id,
                PostContent = x.p.PostContent,
                PostImageUrl = x.p.PostImageUrl,
                TicketBookingPageLink = x.p.TicketBookingPageLink,
                LikeCount = (int)x.p.LikeCount,
                ArtistName = x.u.FullName,
                CreateDate = x.p.CreateDate,
                ArtistAvatarURL = x.u.AvatarImageUrl,
                Status = x.p.Status

            }).ToListAsync();

            return list;
        }

        public async Task<List<PostViewModel>> getPublicPost()
        {
            var query = from p in context.Posts
                        join a in context.Artists
                        on p.ArtistId equals a.Id
                        join u in context.AppUsers
                        on a.Id equals u.Id
                        where p.Status.Equals("Public")
                        select new { p, a, u };

            List<PostViewModel> list = await query.Select(x => new PostViewModel()
            {
                Id = x.p.Id,
                PostContent = x.p.PostContent,
                PostImageUrl = x.p.PostImageUrl,
                TicketBookingPageLink = x.p.TicketBookingPageLink,
                LikeCount = (int)x.p.LikeCount,
                ArtistName = x.u.FullName,
                CreateDate = x.p.CreateDate,
                ArtistAvatarURL = x.u.AvatarImageUrl,
                ArtistId = x.p.ArtistId,


            }).ToListAsync();

            return list;
        }

        public async Task<List<PostViewModel>> getPostsByArtist(Guid artistID)
        {
            var query = from p in context.Posts
                        join a in context.Artists
                        on p.ArtistId equals a.Id
                        join u in context.AppUsers
                        on a.Id equals u.Id
                        where p.ArtistId.Equals(artistID)
                        select new { p, a, u };

            List<PostViewModel> list = await query.Select(x => new PostViewModel()
            {
                Id = x.p.Id,
                PostContent = x.p.PostContent,
                PostImageUrl = x.p.PostImageUrl,
                TicketBookingPageLink = x.p.TicketBookingPageLink,
                LikeCount = (int)x.p.LikeCount,
                ArtistName = x.u.FullName,
                CreateDate = x.p.CreateDate,
                ArtistAvatarURL = x.u.AvatarImageUrl,



            }).ToListAsync();

            return list;
        }

        public async Task<bool> updatePost(PostUpdateModel postModel)
        {
            var postEnt = await Get(postModel.Id);
            if (postEnt != null)
            {
                postEnt.PostContent = postModel.PostContent;
                postEnt.PostImageUrl = postModel.PostImageUrl;
                postEnt.TicketBookingPageLink = postModel.TicketBookingPageLink;
                await Update();
                return true;
            }
            return false;
        }

        public async Task<bool> updatePostStatus(Guid PostID, string status)
        {
            var Post = await Get(PostID);
            if (Post == null) throw new ArgumentException("Post Not Found");

            Post.Status = status;
            await Update();
            return true;
        }
        public async Task<bool> AddPostLike(Guid PostID)
        {
            var Post = await Get(PostID);
            if (Post == null) throw new ArgumentException("Post Not Found");

            Post.LikeCount = Post.LikeCount + 1;
            await Update();
            return true;
        }
        public async Task<bool> RemovePostLike(Guid PostID)
        {
            var Post = await Get(PostID);
            if (Post == null) throw new ArgumentException("Post Not Found");

            Post.LikeCount = Post.LikeCount - 1;
            await Update();
            return true;
        }
    }
}
