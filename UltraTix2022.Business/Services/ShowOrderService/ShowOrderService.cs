using Newtonsoft.Json.Linq;
using NuGet.Common;
using UltraTix2022.API.UltraTix2022.Business.Utilities;
using UltraTix2022.API.UltraTix2022.Data.Models.Commons;
using UltraTix2022.API.UltraTix2022.Data.Models.RequestModel.Entities.ShowOrder;
using UltraTix2022.API.UltraTix2022.Data.Models.ViewModels.Entities.ShowOrder;
using UltraTix2022.API.UltraTix2022.Data.Repositories.ImplementedRepositories.CampaignDetailRepo;
using UltraTix2022.API.UltraTix2022.Data.Repositories.ImplementedRepositories.CampaignRepo;
using UltraTix2022.API.UltraTix2022.Data.Repositories.ImplementedRepositories.LocationRepo;
using UltraTix2022.API.UltraTix2022.Data.Repositories.ImplementedRepositories.OrganizerRepo;
using UltraTix2022.API.UltraTix2022.Data.Repositories.ImplementedRepositories.SaleStageDetailRepo;
using UltraTix2022.API.UltraTix2022.Data.Repositories.ImplementedRepositories.SaleStageRepo;
using UltraTix2022.API.UltraTix2022.Data.Repositories.ImplementedRepositories.ShowCategoryRepo;
using UltraTix2022.API.UltraTix2022.Data.Repositories.ImplementedRepositories.ShowOrderDetailRepo;
using UltraTix2022.API.UltraTix2022.Data.Repositories.ImplementedRepositories.ShowOrderRepo;
using UltraTix2022.API.UltraTix2022.Data.Repositories.ImplementedRepositories.ShowRepo;
using UltraTix2022.API.UltraTix2022.Data.Repositories.ImplementedRepositories.ShowTypeRepo;
using UltraTix2022.API.UltraTix2022.Data.Repositories.ImplementedRepositories.TickTypeRepo;
using UltraTix2022.API.UltraTix2022.Data.Repositories.ImplementedRepositories.UserRepo;

namespace UltraTix2022.API.UltraTix2022.Business.Services.ShowOrderService
{
    public class ShowOrderService : IShowOrderService
    {
        private readonly IConfiguration _configuration;
        private readonly DecodeToken _decodeToken;

        private readonly IShowOrderRepo _showOrderRepo;
        private readonly IShowOrderDetailRepo _showOrderDetailRepo;
        private readonly ICampaignDetailRepo _campaignDetailRepo;
        private readonly ITicketTypeRepo _ticketTypeRepo;
        private readonly IShowRepo _showRepo;
        private readonly ILocationRepo _locationRepo;
        private readonly ISaleStageRepo _saleStageRepo;
        private readonly ISaleStageDetailRepo _saleStageDetailRepo;
        private readonly ICampaignRepo _campaignRepo;
        private readonly IOrganizerRepo _organizerRepo;
        private readonly IShowCategoryRepo _showCategoryRepo;
        private readonly IShowTypeRepo _showTypeRepo;
        private readonly IUserRepo _userRepo;


        public ShowOrderService(
           IShowOrderRepo showOrderRepo,
           IConfiguration configuration,
           IShowOrderDetailRepo showOrderDetailRepo,
           ICampaignDetailRepo campaignDetailRepo,
           ITicketTypeRepo ticketTypeRepo,
           IShowRepo showRepo,
           ILocationRepo locationRepo,
           ISaleStageRepo saleStageRepo,
           ISaleStageDetailRepo saleStageDetailRepo,
           ICampaignRepo campaignRepo,
           IOrganizerRepo organizerRepo,
           IShowCategoryRepo showCategoryRepo,
           IShowTypeRepo showTypeRepo,
           IUserRepo userRepo
           )
        {
            _showOrderRepo = showOrderRepo;
            _decodeToken = new DecodeToken();
            _configuration = configuration;
            _showOrderDetailRepo = showOrderDetailRepo;
            _showRepo = showRepo;
            _decodeToken = new DecodeToken();
            _configuration = configuration;
            _locationRepo = locationRepo;
            _saleStageRepo = saleStageRepo;
            _saleStageDetailRepo = saleStageDetailRepo;
            _campaignRepo = campaignRepo;
            _campaignDetailRepo = campaignDetailRepo;
            _organizerRepo = organizerRepo;
            _showCategoryRepo = showCategoryRepo;
            _showTypeRepo = showTypeRepo;
            _ticketTypeRepo = ticketTypeRepo;
            _userRepo = userRepo;

        }

