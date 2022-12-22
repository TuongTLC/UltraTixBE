using Microsoft.EntityFrameworkCore;
using UltraTix2022.API.UltraTix2022.Data.Models.Entities;
using UltraTix2022.API.UltraTix2022.Data.Models.ViewModels.Entities.Feedback;
using UltraTix2022.Data.Repositories.GenericRepository;

namespace UltraTix2022.API.UltraTix2022.Data.Repositories.ImplementedRepositories.FeedbackRepo
{
    public class FeedbackRepo : Repository<Feedback>, IFeedbackRepo
    {
        public FeedbackRepo(UltraTixDBContext context) : base(context)
        {
        }

        public async Task<bool> checkFeedbackExist(Guid FeedbackID)
        {
            return (await Get(FeedbackID) != null);
        }

        public async Task<List<FeedbackViewModel>> GetFeedbacks()
        {
            var query = from f in context.Feedbacks
                        join ft in context.FeedbackTypes
                        on f.FeedbackTypeId equals ft.Id
                        select new { f, ft };

            List<FeedbackViewModel> list = await query.Select(x => new FeedbackViewModel()
            {
                Id = x.f.Id,
                ReporterEmail = x.f.Email,
                ReporterPhone = x.f.Phone,
                ProblemBrief = x.f.ProblemBrief,
                ProblemDetail = x.f.ProblemDetail,
                FeedbackTypeName = x.ft.TypeName,
                Status = x.f.Status
                

            }).ToListAsync();

            return list;
        }


        public async Task<bool> UpdateFeedbackStatus(Guid FeedbackID, string status)
        {
            var feedback = await Get(FeedbackID);
            feedback.Status = status;
            await Update();
            return true;
        }
    }
}

