namespace UltraTix2022.API.UltraTix2022.Data.Models.ViewModels.Entities.Post
{
    public class PostInsertModel
    {
        public string PostContent { get; set; } = null!;
        public string PostImageUrl { get; set; } = null!;
        public string TicketBookingPageLink { get; set; } = null!;
        public Guid ArtistID { get; set; }
    }
}
