using System;
using System.Collections.Generic;

namespace UltraTix2022.API.UltraTix2022.Data.Models.Entities
{
    public partial class ShowCategory
    {
        public ShowCategory()
        {
            Shows = new HashSet<Show>();
        }

        public Guid Id { get; set; }
        public string? Name { get; set; }
        public Guid ShowTypeId { get; set; }

        public virtual ShowType ShowType { get; set; } = null!;
        public virtual ICollection<Show> Shows { get; set; }
    }
}
