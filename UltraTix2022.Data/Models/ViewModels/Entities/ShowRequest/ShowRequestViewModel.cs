namespace UltraTix2022.API.UltraTix2022.Data.Models.ViewModels.Entities.ShowRequest
{
    public class ShowRequestViewModel
    {
        public Guid Id { get; set; }
        public Guid ShowStaffId { get; set; }
        public string ShowName { get; set; } = string.Empty;
        public string ShowStaffName { get; set; } = string.Empty;
        public Guid ShowId { get; set; }
        public Guid OrganizerId { get; set; }
        public string State { get; set; } = null!;
        public string Message { get; set; } = null!;
        public DateTime RequestDate { get; set; }
    }
}
