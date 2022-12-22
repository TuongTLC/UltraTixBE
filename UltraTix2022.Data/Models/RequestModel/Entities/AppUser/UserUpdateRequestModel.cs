namespace UltraTix2022.API.UltraTix2022.Data.Models.RequestModel.Entities.AppUser
{
    public class UserUpdateRequestModel
    {
        public Guid? Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string ImageURL { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
    }
}
