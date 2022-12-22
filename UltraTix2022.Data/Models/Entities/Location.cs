using System;
using System.Collections.Generic;

namespace UltraTix2022.API.UltraTix2022.Data.Models.Entities
{
    public partial class Location
    {
        public Guid Id { get; set; }
        public string LocationDescription { get; set; } = null!;
        public Guid ShowId { get; set; }

        public virtual Show Show { get; set; } = null!;
    }
}
