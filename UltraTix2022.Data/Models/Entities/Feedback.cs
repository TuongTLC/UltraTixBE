using System;
using System.Collections.Generic;

namespace UltraTix2022.API.UltraTix2022.Data.Models.Entities
{
    public partial class Feedback
    {
        public Guid Id { get; set; }
        public string? ProblemBrief { get; set; }
        public string? ProblemDetail { get; set; }
        public string? Status { get; set; }
        public Guid? FeedbackTypeId { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }

        public virtual FeedbackType? FeedbackType { get; set; }
    }
}