        public Task<bool> CreateShowOrder(string token, ShowOrderRequestModel showOrder)
        {
            throw new NotImplementedException();
        }

        public async Task<List<ShowOrderRequestViewModel>> GetShowOrders(string token)
        {
            int roleID = _decodeToken.Decode(token, Commons.JWTClaimRoleID);
            if (!roleID.Equals(Commons.CUSTOMER)) throw new ArgumentException(Commons.ERROR_403_FORBIDDEN_MSG);
            Guid userID = _decodeToken.DecodeID(token, Commons.JWTClaimID);

            var showOrders = await _showOrderRepo.GetShowOrdersByCustomerId(userID);

            foreach (var order in showOrders)
            {
                var orderDetails = await _showOrderDetailRepo.GetShowOrdersDetailByShowOrderId(order.Id);
                var show = await _showRepo.Get(order.ShowId);
                var location = await _locationRepo.GetLocationByShowID(show.Id);
                order.ShowName = show.ShowName;
                order.ShowStartDate = show.ShowStartDate;
                order.ShowEndDate = show.ShowEndDate;
                order.Location = "Hà Nội";

                if (location != null)
                    order.Location = location.LocationDescription;


                if (order.CampaignId != null)
                {
                    order.BuyFromArtist = await _campaignRepo.GetArtistNameByCampaignID(order.CampaignId ?? new Guid());
                    if (order.BuyFromArtist == null) throw new ArgumentNullException("Artist Name Not Found With ID: " + order.CampaignId);
                }

                order.ShowOrderDetailViewRequestModels.AddRange(orderDetails);

                foreach (var orderDetail in order.ShowOrderDetailViewRequestModels)
                {
                    if (orderDetail.SaleStageDetailId != null)
                    {
                        var saleStageDetail = await _saleStageDetailRepo.Get(orderDetail.SaleStageDetailId ?? new Guid());
                        if (saleStageDetail == null) throw new ArgumentNullException("SaleStageDetail Not Found With ID: " + orderDetail.SaleStageDetailId);
                        var saleStage = await _saleStageRepo.Get(saleStageDetail.SaleStageId);
                        orderDetail.SaleStageName = saleStage.SaleStageOrder;
                        var ticketType = await _ticketTypeRepo.Get(saleStageDetail.TicketTypeId);
                        orderDetail.TicketTypeName = ticketType.TicketTypeName;
                        orderDetail.SaleStageDiscount = saleStage.SaleStageDiscount;
                        orderDetail.TicketTypeDiscount = ticketType.TicketTypeDiscount;
                    }

                    if (order.CampaignId != null)
                    {
                        var campaignDetail = await _campaignDetailRepo.Get(orderDetail.CampaignDetailId ?? new Guid());
                        if (campaignDetail == null) throw new ArgumentNullException("CampaignDetail Not Found With ID: " + orderDetail.CampaignDetailId + "\n and orderDetail ID: "+orderDetail.Id);
                        orderDetail.CampaignName = campaignDetail.CampaignName;
                        orderDetail.CampaignDiscount = campaignDetail.CustomerDiscount ?? 0;
                    }
                }
            }

            return showOrders;
        }

