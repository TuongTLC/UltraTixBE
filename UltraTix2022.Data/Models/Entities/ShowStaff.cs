using System;
using System.Collections.Generic;

namespace UltraTix2022.API.UltraTix2022.Data.Models.Entities
{
    public partial class ShowStaff
    {
        public ShowStaff()
        {
            ShowRequests = new HashSet<ShowRequest>();
            StaffShowDetails = new HashSet<StaffShowDetail>();
        }

        public Guid Id { get; set; }
        public string? AvatarImgUrl { get; set; }
        public string Address { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
        public Guid OrganizerId { get; set; }

        public virtual AppUser IdNavigation { get; set; } = null!;
        public virtual Organizer Organizer { get; set; } = null!;
        public virtual ICollection<ShowRequest> ShowRequests { get; set; }
        public virtual ICollection<StaffShowDetail> StaffShowDetails { get; set; }
    }
}
