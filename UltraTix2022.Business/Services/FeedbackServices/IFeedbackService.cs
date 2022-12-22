using UltraTix2022.API.UltraTix2022.Data.Models.Entities;
using UltraTix2022.API.UltraTix2022.Data.Models.ViewModels.Entities.Feedback;

namespace UltraTix2022.API.UltraTix2022.Business.Services.FeedbackServices
{
    public interface IFeedbackService
    {
        public Task<List<FeedbackViewModel>> GetFeedbacks();
        public Task<bool> UpdateFeedbackStatus(string token, Guid FeedbackID, String status);
        public Task<bool> InsertFeedback(FeedbackInsertModel feedback);
        public Task<List<FeedbackType>> GetFeedbackType();
    }

}

