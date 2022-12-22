using System;
using System.Collections.Generic;

namespace UltraTix2022.API.UltraTix2022.Data.Models.Entities
{
    public partial class PostFollower
    {
        public Guid Id { get; set; }
        public Guid PostId { get; set; }
        public Guid CustomerId { get; set; }

        public virtual Customer Customer { get; set; } = null!;
        public virtual Post Post { get; set; } = null!;
    }
}
