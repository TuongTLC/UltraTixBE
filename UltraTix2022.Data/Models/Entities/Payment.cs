using System;
using System.Collections.Generic;

namespace UltraTix2022.API.UltraTix2022.Data.Models.Entities
{
    public partial class Payment
    {
        public Payment()
        {
            AppTransactions = new HashSet<AppTransaction>();
        }

        public Guid Id { get; set; }
        public string PaymentDescription { get; set; } = null!;
        public bool PaymentStatus { get; set; }

        public virtual ICollection<AppTransaction> AppTransactions { get; set; }
    }
}
