using System;
using System.Collections.Generic;

namespace UltraTix2022.API.UltraTix2022.Data.Models.Entities
{
    public partial class Post
    {
        public Post()
        {
            PostComments = new HashSet<PostComment>();
            PostFollowers = new HashSet<PostFollower>();
        }

        public Guid Id { get; set; }
        public string PostContent { get; set; } = null!;
        public string PostImageUrl { get; set; } = null!;
        public string TicketBookingPageLink { get; set; } = null!;
        public Guid ArtistId { get; set; }
        public string? Status { get; set; }
        public int? LikeCount { get; set; }
        public DateTime? CreateDate { get; set; }

        public virtual Artist Artist { get; set; } = null!;
        public virtual ICollection<PostComment> PostComments { get; set; }
        public virtual ICollection<PostFollower> PostFollowers { get; set; }
    }
}
