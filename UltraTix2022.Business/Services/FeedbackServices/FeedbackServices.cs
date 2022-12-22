using UltraTix2022.API.UltraTix2022.Business.Utilities;
using UltraTix2022.API.UltraTix2022.Data.Models.Commons;
using UltraTix2022.API.UltraTix2022.Data.Models.Entities;
using UltraTix2022.API.UltraTix2022.Data.Models.ViewModels.Entities.Feedback;
using UltraTix2022.API.UltraTix2022.Data.Repositories.ImplementedRepositories.FeedbackRepo;
using UltraTix2022.API.UltraTix2022.Data.Repositories.ImplementedRepositories.FeedbackTypeRepo;

namespace UltraTix2022.API.UltraTix2022.Business.Services.FeedbackServices
{
    public class FeedbackServices : IFeedbackService
    {
        private readonly DecodeToken _decodeToken;
        private readonly IFeedbackRepo _feedbackRepo;
        private readonly IFeedbackTypeRepo _feedbackTypeRepo;
        public FeedbackServices(
            IFeedbackRepo feedbackRepo,
            IFeedbackTypeRepo feedbackTypeRepo)
        {
            _feedbackRepo = feedbackRepo;
            _feedbackTypeRepo = feedbackTypeRepo;
            _decodeToken = new DecodeToken();
        }

        public async Task<List<FeedbackViewModel>> GetFeedbacks()
        {

            try
            {
                var list = await _feedbackRepo.GetFeedbacks();

                return list;

            }
            catch (Exception e)
            {
                throw new ArgumentException(e.ToString());
            }
        }

        public async Task<List<FeedbackType>> GetFeedbackType()
        {
            try
            {
                List<FeedbackType> list = await _feedbackTypeRepo.GetFeedbackTypes();

                return list;

            }
            catch (Exception e)
            {
                throw new ArgumentException(e.ToString());
            }
        }
        public async Task<bool> InsertFeedback(FeedbackInsertModel feedback)
        {

            try
            {
                Feedback newfeedback = new()
                {
                    Email = feedback.Email,
                    Phone = feedback.Phone,
                    ProblemBrief = feedback.ProblemBrief,
                    ProblemDetail = feedback.ProblemDetail,
                    FeedbackTypeId = feedback.FeedbackTypeID,
                    Status = Commons.PENDING,
                };

                var result = await _feedbackRepo.Insert(newfeedback);
                return true;
            }
            catch (Exception e)
            {
                throw new ArgumentException(e.ToString());
            }
        }

        public async Task<bool> UpdateFeedbackStatus(string token, Guid FeedbackID, string status)
        {
            int roleID = _decodeToken.Decode(token, Commons.JWTClaimRoleID);
            if (!roleID.Equals(0)) throw new ArgumentException(Commons.ERROR_403_FORBIDDEN_MSG);
            if (!await _feedbackRepo.checkFeedbackExist(FeedbackID))
            {
                throw new ArgumentException("Feedback not exist");
            }
            try
            {
                if (await _feedbackRepo.UpdateFeedbackStatus(FeedbackID, status))
                {
                    return true;
                }
                return false;
            }
            catch (Exception e)
            {
                throw new ArgumentException(e.ToString());
            }
        }
    }
}

