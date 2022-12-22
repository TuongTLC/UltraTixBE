namespace UltraTix2022.API.UltraTix2022.Data.Models.ViewModels.Entities.User
{
    public class OrganizerAccountInsertModel : UserAccountInsertModel
    {
        public string AvatarImgURL { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string TaxNumber { get; set; } = string.Empty;
        public string TaxIssueLocation { get; set; } = string.Empty;
    }
}
