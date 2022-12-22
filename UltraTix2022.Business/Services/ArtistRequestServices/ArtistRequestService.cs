using System;
using Newtonsoft.Json.Linq;
using NuGet.Common;
using UltraTix2022.API.UltraTix2022.Business.Utilities;
using UltraTix2022.API.UltraTix2022.Data.Models.Commons;
using UltraTix2022.API.UltraTix2022.Data.Models.Entities;
using UltraTix2022.API.UltraTix2022.Data.Models.ViewModels.Entities.ArtistRequest;
using UltraTix2022.API.UltraTix2022.Data.Repositories.ImplementedRepositories.ArtistRequestRepo;
using UltraTix2022.API.UltraTix2022.Data.Repositories.ImplementedRepositories.UserRepo;

namespace UltraTix2022.API.UltraTix2022.Business.Services.ArtistRequestServices
{
	public class ArtistRequestService : IArtistRequestService
	{
        private readonly DecodeToken _decodeToken;
        private readonly IUserRepo _userRepo;
        private readonly IArtistRequestRepo _artistRequestRepo;
        public ArtistRequestService(
            IUserRepo userRepo,
            IArtistRequestRepo artistRequestRepo
            )
		{
            _userRepo = userRepo;
            _artistRequestRepo = artistRequestRepo;
            _decodeToken = new DecodeToken();
        }

        public async Task<bool> CreateArtistRequest(string token, ArtistRequestInsertModel requestInsertModel)
        {
            try
            {
                int roleID = _decodeToken.Decode(token, Commons.JWTClaimRoleID);
                if (!roleID.Equals(Commons.CUSTOMER)) throw new ArgumentException(Commons.ERROR_403_FORBIDDEN_MSG);
                Guid userID = _decodeToken.DecodeID(token, Commons.JWTClaimID);
                if (requestInsertModel != null)
                {
                    ArtistRequest artistRequest = new()
                    {
                        UserId = userID,
                        Description = requestInsertModel.Description,
                        Idnumber = requestInsertModel.Idnumber,
                        Idlocation = requestInsertModel.Idlocation,
                        IdissueDate = requestInsertModel.IdissueDate
                    };
                    await _artistRequestRepo.Insert(artistRequest);
                }
                return true;
            }catch(Exception e)
            {
                throw new ArgumentException(e.ToString());
            }
        }

        public async Task<List<AtistRequestViewModel>> GetRequest(string token)
        {
            try
            {
                int roleID = _decodeToken.Decode(token, Commons.JWTClaimRoleID);
                if (!roleID.Equals(Commons.ADMIN)) throw new ArgumentException(Commons.ERROR_403_FORBIDDEN_MSG);

                var requests = await _artistRequestRepo.getAllRequest();

                return requests;
            }catch(Exception e)
            {
                throw new ArgumentException(e.ToString());
            }
        }

        public async Task<bool> UpdateArtistAccount(Guid requestID, Guid userID, string status, string token)
        {
            try
            {
                int roleID = _decodeToken.Decode(token, Commons.JWTClaimRoleID);
                if (!roleID.Equals(Commons.ADMIN)) throw new ArgumentException(Commons.ERROR_403_FORBIDDEN_MSG);

                var updateRole = await _userRepo.UpdateToArtist(userID);
                var updateStatus = await _artistRequestRepo.UpdateToArtist(requestID, status);

                return updateStatus;
 
            }
            catch(Exception e)
            {
                throw new ArgumentException(e.ToString());
            }
        }
    }
}

