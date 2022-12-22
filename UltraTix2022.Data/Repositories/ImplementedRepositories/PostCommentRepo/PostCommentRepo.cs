using Microsoft.EntityFrameworkCore;
using UltraTix2022.API.UltraTix2022.Data.Models.Entities;
using UltraTix2022.API.UltraTix2022.Data.Models.ViewModels.Entities.Post;
using UltraTix2022.Data.Repositories.GenericRepository;

namespace UltraTix2022.API.UltraTix2022.Data.Repositories.ImplementedRepositories.PostCommentRepo
{
    public class PostCommentRepo : Repository<PostComment>, IPostCommentRepo
    {
        public PostCommentRepo(UltraTixDBContext context) : base(context)
        {
        }

        public async Task<List<PostCommentViewModel>> getPostComment(Guid postID)
        {
            var query = from p in context.PostComments
                        join a in context.AppUsers
                        on p.CommentorId equals a.Id
                        where p.PostId.Equals(postID)

                        select new { p, a };
            List<PostCommentViewModel> list = await query.Select(x => new PostCommentViewModel()
            {
                Id = x.p.Id,
                Content = x.p.Content,
                CommentorName = x.a.FullName,
                CommentTime = x.p.CommentTime,
            }).ToListAsync();
            return list;
        }

    }
}

