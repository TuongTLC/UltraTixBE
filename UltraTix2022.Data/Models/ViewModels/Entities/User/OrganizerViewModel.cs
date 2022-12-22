namespace UltraTix2022.API.UltraTix2022.Data.Models.ViewModels.Entities.User
{
    public class OrganizerViewModel : AppUserViewModel
    {
        public string AvatarImgUrl { get; set; } = null!;
        public string Address { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public double Income { get; set; }
    }
}
