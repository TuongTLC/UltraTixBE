namespace UltraTix2022.API.UltraTix2022.Data.Models.ViewModels.Entities.User
{
    public class ShowStaffViewModel : AppUserViewModel
    {
        public string AvatarImgUrl { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public Guid OrganizerID { get; set; }
        public string OrganizerName { get; set; } = string.Empty;
        //public string FullName { get; set; } = string.Empty;
    }
}
