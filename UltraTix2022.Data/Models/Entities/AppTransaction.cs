using System;
using System.Collections.Generic;

namespace UltraTix2022.API.UltraTix2022.Data.Models.Entities
{
    public partial class AppTransaction
    {
        public Guid Id { get; set; }
        public Guid? PaymentId { get; set; }
        public double Amount { get; set; }
        public DateTime TransactionDate { get; set; }
        public string TransactionDescription { get; set; } = null!;

        public virtual Payment? Payment { get; set; }
        public virtual ArtistWithdrawTransaction? ArtistWithdrawTransaction { get; set; }
        public virtual CustomerPurchaseTransaction? CustomerPurchaseTransaction { get; set; }
        public virtual CustomerWithdrawTransaction? CustomerWithdrawTransaction { get; set; }
        public virtual OrganizerWithdrawTransaction? OrganizerWithdrawTransaction { get; set; }
        public virtual SysAdminWithdrawTransaction? SysAdminWithdrawTransaction { get; set; }
        public virtual SystemToArtistWalletTransaction? SystemToArtistWalletTransaction { get; set; }
        public virtual SystemToOrganizerTransaction? SystemToOrganizerTransaction { get; set; }
    }
}
