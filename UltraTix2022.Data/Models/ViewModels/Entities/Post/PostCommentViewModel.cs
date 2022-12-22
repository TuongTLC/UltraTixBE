namespace UltraTix2022.API.UltraTix2022.Data.Models.ViewModels.Entities.Post
{
    public class PostCommentViewModel
    {
        public Guid Id { get; set; }
        public string? CommentorName { get; set; }
        public string? Content { get; set; }
        public DateTime? CommentTime { get; set; }
    }
}

