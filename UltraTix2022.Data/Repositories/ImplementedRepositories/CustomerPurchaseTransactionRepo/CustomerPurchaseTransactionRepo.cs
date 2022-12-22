using UltraTix2022.API.UltraTix2022.Data.Models.Entities;
using UltraTix2022.Data.Repositories.GenericRepository;

namespace UltraTix2022.API.UltraTix2022.Data.Repositories.ImplementedRepositories.CustomerPurchaseTransactionRepo
{
    public class CustomerPurchaseTransactionRepo : Repository<CustomerPurchaseTransaction>, ICustomerPurchaseTransactionRepo
    {
        public CustomerPurchaseTransactionRepo(UltraTixDBContext context) : base(context)
        {
        }
    }
}
