namespace UltraTix2022.API.UltraTix2022.Data.Models.ViewModels.Entities.User
{
    public class AppUserViewModel
    {
        public Guid ID { get; set; }
        public string Name { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public int RoleID { get; set; }
    }
}
