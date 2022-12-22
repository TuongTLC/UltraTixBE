using UltraTix2022.API.UltraTix2022.Data.Models.Entities;
using UltraTix2022.API.UltraTix2022.Data.Models.RequestModel.Entities.Location;
using UltraTix2022.API.UltraTix2022.Data.Models.ViewModels.Entities.Location;
using UltraTix2022.Data.Repositories.GenericRepository;

namespace UltraTix2022.API.UltraTix2022.Data.Repositories.ImplementedRepositories.LocationRepo
{
    public interface ILocationRepo : IRepository<Location>
    {
        public Task<bool> UpdateLocation(Location location);
        public Task<LocationViewModel?> GetLocationByShowID(Guid ShowID);
    }
}
