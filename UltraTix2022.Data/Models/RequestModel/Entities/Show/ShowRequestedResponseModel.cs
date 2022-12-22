namespace UltraTix2022.API.UltraTix2022.Data.Models.RequestModel.Entities.Show
{
    public class ShowRequestedResponseModel
    {
        public Guid RequestID { get; set; }
        public Guid ShowId { get; set; }
        public string Message { get; set; } = null!;
    }
}
