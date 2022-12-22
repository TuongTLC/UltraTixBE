using System;
using System.Collections.Generic;

namespace UltraTix2022.API.UltraTix2022.Data.Models.Entities
{
    public partial class ArtistWithdrawTransaction
    {
        public Guid Id { get; set; }
        public double Amount { get; set; }
        public DateTime TransactionDate { get; set; }
        public string TransactionDescription { get; set; } = null!;
        public Guid WalletArtistReceiverId { get; set; }
        public string WalletReceiverDetail { get; set; } = null!;
        public Guid ArtistWalletId { get; set; }

        public virtual ArtistWallet ArtistWallet { get; set; } = null!;
        public virtual AppTransaction IdNavigation { get; set; } = null!;
    }
}
