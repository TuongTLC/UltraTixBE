using UltraTix2022.API.UltraTix2022.Data.Models.Entities;
using UltraTix2022.Data.Repositories.GenericRepository;

namespace UltraTix2022.API.UltraTix2022.Data.Repositories.ImplementedRepositories.CustomerWalletRepo
{
    public interface ICustomerWalletRepo : IRepository<CustomerWallet>
    {
        public Task<Guid> GetCustomerWalletIdByCustomerId(Guid customerID);
    }
}
