using Firebase.Auth;
using UltraTix2022.API.UltraTix2022.Business.Utilities;
using UltraTix2022.API.UltraTix2022.Data.JWT;
using UltraTix2022.API.UltraTix2022.Data.Models.Commons;
using UltraTix2022.API.UltraTix2022.Data.Models.Entities;
using UltraTix2022.API.UltraTix2022.Data.Models.RequestModel.Entities.AppUser;
using UltraTix2022.API.UltraTix2022.Data.Models.ViewModels.Entities.User;
using UltraTix2022.API.UltraTix2022.Data.Repositories.ImplementedRepositories.ArtistRepo;
using UltraTix2022.API.UltraTix2022.Data.Repositories.ImplementedRepositories.CustomerRepo;
using UltraTix2022.API.UltraTix2022.Data.Repositories.ImplementedRepositories.OrganizerRepo;
using UltraTix2022.API.UltraTix2022.Data.Repositories.ImplementedRepositories.ShowStaffRepo;
using UltraTix2022.API.UltraTix2022.Data.Repositories.ImplementedRepositories.SystemAdminRepo;
using UltraTix2022.API.UltraTix2022.Data.Repositories.ImplementedRepositories.UserRepo;

namespace UltraTix2022.API.UltraTix2022.Business.Services.UserService
{
    public class UserService : IUserService
    {
        private readonly IUserRepo _userRepo;
        private readonly IConfiguration _configuration;
        private readonly DecodeToken _decodeToken;
        private readonly IOrganizerRepo _organizerRepo;
        private readonly IShowStaffRepo _showStaffRepo;
        private readonly IArtistRepo _artistRepo;
        private readonly ICustomerRepo _customerRepo;
        private readonly ISystemAdminRepo _systemAdminRepo;

        public UserService(IUserRepo userRepo,
            IConfiguration configuration, IOrganizerRepo organizerRepo, IShowStaffRepo showStaffRepo, IArtistRepo artistRepo,
            ICustomerRepo customerRepo, ISystemAdminRepo systemAdminRepo)
        {
            _userRepo = userRepo;
            _configuration = configuration;
            _decodeToken = new();
            _organizerRepo = organizerRepo;
            _customerRepo = customerRepo;
            _artistRepo = artistRepo;
            _showStaffRepo = showStaffRepo;
            _systemAdminRepo = systemAdminRepo;

        }

        public async Task<List<AppUser>> GetAlls()
        {
            var result = await _userRepo.getList();
            return result;
        }

        public async Task<UserTokenModel> LoginWithEmail(UserLoginModel model)
        {
            //Login Firebase with email and password
            var auth = new FirebaseAuthProvider(new FirebaseConfig(_configuration.GetSection("Firebase:ApiKey").Value));
            var account = await auth.SignInWithEmailAndPasswordAsync(model.EmailOrUsername, model.Password);

            if (account == null) throw new ArgumentException(Commons.ERROR_401_LOGIN_FAILED_MSG);

            // Get user in DB by Email
            var user = await _userRepo.GetByEmail(model.EmailOrUsername);

            if (user == null) throw new Exception(Commons.ERROR_500_USER_NOT_FOUND_MSG);

            //Get Token
            user.Token = JWTUserToken.GenerateJWTTokenUser(user);
            return user;
        }

        public async Task<UserTokenModel> LoginWithUsername(UserLoginModel model)
        {
            //Get Email by Username in DB
            var userEmail = await _userRepo.GetEmailByUsername(model.EmailOrUsername);
            if (string.IsNullOrEmpty(userEmail)) throw new ArgumentException(Commons.ERROR_500_USER_NOT_FOUND_MSG);

            //Login Firebase with email and password
            var auth = new FirebaseAuthProvider(new FirebaseConfig(_configuration.GetSection("Firebase:ApiKey").Value));
            var account = await auth.SignInWithEmailAndPasswordAsync(userEmail, model.Password);

            if (account == null) throw new ArgumentException(Commons.ERROR_401_LOGIN_FAILED_MSG);

            // Get user in DB by Email
            var user = await _userRepo.GetByEmail(userEmail);

            if (user == null) throw new Exception(Commons.ERROR_500_USER_NOT_FOUND_MSG);

            //Get Token
            user.Token = JWTUserToken.GenerateJWTTokenUser(user);
            return user;
        }

