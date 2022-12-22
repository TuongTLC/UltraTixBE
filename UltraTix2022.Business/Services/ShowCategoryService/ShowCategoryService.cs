using UltraTix2022.API.UltraTix2022.Data.Models.ViewModels.Entities.ShowType;
using UltraTix2022.API.UltraTix2022.Data.Repositories.ImplementedRepositories.ShowCategoryRepo;

namespace UltraTix2022.API.UltraTix2022.Business.Services.ShowCategoryService
{
    public class ShowCategoryService : IShowCategoryService
    {
        private readonly IShowCategoryRepo _showCategoryRepo;
        public ShowCategoryService(
            IShowCategoryRepo categoryRepo)
        {
            _showCategoryRepo = categoryRepo;

        }

        public async Task<string> GetCategoryNameByID(Guid id)
        {
            var categoryName = await _showCategoryRepo.GetCategoryNameByID(id);
            return categoryName;
        }

        public async Task<List<ShowCategoryModel>> GetShowCategory(Guid showTypeID)
        {
            try
            {
                var result = await _showCategoryRepo.GetShowCategory(showTypeID);
                return result;
            }
            catch (Exception ex)
            {
                throw new ArgumentException(ex.Message);
            }
        }

        public async Task<List<ShowCategoryModel>> GetShowCategories()
        {
            try
            {
                var result = await _showCategoryRepo.GetShowCategories();
                return result;
            }
            catch (Exception ex)
            {
                throw new ArgumentException(ex.Message);
            }
        }

    }
}

