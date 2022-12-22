using System;
using System.Collections.Generic;

namespace UltraTix2022.API.UltraTix2022.Data.Models.Entities
{
    public partial class Show
    {
        public Show()
        {
            Campaigns = new HashSet<Campaign>();
            Locations = new HashSet<Location>();
            SaleStages = new HashSet<SaleStage>();
            ShowOrders = new HashSet<ShowOrder>();
            ShowRequests = new HashSet<ShowRequest>();
            ShowReviews = new HashSet<ShowReview>();
            ShowTransactionHisotries = new HashSet<ShowTransactionHisotry>();
            StaffShowDetails = new HashSet<StaffShowDetail>();
            TicketTypes = new HashSet<TicketType>();
        }

        public Guid Id { get; set; }
        public string ShowName { get; set; } = null!;
        public string? ShowDetail { get; set; }
        public DateTime ShowStartDate { get; set; }
        public DateTime ShowEndDate { get; set; }
        public Guid ShowTypeId { get; set; }
        public Guid ShowOrganizerId { get; set; }
        public string ImgUrl { get; set; } = null!;
        public string Status { get; set; } = null!;
        public string DescriptionImageUrl { get; set; } = null!;
        public Guid? CategoryId { get; set; }
        public string? ShowDescription { get; set; }
        public int Step { get; set; }
        public DateTime? CreationDate { get; set; }
        public bool? IsTransferRevenueToOrganizer { get; set; }

        public virtual ShowCategory? Category { get; set; }
        public virtual Organizer ShowOrganizer { get; set; } = null!;
        public virtual ShowType ShowType { get; set; } = null!;
        public virtual ICollection<Campaign> Campaigns { get; set; }
        public virtual ICollection<Location> Locations { get; set; }
        public virtual ICollection<SaleStage> SaleStages { get; set; }
        public virtual ICollection<ShowOrder> ShowOrders { get; set; }
        public virtual ICollection<ShowRequest> ShowRequests { get; set; }
        public virtual ICollection<ShowReview> ShowReviews { get; set; }
        public virtual ICollection<ShowTransactionHisotry> ShowTransactionHisotries { get; set; }
        public virtual ICollection<StaffShowDetail> StaffShowDetails { get; set; }
        public virtual ICollection<TicketType> TicketTypes { get; set; }
    }
}
