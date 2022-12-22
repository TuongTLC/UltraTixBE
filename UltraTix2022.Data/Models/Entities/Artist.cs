using System;
using System.Collections.Generic;

namespace UltraTix2022.API.UltraTix2022.Data.Models.Entities
{
    public partial class Artist
    {
        public Artist()
        {
            ArtistFollowers = new HashSet<ArtistFollower>();
            ArtistWallets = new HashSet<ArtistWallet>();
            Campaigns = new HashSet<Campaign>();
            Posts = new HashSet<Post>();
        }

        public Guid Id { get; set; }
        public string? AvatarImgUrl { get; set; }
        public string Address { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;

        public virtual AppUser IdNavigation { get; set; } = null!;
        public virtual ICollection<ArtistFollower> ArtistFollowers { get; set; }
        public virtual ICollection<ArtistWallet> ArtistWallets { get; set; }
        public virtual ICollection<Campaign> Campaigns { get; set; }
        public virtual ICollection<Post> Posts { get; set; }
    }
}
