namespace UltraTix2022.API.UltraTix2022.Data.Models.ViewModels.Entities.User
{
    public class UserProfileModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public string Role { get; set; } = string.Empty;
        public int RoleId { get; set; }
        public string Phone { get; set; } = string.Empty;
        public string ImageURL { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
    }
}
