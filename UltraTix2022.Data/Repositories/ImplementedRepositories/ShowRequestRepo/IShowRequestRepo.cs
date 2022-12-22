using UltraTix2022.API.UltraTix2022.Data.Models.Entities;
using UltraTix2022.API.UltraTix2022.Data.Models.ViewModels.Entities.ShowRequest;
using UltraTix2022.Data.Repositories.GenericRepository;

namespace UltraTix2022.API.UltraTix2022.Data.Repositories.ImplementedRepositories.ShowRequestRepo
{
    public interface IShowRequestRepo : IRepository<ShowRequest>
    {
        public Task<bool> UpdateShowRequestMessage(Guid showRequestID, string message);
        public Task<bool> UpdateShowRequestState(Guid showRequestID, string state);
        public Task<List<ShowRequestViewModel>> GetShowRequests(Guid userId);
    }

}
