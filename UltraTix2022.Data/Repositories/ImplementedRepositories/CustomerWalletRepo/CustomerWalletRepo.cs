using Microsoft.EntityFrameworkCore;
using UltraTix2022.API.UltraTix2022.Data.Models.Entities;
using UltraTix2022.Data.Repositories.GenericRepository;

namespace UltraTix2022.API.UltraTix2022.Data.Repositories.ImplementedRepositories.CustomerWalletRepo
{
    public class CustomerWalletRepo : Repository<CustomerWallet>, ICustomerWalletRepo
    {
        public CustomerWalletRepo(UltraTixDBContext context) : base(context)
        {
        }

        public async Task<Guid> GetCustomerWalletIdByCustomerId(Guid customerID)
        {
            var query = from t in context.CustomerWallets
                        where t.OwnerId.Equals(customerID)
                        select new { t };

            CustomerWallet result = await query.Select(x => new CustomerWallet()
            {
                Id = x.t.Id,
                OwnerId = x.t.OwnerId,
                WalletType = x.t.WalletType,
                BankAccountNumber = x.t.BankAccountNumber,

            }).FirstAsync();

            return result.Id;
        }
    }
}
