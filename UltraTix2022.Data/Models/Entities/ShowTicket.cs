using System;
using System.Collections.Generic;

namespace UltraTix2022.API.UltraTix2022.Data.Models.Entities
{
    public partial class ShowTicket
    {
        public Guid Id { get; set; }
        public Guid TicketTypeId { get; set; }
        public int QuantityBuy { get; set; }
        public Guid SaleStageId { get; set; }
        public double SubTotal { get; set; }
        public Guid ShowOrderDetailId { get; set; }
        public string? Description { get; set; }
        public Guid? CampaignDetailId { get; set; }

        public virtual CampaignDetail? CampaignDetail { get; set; }
        public virtual SaleStage SaleStage { get; set; } = null!;
        public virtual ShowOrderDetail ShowOrderDetail { get; set; } = null!;
        public virtual TicketType TicketType { get; set; } = null!;
    }
}
