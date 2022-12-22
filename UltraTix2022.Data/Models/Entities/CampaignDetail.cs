using System;
using System.Collections.Generic;

namespace UltraTix2022.API.UltraTix2022.Data.Models.Entities
{
    public partial class CampaignDetail
    {
        public CampaignDetail()
        {
            ShowOrderDetails = new HashSet<ShowOrderDetail>();
            ShowTickets = new HashSet<ShowTicket>();
        }

        public Guid Id { get; set; }
        public string CampaignName { get; set; } = null!;
        public string CampaignDescription { get; set; } = null!;
        public double ArtistDiscount { get; set; }
        public DateTime CampaignStartDate { get; set; }
        public DateTime CampaignEndDate { get; set; }
        public string TicketBookingPageLink { get; set; } = null!;
        public int TicketTypeQuantity { get; set; }
        public int TicketTypeSold { get; set; }
        public Guid CampaignId { get; set; }
        public double? CustomerDiscount { get; set; }
        public Guid? SaleStageDetailId { get; set; }

        public virtual Campaign Campaign { get; set; } = null!;
        public virtual SaleStageDetail? SaleStageDetail { get; set; }
        public virtual ICollection<ShowOrderDetail> ShowOrderDetails { get; set; }
        public virtual ICollection<ShowTicket> ShowTickets { get; set; }
    }
}
