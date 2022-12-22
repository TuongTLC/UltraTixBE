using System;
using System.Collections.Generic;

namespace UltraTix2022.API.UltraTix2022.Data.Models.Entities
{
    public partial class CustomerWithdrawTransaction
    {
        public Guid Id { get; set; }
        public double Amount { get; set; }
        public DateTime TransactionDate { get; set; }
        public string TransactionDescription { get; set; } = null!;
        public Guid WalletCustomerReceiverId { get; set; }
        public string WalletReceiverDetail { get; set; } = null!;
        public Guid CustomerWalletId { get; set; }

        public virtual CustomerWallet CustomerWallet { get; set; } = null!;
        public virtual AppTransaction IdNavigation { get; set; } = null!;
    }
}
