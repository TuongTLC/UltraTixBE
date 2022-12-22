using System;
using System.Collections.Generic;

namespace UltraTix2022.API.UltraTix2022.Data.Models.Entities
{
    public partial class TicketType
    {
        public TicketType()
        {
            SaleStageDetails = new HashSet<SaleStageDetail>();
            ShowTickets = new HashSet<ShowTicket>();
        }

        public Guid Id { get; set; }
        public string TicketTypeName { get; set; } = null!;
        public string TicketTypeDescription { get; set; } = null!;
        public double OriginalPrice { get; set; }
        public int Quantity { get; set; }
        public double TicketTypeDiscount { get; set; }
        public Guid ShowId { get; set; }
        public int UnitSold { get; set; }
        public int? TicketTypeNormalUnitSold { get; set; }
        public int? TicketTypeViaLinkUnitSold { get; set; }

        public virtual Show Show { get; set; } = null!;
        public virtual ICollection<SaleStageDetail> SaleStageDetails { get; set; }
        public virtual ICollection<ShowTicket> ShowTickets { get; set; }
    }
}
