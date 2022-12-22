using System;
using System.Collections.Generic;

namespace UltraTix2022.API.UltraTix2022.Data.Models.Entities
{
    public partial class ArtistFollower
    {
        public Guid Id { get; set; }
        public Guid ArtistId { get; set; }
        public Guid FollowerId { get; set; }

        public virtual Artist Artist { get; set; } = null!;
        public virtual AppUser Follower { get; set; } = null!;
    }
}
