using System;
namespace UltraTix2022.API.UltraTix2022.Data.Models.ViewModels.Entities.User
{
	public class ArtistListViewModel
	{
		public Guid id { get; set; }
		public string fullName { get; set; }
        public string avatarImgURL { get; set; }
		public bool IsFollowed { get; set; } = false;
    }
}

