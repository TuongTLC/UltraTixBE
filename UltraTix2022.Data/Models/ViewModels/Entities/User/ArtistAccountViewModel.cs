namespace UltraTix2022.API.UltraTix2022.Data.Models.ViewModels.Entities.User
{
    public class ArtistAccountViewModel : AppUserViewModel
    {
        public string AvatarImgURL { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
    }
}
