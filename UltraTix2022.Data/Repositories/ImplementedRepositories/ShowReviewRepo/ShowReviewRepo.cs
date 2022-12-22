using Microsoft.EntityFrameworkCore;
using UltraTix2022.API.UltraTix2022.Data.Models.Entities;
using UltraTix2022.API.UltraTix2022.Data.Models.ViewModels.Entities.ShowReview;
using UltraTix2022.Data.Repositories.GenericRepository;

namespace UltraTix2022.API.UltraTix2022.Data.Repositories.ImplementedRepositories.ShowReviewRepo
{
    public class ShowReviewRepo : Repository<ShowReview>, IShowReviewRepo
    {
        public ShowReviewRepo(UltraTixDBContext context) : base(context)
        {
        }


        public async Task<List<ShowReviewViewModel>> GetShowReviews()
        {

            var query = from s in context.ShowReviews
                        select new { s };

            List<ShowReviewViewModel> result = await query.Select(x => new ShowReviewViewModel()
            {
                Id = x.s.Id,
                ReviewerId = x.s.ReviewerId,
                ReviewerName = x.s.ReviewerName,
                ReviewMessage = x.s.ReviewMessage,
                ShowId = x.s.ShowId,
                DateTimeReview = x.s.DateTimeReview,
                Rate = x.s.Rate
            }).ToListAsync();

            return result;
        }

    }
}
