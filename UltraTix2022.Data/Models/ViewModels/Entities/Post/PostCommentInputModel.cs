namespace UltraTix2022.API.UltraTix2022.Data.Models.ViewModels.Entities.Post
{
    public class PostCommentInputModel
    {
        public Guid PostId { get; set; }
        public Guid CommentorId { get; set; }
        public string Content { get; set; } = null!;
    }

    public class PostCommentInputModelMobile
    {
        public Guid PostId { get; set; }
        public string Content { get; set; } = null!;
    }
}

