using UltraTix2022.API.UltraTix2022.Data.Models.Entities;
using UltraTix2022.Data.Repositories.GenericRepository;

namespace UltraTix2022.API.UltraTix2022.Data.Repositories.ImplementedRepositories.FeedbackTypeRepo
{

    public interface IFeedbackTypeRepo : IRepository<FeedbackType>
    {
        public Task<List<FeedbackType>> GetFeedbackTypes();
        public Task<bool> InsertFeedbackTypes();
    }

}

