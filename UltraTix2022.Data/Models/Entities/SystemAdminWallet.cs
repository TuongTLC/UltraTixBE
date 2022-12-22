using System;
using System.Collections.Generic;

namespace UltraTix2022.API.UltraTix2022.Data.Models.Entities
{
    public partial class SystemAdminWallet
    {
        public SystemAdminWallet()
        {
            SysAdminWithdrawTransactions = new HashSet<SysAdminWithdrawTransaction>();
        }

        public Guid Id { get; set; }
        public string WalletType { get; set; } = null!;
        public string BankAccountNumber { get; set; } = null!;
        public Guid OwnerId { get; set; }

        public virtual SystemAdmin Owner { get; set; } = null!;
        public virtual ICollection<SysAdminWithdrawTransaction> SysAdminWithdrawTransactions { get; set; }
    }
}
