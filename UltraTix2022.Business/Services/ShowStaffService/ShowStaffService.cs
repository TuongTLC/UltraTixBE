using Firebase.Auth;
using UltraTix2022.API.UltraTix2022.Business.Utilities;
using UltraTix2022.API.UltraTix2022.Data.Models.Commons;
using UltraTix2022.API.UltraTix2022.Data.Models.Entities;
using UltraTix2022.API.UltraTix2022.Data.Models.RequestModel.Entities.AppUser;
using UltraTix2022.API.UltraTix2022.Data.Models.ViewModels.Entities.User;
using UltraTix2022.API.UltraTix2022.Data.Repositories.ImplementedRepositories.OrganizerRepo;
using UltraTix2022.API.UltraTix2022.Data.Repositories.ImplementedRepositories.ShowStaffRepo;
using UltraTix2022.API.UltraTix2022.Data.Repositories.ImplementedRepositories.UserRepo;

namespace UltraTix2022.API.UltraTix2022.Business.Services.ShowStaffService
{
    public class ShowStaffService : IShowStaffService
    {
        private readonly IUserRepo _userRepo;
        private readonly IShowStaffRepo _showStaffRepo;
        private readonly IConfiguration _configuration;
        private readonly DecodeToken _decodeToken;
        private readonly IOrganizerRepo _organizerRepo;

        public ShowStaffService(
           IUserRepo userRepo,
           IShowStaffRepo showStaffRepo,
           IConfiguration configuration,
           IOrganizerRepo organizerRepo)
        {
            _userRepo = userRepo;
            _showStaffRepo = showStaffRepo;
            _decodeToken = new DecodeToken();
            _configuration = configuration;
            _organizerRepo = organizerRepo;
        }

        //Admin insert organizer account
        public async Task<bool> InsertShowStaff(string token, ShowStaffAccountInsertModel showstaff)
        {
            if (showstaff == null || showstaff.UserName == null || showstaff.Address == null
                || showstaff.AvatarImgURL == null || showstaff.Phone == null)
            {
                throw new ArgumentNullException("ShowStaff Null");
            }

            if (string.IsNullOrEmpty(showstaff.Email) || string.IsNullOrEmpty(showstaff.Pwd))
            {
                throw new ArgumentException("Email Null || Password Null");
            }

            int roleID = _decodeToken.Decode(token, "RoleID");
            if (!roleID.Equals(1)) throw new ArgumentException("Only organizer can access this resource");

            //Get Organizer RequestID
            Guid ID = _decodeToken.DecodeID(token, Commons.JWTClaimID);

            try
            {
                var auth = new FirebaseAuthProvider(new FirebaseConfig(_configuration.GetSection("Firebase:ApiKey").Value));
                var showStaffAccount = await auth.CreateUserWithEmailAndPasswordAsync(showstaff.Email, showstaff.Pwd, "ShowStaff", true);
                if (showStaffAccount != null)
                {
                    Guid Id = new();

                    AppUser showStaffAdd = new()
                    {
                        Id = Id,
                        Username = showstaff.UserName,
                        Email = showstaff.Email,
                        IsActive = showstaff.IsActive,
                        RoleId = showstaff.RoleID,
                        Password = showstaff.Pwd,
                        AvatarImageUrl = showstaff.AvatarImgURL
                    };

                    //Add AppUser account to DB - AppUser Table
                    Id = await _userRepo.Insert(showStaffAdd);

                    ShowStaff showStaffEntities = new()
                    {
                        Id = Id,
                        AvatarImgUrl = showstaff.AvatarImgURL,
                        Address = showstaff.Address,
                        PhoneNumber = showstaff.Phone,
                        OrganizerId = ID
                    };

                    //Add Organizer account to DB - Organizer Table
                    Id = await _showStaffRepo.Insert(showStaffEntities);

                    return (showStaffEntities != null);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return false;
        }

        public async Task<List<ShowStaffViewModel>> GetShowStaffAccounts(string token)
        {
            int roleID = _decodeToken.Decode(token, Commons.JWTClaimRoleID);

            if (!roleID.Equals(0) && !roleID.Equals(1)) throw new ArgumentException("Only admin and staff can access this resource");

            try
            {

                switch (roleID)
                {
                    case 0:
                        {
                            List<ShowStaffViewModel> result = await _showStaffRepo.GetShowStaffs();

                            foreach (var staff in result)
                            {
                                var organizer = await _organizerRepo.GetProfileByID(staff.OrganizerID);
                                staff.OrganizerName = organizer.FullName;
                            }

                            if (result.Count > 0) return result;
                            break;
                        }

                    case 1:
                        {
                            Guid organizerID = _decodeToken.DecodeID(token, Commons.JWTClaimID);

                            List<ShowStaffViewModel> result = await _showStaffRepo.GetShowStaffsByOrganizer(organizerID);

                            foreach (var staff in result)
                            {
                                var organizer = await _organizerRepo.GetProfileByID(staff.OrganizerID);
                                staff.OrganizerName = organizer.FullName;
                            }

                            if (result.Count > 0) return result;
                            break;
                        }
                }

            }
            catch (Exception ex)
            {
                throw new ArgumentException("Internal Server Error", ex.Message);
            }

            return new List<ShowStaffViewModel>();
        }

        public async Task<ShowStaffViewModel> GetShowStaffDetailById(string token, Guid staffId)
        {
            int roleID = _decodeToken.Decode(token, Commons.JWTClaimRoleID);

            if (!roleID.Equals(0) && !roleID.Equals(1) && !roleID.Equals(2)) throw new ArgumentException("Only admin and staff can access this resource");

            try
            {
                List<ShowStaffViewModel> result = await _showStaffRepo.GetShowStaffs();

                var staffDetail = result.Where(x => x.ID.Equals(staffId)).First();
                var organizer = await _organizerRepo.GetProfileByID(staffDetail.OrganizerID);
                staffDetail.OrganizerName = organizer.FullName;

                return staffDetail;

            }
            catch (Exception ex)
            {
                throw new ArgumentException("Internal Server Error", ex.Message);
            }
        }

        public async Task<bool> UpdateUserProfile(string token, UserUpdateRequestModel profile)
        {
            int roleID = _decodeToken.Decode(token, Commons.JWTClaimRoleID);
            if (!roleID.Equals(0)
                && !roleID.Equals(1)
                && !roleID.Equals(2)) throw new ArgumentException(Commons.ERROR_403_FORBIDDEN_MSG);

            var result = await _userRepo.UpdateProfile(profile);

            if (result)
            {
                return await _showStaffRepo.UpdateProfile(profile);
            }
            return false;
        }

        public async Task<bool> RemoveStaffAccount(string token, Guid staffId)
        {
            int roleID = _decodeToken.Decode(token, Commons.JWTClaimRoleID);
            if (!roleID.Equals(0)
                && !roleID.Equals(1)) throw new ArgumentException(Commons.ERROR_403_FORBIDDEN_MSG);

            return await _userRepo.DeactiveAccount(staffId);
        }
    }
}
