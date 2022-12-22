using System;
using System.Collections.Generic;

namespace UltraTix2022.API.UltraTix2022.Data.Models.Entities
{
    public partial class AppUser
    {
        public AppUser()
        {
            ArtistFollowers = new HashSet<ArtistFollower>();
            ArtistRequests = new HashSet<ArtistRequest>();
            PostComments = new HashSet<PostComment>();
        }

        public Guid Id { get; set; }
        public string Username { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public bool IsActive { get; set; }
        public int RoleId { get; set; }
        public string? AvatarImageUrl { get; set; }
        public string? FullName { get; set; }

        public virtual Role Role { get; set; } = null!;
        public virtual Artist? Artist { get; set; }
        public virtual Customer? Customer { get; set; }
        public virtual Organizer? Organizer { get; set; }
        public virtual ShowStaff? ShowStaff { get; set; }
        public virtual SystemAdmin? SystemAdmin { get; set; }
        public virtual ICollection<ArtistFollower> ArtistFollowers { get; set; }
        public virtual ICollection<ArtistRequest> ArtistRequests { get; set; }
        public virtual ICollection<PostComment> PostComments { get; set; }
    }
}
