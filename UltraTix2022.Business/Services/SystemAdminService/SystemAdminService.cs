using Firebase.Auth;
using UltraTix2022.API.UltraTix2022.Business.Utilities;
using UltraTix2022.API.UltraTix2022.Data.Models.Entities;
using UltraTix2022.API.UltraTix2022.Data.Models.ViewModels.Entities.User;
using UltraTix2022.API.UltraTix2022.Data.Repositories.ImplementedRepositories.SystemAdminRepo;
using UltraTix2022.API.UltraTix2022.Data.Repositories.ImplementedRepositories.UserRepo;

namespace UltraTix2022.API.UltraTix2022.Business.Services.SystemAdminService
{
    public class SystemAdminService : ISystemAdminService
    {
        private readonly IUserRepo _userRepo;
        private readonly ISystemAdminRepo _sysAdminRepo;
        private readonly IConfiguration _configuration;

        private DecodeToken _decodeToken;


        public SystemAdminService(
            IUserRepo userRepo,
            ISystemAdminRepo sysAdminRepo,
            IConfiguration configuration)
        {
            _userRepo = userRepo;
            _sysAdminRepo = sysAdminRepo;
            _decodeToken = new DecodeToken();
            _configuration = configuration;
        }

        public async Task<List<AppUser>> GetAccounts(string token)
        {
            int roleID = _decodeToken.Decode(token, "RoleID");
            if (!roleID.Equals(0)) throw new ArgumentException("Only admin can access this resource");

            try
            {
                List<AppUser> result = await _userRepo.getList();


                if (result.Count > 0) return result;

            }
            catch (Exception ex)
            {
                throw new ArgumentException("Internal Server Error", ex.Message);
            }

            return new List<AppUser>();
        }

        public async Task<List<AdminAccountViewModel>> GetAdminAccounts(string token)
        {
            int roleID = _decodeToken.Decode(token, "RoleID");
            if (!roleID.Equals(0)) throw new ArgumentException("Only admin can access this resource");

            try
            {
                List<AdminAccountViewModel> result = await _sysAdminRepo.GetAdminAccounts();

                if (result.Count > 0) return result;

            }
            catch (Exception ex)
            {
                throw new ArgumentException("Internal Server Error", ex.Message);
            }

            return new List<AdminAccountViewModel>();
        }

        //Admin insert organizer account
        public async Task<bool> InsertAdmin(string token, UserAccountInsertModel admin)
        {
            if (admin == null)
            {
                throw new ArgumentNullException("Organizer Null");
            }

            if (string.IsNullOrEmpty(admin.Email) || string.IsNullOrEmpty(admin.Pwd))
            {
                throw new ArgumentException("Email Null || Password Null");
            }

            int roleID = _decodeToken.Decode(token, "RoleID");
            if (!roleID.Equals(0)) throw new ArgumentException("Only admin can access this resource");

            try
            {
                var auth = new FirebaseAuthProvider(new FirebaseConfig(_configuration.GetSection("Firebase:ApiKey").Value));
                var adminAccount = await auth.CreateUserWithEmailAndPasswordAsync(admin.Email, admin.Pwd, "Organizer", true);
                if (adminAccount != null && !string.IsNullOrEmpty(admin.UserName))
                {
                    Guid Id = new Guid();

                    AppUser adminAdded = new AppUser()
                    {
                        Id = Id,
                        Username = admin.UserName,
                        Email = admin.Email,
                        IsActive = admin.IsActive,
                        RoleId = admin.RoleID,
                        Password = admin.Pwd
                    };

                    //Add AppUser account to DB - AppUser Table
                    Id = await _userRepo.Insert(adminAdded);

                    SystemAdmin addminAcc = new SystemAdmin()
                    {
                        Id = Id,
                    };

                    //Add Admin account to DB - System Admin Table
                    Id = await _sysAdminRepo.Insert(addminAcc);

                    return (addminAcc != null);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return false;
        }

        public async Task<bool> UpdateAccoutnStatus(string token, string status, Guid accountID)
        {
            int roleID = _decodeToken.Decode(token, "RoleID");
            if (!roleID.Equals(0)) throw new ArgumentException("Only admin can access this resource");

            try
            {

                var result = await _userRepo.UpdateAccountStatus(accountID, status);

                return result;

            }
            catch (Exception ex)
            {
                throw new ArgumentException("Internal Server Error", ex.Message);
            }

        }
    }
}
