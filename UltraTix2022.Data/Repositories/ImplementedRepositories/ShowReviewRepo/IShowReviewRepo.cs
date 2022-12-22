using UltraTix2022.API.UltraTix2022.Data.Models.Entities;
using UltraTix2022.API.UltraTix2022.Data.Models.ViewModels.Entities.ShowReview;
using UltraTix2022.Data.Repositories.GenericRepository;

namespace UltraTix2022.API.UltraTix2022.Data.Repositories.ImplementedRepositories.ShowReviewRepo
{

    public interface IShowReviewRepo : IRepository<ShowReview>
    {
        public Task<List<ShowReviewViewModel>> GetShowReviews();
    }
}
