using System;
using System.Collections.Generic;

namespace UltraTix2022.API.UltraTix2022.Data.Models.Entities
{
    public partial class ArtistRequest
    {
        public Guid Id { get; set; }
        public Guid? UserId { get; set; }
        public string? Description { get; set; }
        public string? Idnumber { get; set; }
        public string? Idlocation { get; set; }
        public DateTime? IdissueDate { get; set; }
        public string? Status { get; set; }

        public virtual AppUser? User { get; set; }
    }
}
