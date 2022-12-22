using UltraTix2022.API.UltraTix2022.Data.Models.Entities;
using UltraTix2022.API.UltraTix2022.Data.Models.ViewModels.Entities.ShowTransaction;
using UltraTix2022.Data.Repositories.GenericRepository;

namespace UltraTix2022.API.UltraTix2022.Data.Repositories.ImplementedRepositories.ShowTransactionHistoryRepo
{
    public interface IShowTransactionHistoryRepo : IRepository<ShowTransactionHisotry>
    {
        public Task<List<ShowTransactionRequestViewModel>> GetShowTransactionsByOrganizer(Guid organizerId);
        public Task<List<ShowTransactionRequestViewModel>> GetShowTransactionsByArtist(Guid artistId);
        public Task<List<ShowTransactionRequestViewModel>> GetShowTransactionsByAdmin();
    }
}
