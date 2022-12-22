namespace UltraTix2022.API.UltraTix2022.Data.Models.ViewModels.Entities.User
{
    public class OrganizerInfoModel
    {
        public Guid ID { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string ImageURL { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
    }
}

