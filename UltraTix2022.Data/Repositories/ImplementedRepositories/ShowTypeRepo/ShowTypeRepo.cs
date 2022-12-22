using Microsoft.EntityFrameworkCore;
using UltraTix2022.API.UltraTix2022.Data.Models.Entities;
using UltraTix2022.API.UltraTix2022.Data.Models.ViewModels.Entities.ShowType;
using UltraTix2022.Data.Repositories.GenericRepository;

namespace UltraTix2022.API.UltraTix2022.Data.Repositories.ImplementedRepositories.ShowTypeRepo
{
    public class ShowTypeRepo : Repository<ShowType>, IShowTypeRepo
    {
        public ShowTypeRepo(UltraTixDBContext context) : base(context)
        {

        }

        public async Task<List<ShowTypeViewModel>> GetShowTypes()
        {
            var query = from u in context.ShowTypes
                        select new { u };

            List<ShowTypeViewModel> result = await query.Select(x => new ShowTypeViewModel()
            {
                Id = x.u.Id,
                ShowTypeName = x.u.ShowTypeName,
                ShowTypeDescription = x.u.ShowTypeDescription
            }).ToListAsync();

            return result;
        }

        public async Task<string> GetShowTypeByID(Guid showTypeID)
        {
            var result = await Get(showTypeID);
            return result.ShowTypeName ?? string.Empty;
        }

    }
}
