using UltraTix2022.API.UltraTix2022.Data.Models.Entities;
using UltraTix2022.API.UltraTix2022.Data.Models.RequestModel.Entities.Show;
using UltraTix2022.API.UltraTix2022.Data.Models.ViewModels.Entities.Show;
using UltraTix2022.Data.Repositories.GenericRepository;

namespace UltraTix2022.API.UltraTix2022.Data.Repositories.ImplementedRepositories.ShowRepo
{
    public interface IShowRepo : IRepository<Show>
    {
        public Task<List<ShowViewModel>> GetShowsByType(string type);
        public Task<List<ShowViewModel>> GetShowsByLocation(string location);
        public Task<List<ShowViewModel>> GetShowsByStatus(string status);
        public Task<bool> UpdateShowStatus(Guid showID, string status);
        public Task<bool> UpdateShow(Show showUpdateModel);
        public Task<bool> CheckShowIsExist(Guid showID);
        public Task<List<ShowViewModel>> GetAllShows();
        public Task<List<ShowViewModel>> GetShowsJoinedForArtist(Guid artistId);
        public Task<bool> IsShowBelongToOrganizer(Guid organizerId, Guid showId);
        public Task<bool> UpdateShowCreationStep(Guid showId, int step);
        public Task<int> GetShowCreationStep(Guid showId);
        public Task<DateTime> GetShowCreationDate(Guid showId);
    }
}
