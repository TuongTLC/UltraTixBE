using UltraTix2022.API.UltraTix2022.Data.Models.Entities;
using UltraTix2022.API.UltraTix2022.Data.Models.ViewModels.Entities.ShowType;
using UltraTix2022.Data.Repositories.GenericRepository;

namespace UltraTix2022.API.UltraTix2022.Data.Repositories.ImplementedRepositories.ShowTypeRepo
{
    public interface IShowTypeRepo : IRepository<ShowType>
    {
        public Task<List<ShowTypeViewModel>> GetShowTypes();

        public Task<string> GetShowTypeByID(Guid showTypeID);
    }
}
