using System;
using System.Collections.Generic;

namespace UltraTix2022.API.UltraTix2022.Data.Models.Entities
{
    public partial class SaleStage
    {
        public SaleStage()
        {
            SaleStageDetails = new HashSet<SaleStageDetail>();
            ShowTickets = new HashSet<ShowTicket>();
        }

        public Guid Id { get; set; }
        public string SaleStageOrder { get; set; } = null!;
        public string SaleStageDescription { get; set; } = null!;
        public DateTime SaleStageStartDate { get; set; }
        public DateTime SaleStageEndDate { get; set; }
        public double SaleStageDiscount { get; set; }
        public Guid ShowId { get; set; }

        public virtual Show Show { get; set; } = null!;
        public virtual ICollection<SaleStageDetail> SaleStageDetails { get; set; }
        public virtual ICollection<ShowTicket> ShowTickets { get; set; }
    }
}
