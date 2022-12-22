using Microsoft.EntityFrameworkCore;
using UltraTix2022.API.UltraTix2022.Data.Models.Entities;
using UltraTix2022.Data.Repositories.GenericRepository;

namespace UltraTix2022.API.UltraTix2022.Data.Repositories.ImplementedRepositories.FeedbackTypeRepo
{
    public class FeedbackTypeRepo : Repository<FeedbackType>, IFeedbackTypeRepo
    {
        public FeedbackTypeRepo(UltraTixDBContext context) : base(context)
        {
        }

        public async Task<List<FeedbackType>> GetFeedbackTypes()
        {
            var query = from FeedbackType ft in context.FeedbackTypes
                        select ft;
            List<FeedbackType> result = await query.ToListAsync();
            return result;

        }

        public Task<bool> InsertFeedbackTypes()
        {
            throw new NotImplementedException();
        }
    }
}

