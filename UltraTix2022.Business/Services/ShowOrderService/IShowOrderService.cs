using UltraTix2022.API.UltraTix2022.Data.Models.RequestModel.Entities.ShowOrder;
using UltraTix2022.API.UltraTix2022.Data.Models.ViewModels.Entities.ShowOrder;

namespace UltraTix2022.API.UltraTix2022.Business.Services.ShowOrderService
{
    public interface IShowOrderService
    {
        public Task<bool> CreateShowOrder(string token, ShowOrderRequestModel showOrder);
        public Task<List<ShowOrderRequestViewModel>> GetShowOrders(string token);
        public Task<List<ShowOrderRequestViewModel>> GetShowOrdersForOrganizer(string token);
        public Task<List<ShowOrderRequestViewModel>> GetShowOrdersForOrganizerByID(Guid organizerID, string token);
        public Task<ShowOrderRequestViewModel> GetShowOrderById(string token, Guid orderId);
        public Task<bool> ScanShowOrderById(string token, Guid orderId);
        public Task<bool> IsOrderBelongToOrganizerShow(string token, Guid orderId);
        public Task<List<ShowOrderRequestViewModel>> GetAllShowOrders(string token);
    }
}
