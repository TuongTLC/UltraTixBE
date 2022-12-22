namespace UltraTix2022.API.UltraTix2022.Data.Models.ViewModels.Entities.Show
{
    public class ShowPagingViewModel
    {
        public List<ShowViewModel> Shows { get; set; } = new();
        public int CurrentPage { get; set; } = 1;
        public int PageCount { get; set; } = 1;
    }
}
