using Firebase.Auth;
using UltraTix2022.API.UltraTix2022.Business.Utilities;
using UltraTix2022.API.UltraTix2022.Data.Models.Commons;
using UltraTix2022.API.UltraTix2022.Data.Models.Entities;
using UltraTix2022.API.UltraTix2022.Data.Models.ViewModels.Entities.User;
using UltraTix2022.API.UltraTix2022.Data.Repositories.ImplementedRepositories.ArtistFollowerRepo;
using UltraTix2022.API.UltraTix2022.Data.Repositories.ImplementedRepositories.ArtistRepo;
using UltraTix2022.API.UltraTix2022.Data.Repositories.ImplementedRepositories.UserRepo;

namespace UltraTix2022.API.UltraTix2022.Business.Services.ArtistService
{
    public class ArtistService : IArtistService
    {
        private readonly IUserRepo _userRepo;
        private readonly IArtistRepo _artistRepo;
        private readonly IConfiguration _configuration;
        private readonly DecodeToken _decodeToken;
        private readonly IArtistFollowerRepo _artistFollowerRepo;

        public ArtistService(
           IUserRepo userRepo,
           IArtistRepo artistRepo,
           IArtistFollowerRepo artistFollowerRepo,
            IConfiguration configuration)
        {
            _userRepo = userRepo;
            _artistRepo = artistRepo;
            _decodeToken = new DecodeToken();
            _artistFollowerRepo = artistFollowerRepo;
            _configuration = configuration;
        }

        //Admin insert organizer account
        public async Task<bool> InsertArtist(string token, ArtistInsertModel artist)
        {
            if (artist == null || artist.UserName == null || artist.Address == null
                || artist.AvatarImgURL == null || artist.Phone == null)
            {
                throw new ArgumentNullException("Artist Null");
            }

            if (string.IsNullOrEmpty(artist.Email) || string.IsNullOrEmpty(artist.Pwd))
            {
                throw new ArgumentException("Email Null || Password Null");
            }

            int roleID = _decodeToken.Decode(token, Commons.JWTClaimRoleID);
            if (!roleID.Equals(0)) throw new ArgumentException("Only admin can access this resource");

            try
            {
                var auth = new FirebaseAuthProvider(new FirebaseConfig(_configuration.GetSection("Firebase:ApiKey").Value));
                var artistAccount = await auth.CreateUserWithEmailAndPasswordAsync(artist.Email, artist.Pwd, "Organizer", true);
                if (artistAccount != null)
                {
                    Guid Id = new();

                    AppUser artistAdd = new()
                    {
                        Id = Id,
                        Username = artist.UserName,
                        Email = artist.Email,
                        IsActive = artist.IsActive,
                        RoleId = artist.RoleID,
                        Password = artist.Pwd,
                        AvatarImageUrl = artist.AvatarImgURL
                    };

                    //Add AppUser account to DB - AppUser Table
                    Id = await _userRepo.Insert(artistAdd);

                    Artist artistEntity = new()
                    {
                        Id = Id,
                        AvatarImgUrl = artist.AvatarImgURL,
                        Address = artist.Address,
                        PhoneNumber = artist.Phone
                    };

                    //Add Artist account to DB - Organizer Table
                    Id = await _artistRepo.Insert(artistEntity);

                    return (artistEntity != null);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return false;
        }

        public async Task<List<ArtistAccountViewModel>> GetArtistAccounts(string token)
        {
            int roleID = _decodeToken.Decode(token, Commons.JWTClaimRoleID);
            if (!roleID.Equals(0) && !roleID.Equals(1) && !roleID.Equals(2)) throw new ArgumentException("Only admin can access this resource");

            try
            {
                List<ArtistAccountViewModel> result = await _artistRepo.getList();

                if (result.Count > 0) return result;

            }
            catch (Exception ex)
            {
                throw new ArgumentException("Internal Server Error", ex.Message);
            }

            return new List<ArtistAccountViewModel>();
        }
        public async Task<List<ArtistListViewModel>> GetArtistList(string token)
        {
            try
            {
                List<ArtistListViewModel> result = await _artistRepo.getListActive();

                if (!string.IsNullOrEmpty(token))
                {
                    Guid userId = _decodeToken.DecodeID(token, Commons.JWTClaimID);

                    var artFollowed = await _artistFollowerRepo.GetArtistsFollowed(userId);

                    foreach (var art in result)
                    {
                        foreach (var artist in artFollowed)
                        {
                            if (art.id.Equals(artist))
                            {
                                art.IsFollowed = true;
                            }
                        }
                    }
                }

                if (result.Count > 0) return result;

            }
            catch (Exception ex)
            {
                throw new ArgumentException("Internal Server Error", ex.Message);
            }

            return new List<ArtistListViewModel>();
        }

        public async Task<bool> FollowArtist(string token, Guid artistId)
        {
            try
            {
                int roleID = _decodeToken.Decode(token, Commons.JWTClaimRoleID);
                if (!roleID.Equals(Commons.ADMIN) && !roleID.Equals(Commons.ORGANIZER) &&
                    !roleID.Equals(Commons.STAFF) && !roleID.Equals(Commons.ARTIST) &&
                    !roleID.Equals(Commons.CUSTOMER)) throw new ArgumentException(Commons.ERROR_403_FORBIDDEN_MSG);

                Guid userId = _decodeToken.DecodeID(token, Commons.JWTClaimID);

                var isFollowed = await _artistFollowerRepo.IsFollowedArtist(userId, artistId);

                if (isFollowed)
                {
                    var isUnFollowSuccess = await _artistFollowerRepo.UnFollowArtist(userId, artistId);
                    return isUnFollowSuccess;
                }
                else
                {
                    ArtistFollower artistFollower = new()
                    {
                        Id = Guid.NewGuid(),
                        ArtistId = artistId,
                        FollowerId = userId
                    };
                    try
                    {
                        await _artistFollowerRepo.Insert(artistFollower);
                        return true;
                    }catch(Exception e)
                    {
                        throw new ArgumentException("Follow artist failed with artistid: " + artistId + "\n and userId: " + userId);
                    }
                }
            }catch(Exception e)
            {
                throw new ArgumentException("Follow artist fail with id: " + artistId + "message: " + e.Message);
            }

        }

        public async Task<List<Guid>> GetArtistsFollowed(string token)
        {
            int roleID = _decodeToken.Decode(token, Commons.JWTClaimRoleID);
            if (!roleID.Equals(Commons.ADMIN) && !roleID.Equals(Commons.ORGANIZER) &&
                !roleID.Equals(Commons.STAFF) && !roleID.Equals(Commons.ARTIST) &&
                !roleID.Equals(Commons.CUSTOMER)) throw new ArgumentException(Commons.ERROR_403_FORBIDDEN_MSG);

            Guid userId = _decodeToken.DecodeID(token, Commons.JWTClaimID);

            var result = await _artistFollowerRepo.GetArtistsFollowed(userId);
            return result;
        }
    }

}
