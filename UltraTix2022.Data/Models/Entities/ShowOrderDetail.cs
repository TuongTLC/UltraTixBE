using System;
using System.Collections.Generic;

namespace UltraTix2022.API.UltraTix2022.Data.Models.Entities
{
    public partial class ShowOrderDetail
    {
        public ShowOrderDetail()
        {
            ShowTickets = new HashSet<ShowTicket>();
        }

        public Guid Id { get; set; }
        public int QuantityBuy { get; set; }
        public double SubTotal { get; set; }
        public Guid ShowOrderId { get; set; }
        public string? Description { get; set; }
        public Guid? CampaignDetailId { get; set; }
        public Guid? SaleStageDetailId { get; set; }

        public virtual CampaignDetail? CampaignDetail { get; set; }
        public virtual SaleStageDetail? SaleStageDetail { get; set; }
        public virtual ShowOrder ShowOrder { get; set; } = null!;
        public virtual ICollection<ShowTicket> ShowTickets { get; set; }
    }
}
