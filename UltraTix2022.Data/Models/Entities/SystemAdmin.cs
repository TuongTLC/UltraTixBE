using System;
using System.Collections.Generic;

namespace UltraTix2022.API.UltraTix2022.Data.Models.Entities
{
    public partial class SystemAdmin
    {
        public SystemAdmin()
        {
            SystemAdminWallets = new HashSet<SystemAdminWallet>();
        }

        public Guid Id { get; set; }

        public virtual AppUser IdNavigation { get; set; } = null!;
        public virtual ICollection<SystemAdminWallet> SystemAdminWallets { get; set; }
    }
}
