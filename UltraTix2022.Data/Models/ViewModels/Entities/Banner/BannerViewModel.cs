namespace UltraTix2022.API.UltraTix2022.Data.Models.ViewModels.Entities.Banner
{
    public class BannerViewModel
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string BannerImgUrl { get; set; } = string.Empty;
        public string Organizer { get; set; } = string.Empty;
    }
}
