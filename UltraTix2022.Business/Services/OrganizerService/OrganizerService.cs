using Firebase.Auth;
using UltraTix2022.API.UltraTix2022.Business.Services.ShowOrderService;
using UltraTix2022.API.UltraTix2022.Business.Services.ShowTransactionService;
using UltraTix2022.API.UltraTix2022.Business.Utilities;
using UltraTix2022.API.UltraTix2022.Data.Models.Commons;
using UltraTix2022.API.UltraTix2022.Data.Models.Entities;
using UltraTix2022.API.UltraTix2022.Data.Models.ViewModels.Entities.ShowOrder;
using UltraTix2022.API.UltraTix2022.Data.Models.ViewModels.Entities.ShowTransaction;
using UltraTix2022.API.UltraTix2022.Data.Models.ViewModels.Entities.User;
using UltraTix2022.API.UltraTix2022.Data.Repositories.ImplementedRepositories.OrganizerRepo;
using UltraTix2022.API.UltraTix2022.Data.Repositories.ImplementedRepositories.ShowTransactionHistoryRepo;
using UltraTix2022.API.UltraTix2022.Data.Repositories.ImplementedRepositories.UserRepo;

namespace UltraTix2022.API.UltraTix2022.Business.Services.OrganizerService
{
    public class OrganizerService : IOrganizerService
    {
        private readonly IUserRepo _userRepo;
        private readonly IOrganizerRepo _organizerRepo;
        private readonly IConfiguration _configuration;
        private readonly DecodeToken _decodeToken;
        private readonly IShowOrderService _showOrderService;
        private readonly IShowTransactionService _showTransactionService;
        public OrganizerService(
        IShowTransactionService showTransactionService,
           IUserRepo userRepo,
           IOrganizerRepo organizerRepo,
           IShowOrderService showOrderService,
           IConfiguration configuration)
        {
            _userRepo = userRepo;
            _showTransactionService = showTransactionService;
            _organizerRepo = organizerRepo;
            _decodeToken = new DecodeToken();
            _configuration = configuration;
            _showOrderService = showOrderService;
        }

        //Admin insert organizer account
        public async Task<bool> InsertOrganizer(string token, OrganizerAccountInsertModel organizer)
        {
            if (organizer == null || organizer.UserName == null || organizer.Address == null
                || organizer.AvatarImgURL == null || organizer.Phone == null)
            {
                throw new ArgumentNullException("Organizer Null");
            }

            if (string.IsNullOrEmpty(organizer.Email) || string.IsNullOrEmpty(organizer.Pwd))
            {
                throw new ArgumentException("Email Null || Password Null");
            }

            int roleID = _decodeToken.Decode(token, Commons.JWTClaimRoleID);
            if (!roleID.Equals(0)) throw new ArgumentException("Only admin can access this resource");

            try
            {
                var auth = new FirebaseAuthProvider(new FirebaseConfig(_configuration.GetSection("Firebase:ApiKey").Value));
                var organizerAccount = await auth.CreateUserWithEmailAndPasswordAsync(organizer.Email, organizer.Pwd, "Organizer", true);
                if (organizerAccount != null)
                {
                    Guid Id = new();

                    AppUser organizerAdd = new()
                    {
                        Id = Id,
                        Username = organizer.UserName,
                        Email = organizer.Email,
                        IsActive = organizer.IsActive,
                        RoleId = organizer.RoleID,
                        Password = organizer.Pwd,
                        AvatarImageUrl = organizer.AvatarImgURL

                    };

                    //Add AppUser account to DB - AppUser Table
                    Id = await _userRepo.Insert(organizerAdd);

                    Organizer organizerAcc = new()
                    {
                        Id = Id,
                        AvatarImgUrl = organizer.AvatarImgURL,
                        Address = organizer.Address,
                        PhoneNumber = organizer.Phone,
                        TaxNumber = organizer.TaxNumber,
                        TaxLocation = organizer.TaxIssueLocation
                       
                    };

                    //Add Organizer account to DB - Organizer Table
                    Id = await _organizerRepo.Insert(organizerAcc);

                    return (organizerAcc != null);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return false;
        }

        public async Task<List<OrganizerViewModel>> GetOrganizerAccounts(string token)
        {
            int roleID = _decodeToken.Decode(token, Commons.JWTClaimRoleID);
            if (!roleID.Equals(0)) throw new ArgumentException("Only admin can access this resource");

            try
            {

                List<OrganizerViewModel> result = await _organizerRepo.GetOrganizers();

                foreach(OrganizerViewModel organizer in result)
                {
                   double income = 0;
                   List<ShowTransactionRequestViewModel> order = await _showTransactionService.GetShowTransactionsByOrganizerID(token, organizer.ID);
                    foreach (ShowTransactionRequestViewModel showTrans in order) {
                        income += (showTrans.Revenue -showTrans.ArtistCommission); 
                    }
                    organizer.Income = income;
                }

                if (result.Count > 0) return result;

            }
            catch (Exception ex)
            {
                throw new ArgumentException("Internal Server Error", ex.Message);
            }

            return new List<OrganizerViewModel>();
        }

        public async Task<List<OrganizerInfoModel>> GetOrganizerInfo()
        {

            try
            {
                List<OrganizerInfoModel> result = await _organizerRepo.GetOrganizersInfo();



                if (result.Count > 0) return result;

            }
            catch (Exception ex)
            {
                throw new ArgumentException("Internal Server Error", ex.Message);
            }

            return new List<OrganizerInfoModel>();
        }
    }
}