        public async Task<UserProfileModel> GetUserProfileByToken(string token)
        {
            int roleID = _decodeToken.Decode(token, Commons.JWTClaimRoleID);
            Guid userID = _decodeToken.DecodeID(token, Commons.JWTClaimID);

            if (!roleID.Equals(0) && !roleID.Equals(1) && !roleID.Equals(2) && !roleID.Equals(3) && !roleID.Equals(4) && !roleID.Equals(5)
                ) throw new ArgumentException(Commons.ERROR_403_FORBIDDEN_MSG);

            UserProfileModel profile = new();

            switch (roleID)
            {
                case 0:
                    {
                        profile = await _systemAdminRepo.GetProfileByID(userID);
                        break;
                    }
                case 1:
                    {
                        profile = await _organizerRepo.GetProfileByID(userID);
                        break;
                    }
                case 2:
                    {
                        profile = await _showStaffRepo.GetProfileByID(userID);
                        break;
                    }
                case 3:
                    {
                        break;
                    }
                case 4:
                    {
                        profile = await _artistRepo.GetProfileByID(userID);
                        break;
                    }
                case 5:
                    {
                        profile = await _customerRepo.GetProfileByID(userID);
                        break;
                    }
                default:
                    {
                        throw new ArgumentException(Commons.ERROR_403_FORBIDDEN_MSG);
                    }
            }

            return profile ?? throw new Exception(Commons.ERROR_500_USER_NOT_FOUND_MSG + token);
        }

        public async Task<List<ArtistViewModel>> GetArtists()
        {
            var artists = await _artistRepo.GetArtists();

            return artists ?? new();
        }

        public async Task<bool> UpdateUserProfile(string token, UserUpdateRequestModel profile)
        {
            int roleID = _decodeToken.Decode(token, Commons.JWTClaimRoleID);
            if (!roleID.Equals(0)
                && !roleID.Equals(1)
                && !roleID.Equals(2)
                && !roleID.Equals(3)
                && !roleID.Equals(4)
                && !roleID.Equals(5)) throw new ArgumentException(Commons.ERROR_403_FORBIDDEN_MSG);
            Guid userId = _decodeToken.DecodeID(token, Commons.JWTClaimID);
            profile.Id = userId;
            var result = await _userRepo.UpdateProfile(profile);

            if (result)
            {
                switch (roleID)
                {
                    case 0:
                        {
                            break;
                        }
                    case 1:
                        {
                            await _organizerRepo.UpdateProfile(profile);
                            break;
                        }
                    case 2:
                        {
                            await _showStaffRepo.UpdateProfile(profile);
                            break;
                        }
                    case 3:
                        {
                            break;
                        }
                    case 4:
                        {
                            await _artistRepo.UpdateProfile(profile);
                            break;
                        }
                    case 5:
                        {
                            await _customerRepo.UpdateProfile(profile);
                            break;
                        }
                }

                return true;
            }

            return false;
        }

        public async Task<bool> RemoveAccount(string token, Guid userId)
        {
            int roleID = _decodeToken.Decode(token, Commons.JWTClaimRoleID);
            if (!roleID.Equals(0)
                && !roleID.Equals(1)
                && !roleID.Equals(4)
                && !roleID.Equals(5)) throw new ArgumentException(Commons.ERROR_403_FORBIDDEN_MSG);

            return await _userRepo.DeactiveAccount(userId);
        }

        public async Task<bool> SignUp(RegisterAccountModel account)
        {
            var email = await _userRepo.GetByEmail(account.Email);

            if (email != null) throw new ArgumentException("Email has been used");

            var auth = new FirebaseAuthProvider(new FirebaseConfig(_configuration.GetSection("Firebase:ApiKey").Value));
            var user = await auth.CreateUserWithEmailAndPasswordAsync(account.Email, account.Password, "User", true);
            if (user != null)
            {
                var id = Guid.NewGuid();

                var index = account.Email.IndexOf("@gmail.com");

                AppUser appUser = new()
                {
                    Id = id,
                    Username = account.Email.Substring(0, index),
                    FullName = "Nguyễn Văn A",
                    IsActive = false,
                    Email = account.Email,
                    RoleId = 5,
                    Password = account.Password,
                    AvatarImageUrl = "https://ultratiximg.blob.core.windows.net/ultratixshowimg/defaultAvatr.png"
                };

                appUser.Customer = new()
                {
                    Id = appUser.Id,
                    AvatarImgUrl = appUser.AvatarImageUrl,
                    Address = string.Empty,
                    PhoneNumber = string.Empty,
                };

                appUser.IsActive = true;

                //Add AppUser account to DB - AppUser Table
                await _userRepo.Insert(appUser);

                return true;
            }
            return false;
        }

        public async Task<bool> IsEmailUsed(String email)
        {
           var isUsed = await _userRepo.IsEmailUsed(email);

           return isUsed;
        }

        public async Task<string> GetEmailById(Guid id)
        {

            var profile = await _userRepo.Get(id);
            if (profile == null) throw new Exception("User not found in DB with ID: " + id);
            return profile.Email ?? "loli190516@gmail.com";
        }

        public async Task<string> GetNameById(Guid id)
        {
            var name = await _userRepo.GetNameByID(id);
            return name ?? string.Empty;
        }
    }
}
