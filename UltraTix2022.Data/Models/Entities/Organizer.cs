using System;
using System.Collections.Generic;

namespace UltraTix2022.API.UltraTix2022.Data.Models.Entities
{
    public partial class Organizer
    {
        public Organizer()
        {
            OrganizerWallets = new HashSet<OrganizerWallet>();
            ShowRequests = new HashSet<ShowRequest>();
            ShowStaffs = new HashSet<ShowStaff>();
            Shows = new HashSet<Show>();
        }

        public Guid Id { get; set; }
        public string? AvatarImgUrl { get; set; }
        public string Address { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
        public string? TaxNumber { get; set; }
        public string? TaxLocation { get; set; }

        public virtual AppUser IdNavigation { get; set; } = null!;
        public virtual ICollection<OrganizerWallet> OrganizerWallets { get; set; }
        public virtual ICollection<ShowRequest> ShowRequests { get; set; }
        public virtual ICollection<ShowStaff> ShowStaffs { get; set; }
        public virtual ICollection<Show> Shows { get; set; }
    }
}
