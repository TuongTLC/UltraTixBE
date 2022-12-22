using UltraTix2022.API.UltraTix2022.Business.Utilities;
using UltraTix2022.API.UltraTix2022.Data.Models.Commons;
using UltraTix2022.API.UltraTix2022.Data.Models.Entities;
using UltraTix2022.API.UltraTix2022.Data.Models.RequestModel.Entities.Show;
using UltraTix2022.API.UltraTix2022.Data.Models.ViewModels.Entities.ShowRequest;
using UltraTix2022.API.UltraTix2022.Data.Repositories.ImplementedRepositories.OrganizerRepo;
using UltraTix2022.API.UltraTix2022.Data.Repositories.ImplementedRepositories.ShowRepo;
using UltraTix2022.API.UltraTix2022.Data.Repositories.ImplementedRepositories.ShowRequestRepo;
using UltraTix2022.API.UltraTix2022.Data.Repositories.ImplementedRepositories.ShowStaffRepo;

namespace UltraTix2022.API.UltraTix2022.Business.Services.ShowRequestService
{
    public class ShowRequestService : IShowRequestService
    {
        private readonly IConfiguration _configuration;
        private readonly DecodeToken _decodeToken;

        private readonly IShowRepo _showRepo;
        private readonly IShowRequestRepo _showRequestRepo;
        private readonly IShowStaffRepo _showStaffRepo;
        private readonly IOrganizerRepo _organizerRepo;


        public ShowRequestService(
           IShowRepo showRepo,
           IConfiguration configuration,
           IOrganizerRepo organizerRepo,
           IShowStaffRepo showStaffRepo,
           IShowRequestRepo showRequestRepo
           )
        {
            _showRepo = showRepo;
            _decodeToken = new DecodeToken();
            _configuration = configuration;
            _organizerRepo = organizerRepo;
            _showStaffRepo = showStaffRepo;
            _showRequestRepo = showRequestRepo;

        }

        public async Task<bool> ApproveShow(string token, ShowRequestedResponseModel requestModel)
        {
            int roleID = _decodeToken.Decode(token, Commons.JWTClaimRoleID);

            if (!roleID.Equals(Commons.ORGANIZER)) throw new ArgumentException(Commons.ERROR_403_FORBIDDEN_MSG);

            try
            {
                if (await _showRepo.Get(requestModel.ShowId) == null)
                {
                    throw new ArgumentException(Commons.ERROR_404_INVALID_DATA_MSG);
                }

                if (await _showRepo.UpdateShowStatus(requestModel.ShowId, Commons.PUBLIC))
                {
                    if (await _showRequestRepo.UpdateShowRequestMessage(requestModel.RequestID, requestModel.Message))
                    {
                        await _showRequestRepo.UpdateShowRequestState(requestModel.RequestID, Commons.APPROVE);
                        return true;
                    }

                }

                return false;
            }
            catch (Exception e)
            {
                throw new ArgumentException(e.Message);
            }
        }

        public async Task<ShowRequest> GetByID(Guid ID)
        {
            return await _showRequestRepo.Get(ID);
        }

        public async Task<List<ShowRequestViewModel>> GetShowRequests(string token)
        {
            int roleID = _decodeToken.Decode(token, Commons.JWTClaimRoleID);
            Guid userID = _decodeToken.DecodeID(token, Commons.JWTClaimID);

            if (!roleID.Equals(Commons.ORGANIZER)) throw new ArgumentException(Commons.ERROR_403_FORBIDDEN_MSG);

            var showRequests = await _showRequestRepo.GetShowRequests(userID);

            foreach (var showRequest in showRequests)
            {
                var show = await _showRepo.Get(showRequest.ShowId);
                var staff = await _showStaffRepo.GetProfileByID(showRequest.ShowStaffId);

                showRequest.ShowStaffName = staff.FullName;
                showRequest.ShowName = show.ShowName;
            }

            return showRequests ?? new();
        }

        public async Task<bool> RejectShow(string token, ShowRequestedResponseModel requestModel)
        {
            int roleID = _decodeToken.Decode(token, Commons.JWTClaimRoleID);

            if (!roleID.Equals(Commons.ORGANIZER)) throw new ArgumentException(Commons.ERROR_403_FORBIDDEN_MSG);

            try
            {
                if (await _showRepo.Get(requestModel.ShowId) == null)
                {
                    throw new ArgumentException(Commons.ERROR_404_INVALID_DATA_MSG);
                }

                if (await _showRepo.UpdateShowStatus(requestModel.ShowId, Commons.REJECT))
                {
                    if (await _showRequestRepo.UpdateShowRequestMessage(requestModel.RequestID, requestModel.Message))
                    {
                        await _showRequestRepo.UpdateShowRequestState(requestModel.RequestID, Commons.REJECT);
                        return true;
                    }
                }

                return false;
            }
            catch (Exception e)
            {
                throw new ArgumentException(e.Message);
            }
        }

        public async Task<ShowRequest> RequestShow(string token, ShowRequestModel show)
        {
            int roleID = _decodeToken.Decode(token, Commons.JWTClaimRoleID);
            if (!roleID.Equals(Commons.STAFF)) throw new ArgumentException("Only Staff can access this resource");

            try
            {
                ShowRequest showReq = new()
                {
                    Id = Guid.NewGuid(),
                    ShowStaffId = _decodeToken.DecodeID(token, Commons.JWTClaimID),
                    ShowId = show.ShowId,
                    RequestDate = DateTime.Now
                };

                var organizer = await _organizerRepo.GetOrganizerbyStaffID(showReq.ShowStaffId);

                if (organizer == null) throw new ArgumentException("ShowStaff are not belongs to any Organizer");
                showReq.OrganizerId = organizer.ID;
                showReq.State = Commons.PENDING;
                showReq.Message = string.Empty;

                showReq.Id = await _showRequestRepo.Insert(showReq);

                if (!await _showRepo.UpdateShowStatus(showReq.ShowId, Commons.PENDING)) throw new ArgumentException("Update Show Status Failed");

                var req = await _showRequestRepo.Get(showReq.Id);
                return req;
            }
            catch (Exception e)
            {
                throw new ArgumentException(e.Message);
            }
        }
    }
}
