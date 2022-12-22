namespace UltraTix2022.API.UltraTix2022.Data.Models.ViewModels.Entities.ShowType
{
    public class ShowTypeViewModel
    {
        public Guid Id { get; set; }
        public string ShowTypeName { get; set; } = null!;
        public string ShowTypeDescription { get; set; } = null!;
    }
}
