using System;
using System.Collections.Generic;

namespace UltraTix2022.API.UltraTix2022.Data.Models.Entities
{
    public partial class CustomerWallet
    {
        public CustomerWallet()
        {
            CustomerWithdrawTransactions = new HashSet<CustomerWithdrawTransaction>();
        }

        public Guid Id { get; set; }
        public string WalletType { get; set; } = null!;
        public string BankAccountNumber { get; set; } = null!;
        public Guid OwnerId { get; set; }

        public virtual Customer Owner { get; set; } = null!;
        public virtual ICollection<CustomerWithdrawTransaction> CustomerWithdrawTransactions { get; set; }
    }
}
