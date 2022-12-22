using UltraTix2022.API.UltraTix2022.Data.Models.ViewModels.Entities.ShowTransaction;

namespace UltraTix2022.API.UltraTix2022.Business.Services.ShowTransactionService
{
    public interface IShowTransactionService
    {
        public Task<List<ShowTransactionRequestViewModel>> GetShowTransactions(string token);
        public Task<List<ShowTransactionRequestViewModel>> GetShowTransactionsByOrganizerID(string token, Guid organizerID);
        public Task<List<ShowTransactionRequestViewModel>> GetShowTransactionsForAdmin(string token);
        public Task<List<ShowTransactionRequestViewModel>> GetShowTransactionsByOrganizerForAdmin(string token, Guid organizerID);
    }
}
