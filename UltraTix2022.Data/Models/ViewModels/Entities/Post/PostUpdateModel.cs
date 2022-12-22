namespace UltraTix2022.API.UltraTix2022.Data.Models.ViewModels.Entities.Post
{
    public class PostUpdateModel
    {
        public Guid Id { get; set; }
        public string PostContent { get; set; } = null!;
        public string PostImageUrl { get; set; } = null!;
        public string TicketBookingPageLink { get; set; } = null!;
        public Guid ArtistId { get; set; }
        public string? Status { get; set; }
        public int? LikeCount { get; set; }
        public DateTime? CreateDate { get; set; }
    }

}