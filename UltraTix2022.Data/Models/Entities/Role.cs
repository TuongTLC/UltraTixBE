using System;
using System.Collections.Generic;

namespace UltraTix2022.API.UltraTix2022.Data.Models.Entities
{
    public partial class Role
    {
        public Role()
        {
            AppUsers = new HashSet<AppUser>();
        }

        public int Id { get; set; }
        public string RoleDescription { get; set; } = null!;

        public virtual ICollection<AppUser> AppUsers { get; set; }
    }
}
