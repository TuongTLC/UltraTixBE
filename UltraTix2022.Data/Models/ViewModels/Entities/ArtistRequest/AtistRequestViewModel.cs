using System;
namespace UltraTix2022.API.UltraTix2022.Data.Models.ViewModels.Entities.ArtistRequest
{
	public class AtistRequestViewModel
	{
        public Guid Id { get; set; }
        public Guid? UserID{ get; set; }
        public string? FullName { get; set; }
        public string? Description { get; set; }
        public string? Idnumber { get; set; }
        public string? Idlocation { get; set; }
        public DateTime? IdIssueDate { get; set; }
        public string? Status { get; set; }
    }
}

