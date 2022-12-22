using System;
using System.Collections.Generic;

namespace UltraTix2022.API.UltraTix2022.Data.Models.Entities
{
    public partial class FeedbackType
    {
        public FeedbackType()
        {
            Feedbacks = new HashSet<Feedback>();
        }

        public Guid Id { get; set; }
        public string? TypeName { get; set; }
        public string? TypeDiscription { get; set; }

        public virtual ICollection<Feedback> Feedbacks { get; set; }
    }
}
