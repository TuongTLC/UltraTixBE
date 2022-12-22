using Microsoft.EntityFrameworkCore;
using UltraTix2022.API.UltraTix2022.Data.Models.Entities;
using UltraTix2022.API.UltraTix2022.Data.Models.ViewModels.Entities.ShowType;
using UltraTix2022.Data.Repositories.GenericRepository;


namespace UltraTix2022.API.UltraTix2022.Data.Repositories.ImplementedRepositories.ShowCategoryRepo
{
    public class ShowCategoryRepo : Repository<ShowCategory>, IShowCategoryRepo
    {
        public ShowCategoryRepo(UltraTixDBContext context) : base(context)
        {
        }

        public async Task<List<ShowCategoryModel>> GetShowCategory(Guid showTypeID)
        {
            var query = from ShowCategory s in context.ShowCategories
                        where s.ShowTypeId.Equals(showTypeID)
                        select s;

            List<ShowCategoryModel> result = await query.Select(x => new ShowCategoryModel()
            {
                Id = x.Id,
                CategoryName = x.Name ?? string.Empty,
                ShowtypeID = x.ShowTypeId
            }).ToListAsync();

            return result;
        }

        public async Task<string> GetCategoryNameByID(Guid categoryID)
        {
            var category = await Get(categoryID);
            if (category == null) throw new ArgumentException("Category not found with ID: " + categoryID);
            if (string.IsNullOrEmpty(category.Name)) throw new ArgumentException("Category Name is Empty Or Null with ID: " + categoryID);
            return category.Name ?? string.Empty;
        }

        public async Task<List<ShowCategoryModel>> GetShowCategories()
        {
            var query = from ShowCategory s in context.ShowCategories
                        select s;

            List<ShowCategoryModel> result = await query.Select(x => new ShowCategoryModel()
            {
                Id = x.Id,
                CategoryName = x.Name ?? string.Empty,
                ShowtypeID = x.ShowTypeId
            }).ToListAsync();

            return result;
        }

    }
}

