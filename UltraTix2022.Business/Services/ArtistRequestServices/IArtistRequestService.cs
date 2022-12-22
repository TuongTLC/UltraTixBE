using System;
using UltraTix2022.API.UltraTix2022.Data.Models.ViewModels.Entities.ArtistRequest;

namespace UltraTix2022.API.UltraTix2022.Business.Services.ArtistRequestServices
{
	public interface IArtistRequestService
	{
		public Task<List<AtistRequestViewModel>> GetRequest(string token);

		public Task<bool> UpdateArtistAccount(Guid requestID, Guid userID, string status, string token);
		public Task<bool> CreateArtistRequest(string token, ArtistRequestInsertModel requestInsertModel);

	}
}

