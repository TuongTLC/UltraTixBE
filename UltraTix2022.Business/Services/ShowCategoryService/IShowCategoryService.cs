using UltraTix2022.API.UltraTix2022.Data.Models.ViewModels.Entities.ShowType;

namespace UltraTix2022.API.UltraTix2022.Business.Services.ShowCategoryService
{
    public interface IShowCategoryService
    {
        public Task<List<ShowCategoryModel>> GetShowCategory(Guid showTypeID);

        public Task<List<ShowCategoryModel>> GetShowCategories();

        public Task<string> GetCategoryNameByID(Guid id);

    }

}