        public async Task<ShowOrderRequestViewModel> GetShowOrderById(string token, Guid orderId)
        {
            int roleID = _decodeToken.Decode(token, Commons.JWTClaimRoleID);
            if (!roleID.Equals(Commons.TICKETINSPECTOR) && !roleID.Equals(Commons.STAFF)) throw new ArgumentException(Commons.ERROR_403_FORBIDDEN_MSG);

            var showOrderEntity = await _showOrderRepo.Get(orderId);

            if (showOrderEntity == null) throw new ArgumentException("Show Order Not Found With ID: " + orderId);

            if (!await IsOrderBelongToOrganizerShow(token, orderId)) throw new ArgumentException("ShowOrder Not Belong To Organizer");

            ShowOrderRequestViewModel result = new()
            {
                Id = showOrderEntity.Id,
                CustomerId = showOrderEntity.CustomerId,
                CampaignId = showOrderEntity.CampaignId,
                OrderDate = showOrderEntity.OrderDate,
                OrderDescription = showOrderEntity.OrderDescription,
                ShowId = showOrderEntity.ShowId,
                TotalPay = showOrderEntity.TotalPay,
                IsUsed = showOrderEntity.IsUsed ?? false
            };

            var orderDetails = await _showOrderDetailRepo.GetShowOrdersDetailByShowOrderId(showOrderEntity.Id);
            var show = await _showRepo.Get(showOrderEntity.ShowId);
            var location = await _locationRepo.GetLocationByShowID(show.Id);
            result.ShowName = show.ShowName;
            result.ShowStartDate = show.ShowStartDate;
            result.ShowEndDate = show.ShowEndDate;
            result.Location = "Hà Nội";

            if (location != null)
                result.Location = location.LocationDescription;

            if (showOrderEntity.CampaignId != null)
            {
                result.BuyFromArtist = await _campaignRepo.GetArtistNameByCampaignID(showOrderEntity.CampaignId ?? new Guid());
            }

            result.ShowOrderDetailViewRequestModels.AddRange(orderDetails);

            foreach (var orderDetail in result.ShowOrderDetailViewRequestModels)
            {
                if (orderDetail.SaleStageDetailId != null)
                {
                    var saleStageDetail = await _saleStageDetailRepo.Get(orderDetail.SaleStageDetailId ?? new Guid());
                    var saleStage = await _saleStageRepo.Get(saleStageDetail.SaleStageId);
                    orderDetail.SaleStageName = saleStage.SaleStageOrder;
                    var ticketType = await _ticketTypeRepo.Get(saleStageDetail.TicketTypeId);
                    orderDetail.TicketTypeName = ticketType.TicketTypeName;
                    orderDetail.SaleStageDiscount = saleStage.SaleStageDiscount;
                    orderDetail.TicketTypeDiscount = ticketType.TicketTypeDiscount;
                }

                if (showOrderEntity.CampaignId != null)
                {
                    var campaignDetail = await _campaignDetailRepo.Get(orderDetail.CampaignDetailId ?? new Guid());
                    orderDetail.CampaignName = campaignDetail.CampaignName;
                    orderDetail.CampaignDiscount = campaignDetail.CustomerDiscount ?? 0;
                }
            }

            return result;
        }

        public async Task<bool> ScanShowOrderById(string token, Guid orderId)
        {
            int roleID = _decodeToken.Decode(token, Commons.JWTClaimRoleID);
            if (!roleID.Equals(Commons.TICKETINSPECTOR) && !roleID.Equals(Commons.STAFF)) throw new ArgumentException(Commons.ERROR_403_FORBIDDEN_MSG);

            var showOrderEntity = await _showOrderRepo.Get(orderId);

            if (showOrderEntity == null) throw new ArgumentException("Show Order Not Found With ID: " + orderId);

            var isUpdateStatusAfterScanned = await _showOrderRepo.UpdateShowOrderStatusAfterScanned(orderId);

            return isUpdateStatusAfterScanned;
        }

        public async Task<List<ShowOrderRequestViewModel>> GetShowOrdersForOrganizer(string token)
        {
            int roleID = _decodeToken.Decode(token, Commons.JWTClaimRoleID);
            if (!roleID.Equals(Commons.ORGANIZER)) throw new ArgumentException(Commons.ERROR_403_FORBIDDEN_MSG);
            Guid userID = _decodeToken.DecodeID(token, Commons.JWTClaimID);

            var showOrders = await _showOrderRepo.GetShowOrdersByOrganizerId(userID);

            foreach (var order in showOrders)
            {
                var orderDetails = await _showOrderDetailRepo.GetShowOrdersDetailByShowOrderId(order.Id);
                var show = await _showRepo.Get(order.ShowId);
                order.ShowName = show.ShowName;

                if (order.CampaignId != null)
                {
                    order.BuyFromArtist = await _campaignRepo.GetArtistNameByCampaignID(order.CampaignId ?? new Guid());
                }

                order.ShowOrderDetailViewRequestModels.AddRange(orderDetails);

                foreach (var orderDetail in order.ShowOrderDetailViewRequestModels)
                {
                    if (orderDetail.SaleStageDetailId != null)
                    {
                        var saleStageDetail = await _saleStageDetailRepo.Get(orderDetail.SaleStageDetailId ?? new Guid());
                        var saleStage = await _saleStageRepo.Get(saleStageDetail.SaleStageId);
                        orderDetail.SaleStageName = saleStage.SaleStageOrder;
                        var ticketType = await _ticketTypeRepo.Get(saleStageDetail.TicketTypeId);
                        orderDetail.TicketTypeName = ticketType.TicketTypeName;
                        orderDetail.SaleStageDiscount = saleStage.SaleStageDiscount;
                        orderDetail.TicketTypeDiscount = ticketType.TicketTypeDiscount;
                        orderDetail.OriginalPrice = orderDetail.SubTotal / orderDetail.QuantityBuy / (1 - orderDetail.TicketTypeDiscount / 100) / (1 - orderDetail.SaleStageDiscount / 100);
                    }

                    if (order.CampaignId != null)
                    {
                        var campaignDetail = await _campaignDetailRepo.Get(orderDetail.CampaignDetailId ?? new Guid());
                        orderDetail.CampaignName = campaignDetail.CampaignName;
                        orderDetail.CampaignDiscount = campaignDetail.CustomerDiscount ?? 0;
                        orderDetail.OriginalPrice = orderDetail.SubTotal / orderDetail.QuantityBuy / (1 - orderDetail.TicketTypeDiscount / 100) / (1 - orderDetail.SaleStageDiscount / 100) / ( 1 - orderDetail.CampaignDiscount / 100);
                    }
                }
            }

            return showOrders;
        }

