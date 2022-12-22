namespace UltraTix2022.API.UltraTix2022.Data.Models.ViewModels.Entities.Post
{
    public class PostViewModel
    {
        public Guid Id { get; set; }
        public string PostContent { get; set; } = null!;
        public string PostImageUrl { get; set; } = null!;
        public string TicketBookingPageLink { get; set; } = null!;
        public string ArtistName { get; set; }
        public string ArtistAvatarURL { get; set; }
        public string Status { get; set; }
        public int? LikeCount { get; set; }
        public DateTime? CreateDate { get; set; }
        public List<PostCommentViewModel> PostComments { get; set; }
        public Guid ArtistId { get; set; }
        public bool IsLiked { get; set; } = false;
        public bool IsFollowed { get; set; } = false;
    }
}

