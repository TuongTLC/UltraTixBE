using System;
using System.Collections.Generic;

namespace UltraTix2022.API.UltraTix2022.Data.Models.Entities
{
    public partial class StaffShowDetail
    {
        public Guid Id { get; set; }
        public Guid ShowStaffId { get; set; }
        public Guid ShowId { get; set; }

        public virtual Show Show { get; set; } = null!;
        public virtual ShowStaff ShowStaff { get; set; } = null!;
    }
}
