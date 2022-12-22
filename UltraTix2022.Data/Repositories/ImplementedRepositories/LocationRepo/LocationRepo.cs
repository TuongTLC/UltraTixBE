using Microsoft.EntityFrameworkCore;
using UltraTix2022.API.UltraTix2022.Data.Models.Entities;
using UltraTix2022.API.UltraTix2022.Data.Models.RequestModel.Entities.Location;
using UltraTix2022.API.UltraTix2022.Data.Models.ViewModels.Entities.Location;
using UltraTix2022.Data.Repositories.GenericRepository;

namespace UltraTix2022.API.UltraTix2022.Data.Repositories.ImplementedRepositories.LocationRepo
{
    public class LocationRepo : Repository<Location>, ILocationRepo
    {
        public LocationRepo(UltraTixDBContext context) : base(context)
        {

        }

        public async Task<LocationViewModel?> GetLocationByShowID(Guid ShowID)
        {
            return await (from r in context.Locations
                          where r.ShowId.Equals(ShowID)
                          select new LocationViewModel()
                          {
                              Id = r.Id,
                              LocationDescription = r.LocationDescription,
                              ShowId = r.ShowId
                          }).FirstOrDefaultAsync();
        }

        public async Task<bool> UpdateLocation(Location location)
        {
            var locationEntity = await Get(location.Id);
            if (locationEntity != null)
            {
                locationEntity.LocationDescription = location.LocationDescription;
                await Update();
                return true;
            }
            return false;
        }
    }

}
