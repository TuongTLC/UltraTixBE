using UltraTix2022.API.UltraTix2022.Data.Models.Entities;
using UltraTix2022.API.UltraTix2022.Data.Models.RequestModel.Entities.Show;
using UltraTix2022.API.UltraTix2022.Data.Models.ViewModels.Entities.ShowRequest;

namespace UltraTix2022.API.UltraTix2022.Business.Services.ShowRequestService
{
    public interface IShowRequestService
    {
        public Task<ShowRequest> RequestShow(string token, ShowRequestModel show);

        public Task<bool> ApproveShow(string token, ShowRequestedResponseModel requestModel);

        public Task<bool> RejectShow(string token, ShowRequestedResponseModel requestModel);

        public Task<ShowRequest> GetByID(Guid ID);

        public Task<List<ShowRequestViewModel>> GetShowRequests(string token);
    }
}
