using System;
using UltraTix2022.API.UltraTix2022.Data.Models.Entities;
using UltraTix2022.API.UltraTix2022.Data.Models.RequestModel.Entities.AppUser;
using UltraTix2022.API.UltraTix2022.Data.Models.ViewModels.Entities.ArtistRequest;
using UltraTix2022.Data.Repositories.GenericRepository;

namespace UltraTix2022.API.UltraTix2022.Data.Repositories.ImplementedRepositories.ArtistRequestRepo
{
	public interface IArtistRequestRepo : IRepository<ArtistRequest>
	{
        public Task<bool> UpdateToArtist(Guid requestId, string satus);
        public Task<List<AtistRequestViewModel>> getAllRequest();
    }
}

