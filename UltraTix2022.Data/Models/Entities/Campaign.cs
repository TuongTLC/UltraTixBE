using System;
using System.Collections.Generic;

namespace UltraTix2022.API.UltraTix2022.Data.Models.Entities
{
    public partial class Campaign
    {
        public Campaign()
        {
            CampaignDetails = new HashSet<CampaignDetail>();
            ShowOrders = new HashSet<ShowOrder>();
            ShowTransactionHisotries = new HashSet<ShowTransactionHisotry>();
        }

        public Guid Id { get; set; }
        public double MinDiscount { get; set; }
        public double MaxDiscount { get; set; }
        public Guid ArtistId { get; set; }
        public Guid ShowId { get; set; }
        public string? BookingLink { get; set; }

        public virtual Artist Artist { get; set; } = null!;
        public virtual Show Show { get; set; } = null!;
        public virtual ICollection<CampaignDetail> CampaignDetails { get; set; }
        public virtual ICollection<ShowOrder> ShowOrders { get; set; }
        public virtual ICollection<ShowTransactionHisotry> ShowTransactionHisotries { get; set; }
    }
}
