namespace UltraTix2022.API.UltraTix2022.Data.Models.ViewModels.Entities.Location
{
    public class LocationViewModel
    {
        public Guid Id { get; set; }
        public string LocationDescription { get; set; } = null!;
        public Guid ShowId { get; set; }
    }
}
