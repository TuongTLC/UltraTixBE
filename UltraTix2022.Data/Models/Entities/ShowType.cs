using System;
using System.Collections.Generic;

namespace UltraTix2022.API.UltraTix2022.Data.Models.Entities
{
    public partial class ShowType
    {
        public ShowType()
        {
            ShowCategories = new HashSet<ShowCategory>();
            Shows = new HashSet<Show>();
        }

        public Guid Id { get; set; }
        public string ShowTypeName { get; set; } = null!;
        public string ShowTypeDescription { get; set; } = null!;

        public virtual ICollection<ShowCategory> ShowCategories { get; set; }
        public virtual ICollection<Show> Shows { get; set; }
    }
}
