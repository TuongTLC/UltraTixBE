using System;
using System.Collections.Generic;

namespace UltraTix2022.API.UltraTix2022.Data.Models.Entities
{
    public partial class SysAdminWithdrawTransaction
    {
        public Guid Id { get; set; }
        public double Amount { get; set; }
        public DateTime TransactionDate { get; set; }
        public string TransactionDescription { get; set; } = null!;
        public Guid WalletSysAdminReceiverId { get; set; }
        public string WalletReceiverDetail { get; set; } = null!;
        public Guid SystemAdminWalletId { get; set; }

        public virtual AppTransaction IdNavigation { get; set; } = null!;
        public virtual SystemAdminWallet SystemAdminWallet { get; set; } = null!;
    }
}
