using System;
using System.Collections.Generic;

namespace UltraTix2022.API.UltraTix2022.Data.Models.Entities
{
    public partial class SaleStageDetail
    {
        public SaleStageDetail()
        {
            CampaignDetails = new HashSet<CampaignDetail>();
            ShowOrderDetails = new HashSet<ShowOrderDetail>();
        }

        public Guid Id { get; set; }
        public Guid SaleStageId { get; set; }
        public Guid TicketTypeId { get; set; }
        public int TicketTypeQuantity { get; set; }
        public int TicketTypeQuantitySold { get; set; }
        public int? TicketTypeNormalUnitSold { get; set; }
        public int? TicketTypeViaLinkUnitSold { get; set; }

        public virtual SaleStage SaleStage { get; set; } = null!;
        public virtual TicketType TicketType { get; set; } = null!;
        public virtual ICollection<CampaignDetail> CampaignDetails { get; set; }
        public virtual ICollection<ShowOrderDetail> ShowOrderDetails { get; set; }
    }
}
