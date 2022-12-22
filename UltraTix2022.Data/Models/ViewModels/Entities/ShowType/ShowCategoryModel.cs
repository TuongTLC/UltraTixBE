namespace UltraTix2022.API.UltraTix2022.Data.Models.ViewModels.Entities.ShowType
{
    public class ShowCategoryModel
    {
        public Guid Id { get; set; }
        public string CategoryName { get; set; } = string.Empty;
        public Guid ShowtypeID { get; set; }
    }
}

