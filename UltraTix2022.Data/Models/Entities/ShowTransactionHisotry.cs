using System;
using System.Collections.Generic;

namespace UltraTix2022.API.UltraTix2022.Data.Models.Entities
{
    public partial class ShowTransactionHisotry
    {
        public Guid Id { get; set; }
        public Guid ShowId { get; set; }
        public string? ShowName { get; set; }
        public Guid ShowOrderId { get; set; }
        public Guid CustomerId { get; set; }
        public Guid? CampaignId { get; set; }
        public int TotalTicketsBuy { get; set; }
        public double Amount { get; set; }
        public bool IsBuyViaArtistLink { get; set; }
        public double? ArtistCommission { get; set; }
        public double? Revenue { get; set; }

        public virtual Campaign? Campaign { get; set; }
        public virtual Customer Customer { get; set; } = null!;
        public virtual Show Show { get; set; } = null!;
        public virtual ShowOrder ShowOrder { get; set; } = null!;
    }
}
