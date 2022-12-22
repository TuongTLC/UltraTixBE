using System;
using System.Collections.Generic;

namespace UltraTix2022.API.UltraTix2022.Data.Models.Entities
{
    public partial class ShowReview
    {
        public Guid Id { get; set; }
        public Guid ShowId { get; set; }
        public Guid ReviewerId { get; set; }
        public string ReviewerName { get; set; } = null!;
        public string ReviewMessage { get; set; } = null!;
        public DateTime DateTimeReview { get; set; }
        public double? Rate { get; set; }

        public virtual Customer Reviewer { get; set; } = null!;
        public virtual Show Show { get; set; } = null!;
    }
}
