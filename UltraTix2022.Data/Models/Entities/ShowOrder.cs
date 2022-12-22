using System;
using System.Collections.Generic;

namespace UltraTix2022.API.UltraTix2022.Data.Models.Entities
{
    public partial class ShowOrder
    {
        public ShowOrder()
        {
            CustomerPurchaseTransactions = new HashSet<CustomerPurchaseTransaction>();
            ShowOrderDetails = new HashSet<ShowOrderDetail>();
            ShowTransactionHisotries = new HashSet<ShowTransactionHisotry>();
        }

        public Guid Id { get; set; }
        public string OrderDescription { get; set; } = null!;
        public DateTime OrderDate { get; set; }
        public double TotalPay { get; set; }
        public Guid CustomerId { get; set; }
        public Guid ShowId { get; set; }
        public Guid? CampaignId { get; set; }
        public bool? IsUsed { get; set; }

        public virtual Campaign? Campaign { get; set; }
        public virtual Customer Customer { get; set; } = null!;
        public virtual Show Show { get; set; } = null!;
        public virtual ICollection<CustomerPurchaseTransaction> CustomerPurchaseTransactions { get; set; }
        public virtual ICollection<ShowOrderDetail> ShowOrderDetails { get; set; }
        public virtual ICollection<ShowTransactionHisotry> ShowTransactionHisotries { get; set; }
    }
}
