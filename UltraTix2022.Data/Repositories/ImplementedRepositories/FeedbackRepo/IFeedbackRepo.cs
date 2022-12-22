using UltraTix2022.API.UltraTix2022.Data.Models.Entities;
using UltraTix2022.API.UltraTix2022.Data.Models.ViewModels.Entities.Feedback;
using UltraTix2022.Data.Repositories.GenericRepository;

namespace UltraTix2022.API.UltraTix2022.Data.Repositories.ImplementedRepositories.FeedbackRepo
{
    public interface IFeedbackRepo : IRepository<Feedback>
    {
        public Task<bool> checkFeedbackExist(Guid FeedbackID);
        public Task<List<FeedbackViewModel>> GetFeedbacks();
        public Task<bool> UpdateFeedbackStatus(Guid FeedbackID, string status);
    }
}

