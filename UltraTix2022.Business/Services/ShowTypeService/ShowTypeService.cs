using UltraTix2022.API.UltraTix2022.Business.Utilities;
using UltraTix2022.API.UltraTix2022.Data.Models.Commons;
using UltraTix2022.API.UltraTix2022.Data.Models.Entities;
using UltraTix2022.API.UltraTix2022.Data.Models.RequestModel.Entities.ShowType;
using UltraTix2022.API.UltraTix2022.Data.Models.ViewModels.Entities.ShowType;
using UltraTix2022.API.UltraTix2022.Data.Repositories.ImplementedRepositories.ShowTypeRepo;

namespace UltraTix2022.API.UltraTix2022.Business.Services.ShowTypeService
{
    public class ShowTypeService : IShowTypeService
    {
        private readonly DecodeToken _decodeToken;
        private readonly IShowTypeRepo _showTypeRepo;

        public ShowTypeService(
           IShowTypeRepo showTypeRepo)
        {
            _decodeToken = new DecodeToken();
            _showTypeRepo = showTypeRepo;
        }

        //Admin insert organizer account
        public async Task<bool> InsertShowType(string token, ShowTypeRequestInsertModel showType)
        {
            if (showType == null || showType.ShowTypeName == null
                || showType.Description == null)
            {
                throw new ArgumentNullException("ShowType Null");
            }

            if (string.IsNullOrEmpty(showType.ShowTypeName) || string.IsNullOrEmpty(showType.Description))
            {
                throw new ArgumentException("ShowTypeName Null || ShowTypeDescription Null");
            }

            int roleID = _decodeToken.Decode(token, Commons.JWTClaimRoleID);
            if (!roleID.Equals(0)) throw new ArgumentException("Only admin can access this resource");

            try
            {
                //Create new Guid RequestID Show Type
                Guid Id = new();

                ShowType newShow = new()
                {
                    Id = Id,
                    ShowTypeName = showType.ShowTypeName,
                    ShowTypeDescription = showType.Description
                };

                //Add AppUser account to DB - AppUser Table
                Id = await _showTypeRepo.Insert(newShow);

                return true;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<List<ShowTypeViewModel>> GetShowType(string token)
        {
            int roleID = _decodeToken.Decode(token, Commons.JWTClaimRoleID);
            if (roleID.Equals(0) || roleID.Equals(2))
            {
                try
                {
                    List<ShowTypeViewModel> result = await _showTypeRepo.GetShowTypes();

                    if (result.Count > 0) return result;

                }
                catch (Exception ex)
                {
                    throw new ArgumentException("Internal Server Error", ex.Message);
                }
            }
            else
            {
                throw new ArgumentException("Only admin and staff can access this resource");
            }
            return new List<ShowTypeViewModel>();
        }

    }
}
