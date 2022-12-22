using System;
using System.Collections.Generic;

namespace UltraTix2022.API.UltraTix2022.Data.Models.Entities
{
    public partial class ShowRequest
    {
        public Guid Id { get; set; }
        public Guid ShowStaffId { get; set; }
        public Guid ShowId { get; set; }
        public Guid OrganizerId { get; set; }
        public string State { get; set; } = null!;
        public string Message { get; set; } = null!;
        public DateTime RequestDate { get; set; }

        public virtual Organizer Organizer { get; set; } = null!;
        public virtual Show Show { get; set; } = null!;
        public virtual ShowStaff ShowStaff { get; set; } = null!;
    }
}
