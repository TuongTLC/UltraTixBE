using System;
namespace UltraTix2022.API.UltraTix2022.Data.Models.ViewModels.Entities.ArtistRequest
{
	public class ArtistRequestInsertModel
	{
        public string? Description { get; set; }
        public string? Idnumber { get; set; }
        public string? Idlocation { get; set; }
        public DateTime? IdissueDate { get; set; }
    }
}

