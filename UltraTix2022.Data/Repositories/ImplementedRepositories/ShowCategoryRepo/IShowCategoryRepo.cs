using UltraTix2022.API.UltraTix2022.Data.Models.Entities;
using UltraTix2022.API.UltraTix2022.Data.Models.ViewModels.Entities.ShowType;
using UltraTix2022.Data.Repositories.GenericRepository;

namespace UltraTix2022.API.UltraTix2022.Data.Repositories.ImplementedRepositories.ShowCategoryRepo
{
    public interface IShowCategoryRepo : IRepository<ShowCategory>
    {
        public Task<List<ShowCategoryModel>> GetShowCategory(Guid showTypeID);

        public Task<string> GetCategoryNameByID(Guid categoryID);

        public Task<List<ShowCategoryModel>> GetShowCategories();
    }
}

