using System;
using System.Collections.Generic;

namespace UltraTix2022.API.UltraTix2022.Data.Models.Entities
{
    public partial class PostComment
    {
        public Guid Id { get; set; }
        public Guid? PostId { get; set; }
        public Guid? CommentorId { get; set; }
        public string? Content { get; set; }
        public DateTime? CommentTime { get; set; }

        public virtual AppUser? Commentor { get; set; }
        public virtual Post? Post { get; set; }
    }
}