        public async Task<bool> IsOrderBelongToOrganizerShow(string token, Guid orderId)
        {
            int roleID = _decodeToken.Decode(token, Commons.JWTClaimRoleID);
            Guid staffID = _decodeToken.DecodeID(token, Commons.JWTClaimID);
            
            if (!roleID.Equals(2)) throw new ArgumentException(Commons.ERROR_403_FORBIDDEN_MSG);

            var organizer = await _organizerRepo.GetOrganizerbyStaffID(staffID);
            if (organizer == null) throw new ArgumentException("Staff Not Belong To Any Organizer");

            var order = await _showOrderRepo.Get(orderId);
            
            if (order == null) throw new ArgumentException("Show Order Not Found In DB");
            
            var isOrderBelongToShowOfOrganizer = await _showRepo.IsShowBelongToOrganizer(organizer.ID, order.ShowId);
            
            if (isOrderBelongToShowOfOrganizer) return true;
            
            return false;
        }

        public Task<List<ShowOrderRequestViewModel>> GetAllShowOrders(string token)
        {
            int roleID = _decodeToken.Decode(token, Commons.JWTClaimRoleID);
            if (!roleID.Equals(Commons.ADMIN)) throw new ArgumentException(Commons.ERROR_403_FORBIDDEN_MSG);
            try
            {
                var list = _showOrderRepo.GetShowOrders();

                return list;

            }catch(Exception e)
            {
                throw new ArgumentException(e.ToString());
            }
        }

        public async Task<List<ShowOrderRequestViewModel>> GetShowOrdersForOrganizerByID(Guid organizerID, string token)
        {
            int roleID = _decodeToken.Decode(token, Commons.JWTClaimRoleID);
            if (!roleID.Equals(Commons.ADMIN)) throw new ArgumentException(Commons.ERROR_403_FORBIDDEN_MSG);


            var showOrders = await _showOrderRepo.GetShowOrdersByOrganizerId(organizerID);

            foreach (var order in showOrders)
            {
                var orderDetails = await _showOrderDetailRepo.GetShowOrdersDetailByShowOrderId(order.Id);
                var show = await _showRepo.Get(order.ShowId);
                order.ShowName = show.ShowName;

                if (order.CampaignId != null)
                {
                    order.BuyFromArtist = await _campaignRepo.GetArtistNameByCampaignID(order.CampaignId ?? new Guid());
                }

                order.ShowOrderDetailViewRequestModels.AddRange(orderDetails);

                foreach (var orderDetail in order.ShowOrderDetailViewRequestModels)
                {
                    if (orderDetail.SaleStageDetailId != null)
                    {
                        var saleStageDetail = await _saleStageDetailRepo.Get(orderDetail.SaleStageDetailId ?? new Guid());
                        var saleStage = await _saleStageRepo.Get(saleStageDetail.SaleStageId);
                        orderDetail.SaleStageName = saleStage.SaleStageOrder;
                        var ticketType = await _ticketTypeRepo.Get(saleStageDetail.TicketTypeId);
                        orderDetail.TicketTypeName = ticketType.TicketTypeName;
                        orderDetail.SaleStageDiscount = saleStage.SaleStageDiscount;
                        orderDetail.TicketTypeDiscount = ticketType.TicketTypeDiscount;
                    }

                    if (order.CampaignId != null)
                    {
                        var campaignDetail = await _campaignDetailRepo.Get(orderDetail.CampaignDetailId ?? new Guid());
                        orderDetail.CampaignName = campaignDetail.CampaignName;
                        orderDetail.CampaignDiscount = campaignDetail.CustomerDiscount ?? 0;
                    }
                }
            }

            return showOrders;
        }
    }
}
