using Diacritics.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Swashbuckle.AspNetCore.Annotations;
using UltraTix2022.API.SignalR;
using UltraTix2022.API.UltraTix2022.Business.Services.EmailService;
using UltraTix2022.API.UltraTix2022.Business.Services.ImgService;
using UltraTix2022.API.UltraTix2022.Business.Services.ShowCategoryService;
using UltraTix2022.API.UltraTix2022.Business.Services.ShowOrderService;
using UltraTix2022.API.UltraTix2022.Business.Services.ShowRequestService;
using UltraTix2022.API.UltraTix2022.Business.Services.ShowService;
using UltraTix2022.API.UltraTix2022.Business.Services.ShowTransactionService;
using UltraTix2022.API.UltraTix2022.Business.Services.UserService;
using UltraTix2022.API.UltraTix2022.Business.Utilities;
using UltraTix2022.API.UltraTix2022.Data.Models.Commons;
using UltraTix2022.API.UltraTix2022.Data.Models.RequestModel.Entities.Show;
using UltraTix2022.API.UltraTix2022.Data.Models.RequestModel.ShowReview;
using UltraTix2022.API.UltraTix2022.Data.Models.ViewModels.Entities.Banner;
using UltraTix2022.API.UltraTix2022.Data.Models.ViewModels.Entities.Show;

namespace UltraTix2022.API.Controllers
{
    [Route("api/v1/show")]
    [ApiController]
    [ApiVersion("1.0")]
    [Authorize]
    public class ShowController : ControllerBase
    {
        private readonly IShowService _showService;
        private readonly ImgService _imgService = new();
        private readonly IShowCategoryService _showCategoryService;
        private readonly IHubContext<NotifyHub> _hubContext;
        private readonly IShowRequestService _showRequestService;
        private readonly IShowOrderService _showOrderService;
        private readonly IShowTransactionService _showTransactionService;
        private readonly DecodeToken _decodeToken;
        private readonly IEmailService _emailService;
        private readonly IUserService _userService;

        public ShowController(IShowService showService, IHubContext<NotifyHub> hubContext, IShowRequestService showRequestService, IShowCategoryService showCategoryService, IShowOrderService showOrderService, IShowTransactionService showTransactionService, IEmailService emailService, IUserService userService)
        {
            _showService = showService;
            _hubContext = hubContext;
            _showRequestService = showRequestService;
            _showCategoryService = showCategoryService;
            _showOrderService = showOrderService;
            _showTransactionService = showTransactionService;
            _decodeToken = new DecodeToken();
            _emailService = emailService;
            _userService = userService;
        }

        [HttpPost("insert-show")]
        [SwaggerOperation(Summary = "Insert show for staff role")]
        [Authorize(Roles = "Staff")]
        public async Task<IActionResult> InsertShow([FromBody] ShowRequestInsertModel request)
        {
            try
            {
                string token = (Request.Headers)["Authorization"].ToString().Split(" ")[1];
                var result = await _showService.InsertShow(token, request);
                return Ok(result);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (SqlException)
            {
                return StatusCode(500, "Internal Server Exception");
            }
            catch (DbUpdateException)
            {
                return StatusCode(500, "Internal Server Exception");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("insert-sale-stage")]
        [SwaggerOperation(Summary = "Insert show sale stage type for staff role")]
        [Authorize(Roles = "Staff")]
        public async Task<IActionResult> InsertShowSaleStage([FromBody] ShowSaleStagesRequestInsertModel request)
        {
            try
            {
                string token = (Request.Headers)["Authorization"].ToString().Split(" ")[1];
                var result = await _showService.InsertShowSaleStage(token, request);
                return result != null ? Ok(result) : BadRequest("Insert SaleStage To DB Failed with Show ID: " + request.ShowID);
            }
            catch (ArgumentNullException ex)
            {
                return StatusCode(404, ex);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (SqlException)
            {
                return StatusCode(500, "Internal Server Exception");
            }
            catch (DbUpdateException)
            {
                return StatusCode(500, "Internal Server Exception");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("insert-campaign")]
        [SwaggerOperation(Summary = "Insert show campaign for staff role")]
        [Authorize(Roles = "Staff")]
        public async Task<IActionResult> InsertShowCampaign([FromBody] ShowCampaignsRequestInsertModel request)
        {
            try
            {
                string token = (Request.Headers)["Authorization"].ToString().Split(" ")[1];
                var result = await _showService.InsertShowCampaign(token, request);
                return result != null ? Ok(result) : BadRequest("Insert Campaign To DB Failed with Show ID: " + request.ShowID);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (SqlException)
            {
                return StatusCode(500, "Internal Server Exception");
            }
            catch (DbUpdateException)
            {
                return StatusCode(500, "Internal Server Exception");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("update-show-status")]
        [SwaggerOperation(Summary = "Update show status for admin, organizer, staff role")]
        [Authorize(Roles = "Staff,Admin,Organizer")]
        public async Task<IActionResult> UpdateShowStatus(Guid showID, string status)
        {
            try
            {
                string token = (Request.Headers)["Authorization"].ToString().Split(" ")[1];

                foreach (var showStatus in Commons.SHOWSTATUS)
                {
                    if (status.ToLower().Trim().Equals(showStatus.ToLower().Trim()))
                    {
                        status = showStatus;
                        bool result = await _showService.UpdateShowStatus(token, showID, status);
                        return Ok(result);
                    }
                }
                return BadRequest("INVALID STATUS: " + status);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (SqlException)
            {
                return StatusCode(500, "Internal Server Exception");
            }
            catch (DbUpdateException)
            {
                return StatusCode(500, "Internal Server Exception");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("update-show")]
        [SwaggerOperation(Summary = "Update show status for admin, organizer, staff role")]
        [Authorize(Roles = "Staff")]
        public async Task<IActionResult> UpdateShow([FromBody] ShowRequestUpdateModel request)
        {
            try
            {
                string token = (Request.Headers)["Authorization"].ToString().Split(" ")[1];

                var result = await _showService.UpdateShow(token, request);

                return Ok(result);

            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (SqlException)
            {
                return StatusCode(500, "Internal Server Exception");
            }
            catch (DbUpdateException)
            {
                return StatusCode(500, "Internal Server Exception");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("update-show-sale-stage")]
        [SwaggerOperation(Summary = "Update show sale stage for admin, organizer, staff role")]
        [Authorize(Roles = "Staff")]
        public async Task<IActionResult> UpdateShowSaleStage([FromBody] ShowSaleStagesRequestUpdateModel request)
        {
            try
            {
                string token = (Request.Headers)["Authorization"].ToString().Split(" ")[1];

                var result = await _showService.UpdateShowSaleStage(token, request);

                return Ok(result);

            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (SqlException)
            {
                return StatusCode(500, "Internal Server Exception");
            }
            catch (DbUpdateException)
            {
                return StatusCode(500, "Internal Server Exception");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("update-show-campaign")]
        [SwaggerOperation(Summary = "Update show campaign for admin, organizer, staff role")]
        [Authorize(Roles = "Staff")]
        public async Task<IActionResult> UpdateShowCampaign([FromBody] ShowCampaignsRequestUpdateModel request)
        {
            try
            {
                string token = (Request.Headers)["Authorization"].ToString().Split(" ")[1];

                var result = await _showService.UpdateShowCampaign(token, request);

                return Ok(result);

            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (SqlException)
            {
                return StatusCode(500, "Internal Server Exception");
            }
            catch (DbUpdateException)
            {
                return StatusCode(500, "Internal Server Exception");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("get-shows-for-ogranizer")]
        [Authorize(Roles = "Organizer")]
        public async Task<IActionResult> GetShowsByOrganizer()
        {
            try
            {
                string token = (Request.Headers)["Authorization"].ToString().Split(" ")[1];
                var result = await _showService.GetShowsByOrganizer(token);
                return (result != null) ? Ok(result) : StatusCode(500, "Internal Server Exception");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("get-show-orders-overview-for-ogranizer")]
        [Authorize(Roles = "Organizer")]
        public async Task<IActionResult> GetShowOdersOverviewByOrganizer()
        {
            try
            {
                string token = (Request.Headers)["Authorization"].ToString().Split(" ")[1];
                var result = await _showService.GetShowOrdersOverviewByOrganizer(token);
                var shows = await _showOrderService.GetShowOrdersForOrganizer(token);
                foreach(var show in result)
                {
                    var orders = shows.Where(x => x.ShowId.Equals(show.Id)).ToList();
                    show.NumberOrders = orders.Count;
                }

                result = result.OrderByDescending(x => x.NumberOrders).ToList();

                return (result != null) ? Ok(result) : StatusCode(500, "Internal Server Exception");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("get-shows-for-ogranizer-by-id")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetShowsByOrganizerID(Guid organizerID)
        {
            try
            {
                string token = (Request.Headers)["Authorization"].ToString().Split(" ")[1];
                var result = await _showService.GetShowsByOrganizerID(token, organizerID);
                return (result != null) ? Ok(result) : StatusCode(500, "Internal Server Exception");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("get-shows-joined-for-artist")]
        [Authorize(Roles = "Artist")]
        public async Task<IActionResult> GetShowsByArtist()
        {
            try
            {
                string token = (Request.Headers)["Authorization"].ToString().Split(" ")[1];
                var result = await _showService.GetShowsByArtist(token);

                foreach(var show in result)
                {
                    var showTrans = await _showTransactionService.GetShowTransactions(token);
                    showTrans = showTrans.Where(x => x.ShowId.Equals(show.Id)).ToList();

                    var totalCommission = 0.0;
                    var totalTicketSold = 0;

                    foreach(var showTran in showTrans)
                    {
                        totalCommission += showTran.ArtistCommission;
                        totalTicketSold += showTran.TotalTicketsBuy;
                    }

                    var isAdminTransferCommission = await _showService.IsAdminTrasferShowRevenueToOrganizer(token, show.Id);

                    show.TotalArtistCommission = totalCommission;
                    show.TotalTicketSold = totalTicketSold;
                    show.IsAdminTransferCommission = isAdminTransferCommission;
                    show.TotalTicketSell = await _showService.GetTotalTicketArtistSell(show.Id, token);
                }

                return (result != null) ? Ok(result) : StatusCode(500, "Internal Server Exception");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("get-show-transactions")]
        [Authorize(Roles = "Artist,Organizer")]
        [SwaggerOperation(Summary = "get show transactions for organizer and artist")]
        public async Task<IActionResult> GetShowTransactions()
        {
            try
            {
                string token = (Request.Headers)["Authorization"].ToString().Split(" ")[1];
                var result = await _showTransactionService.GetShowTransactions(token);

                foreach(var tran in result)
                {
                    tran.Revenue = tran.Amount - tran.ArtistCommission;
                }

                return (result != null) ? Ok(result) : StatusCode(500, "Internal Server Exception");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("dashboard-overview")]
        [Authorize(Roles = "Organizer")]
        [SwaggerOperation(Summary = "get dashboard overview for organizer")]
        public async Task<IActionResult> GetDashBoardOverViewForOrganizer()
        {
            try
            {
                string token = (Request.Headers)["Authorization"].ToString().Split(" ")[1];
                var result = await _showTransactionService.GetShowTransactions(token);
                var totalTransaction = 0.0;
                var totalTicketsSold = 0;

                foreach (var transaction in result)
                {
                    totalTransaction += (transaction.Revenue - transaction.ArtistCommission);
                    totalTicketsSold += transaction.TotalTicketsBuy;
                }

                var allShows = await _showService.GetShowsByOrganizer(token);
                allShows = allShows.Where(x => x.Status.Equals(Commons.PUBLIC)).ToList();

                DashBoardOverviewModel dashBoardOverviewModel = new()
                {
                    TotalRevenueFromAllShows = totalTransaction,
                    TotalTicketsSold = totalTicketsSold,
                    TotalPublicShows = allShows.Count
                };

                return Ok(dashBoardOverviewModel);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("get-overview-shows-transactions")]
        [Authorize(Roles = "Organizer")]
        [SwaggerOperation(Summary = "get overview shows transactions for organizer")]
        public async Task<IActionResult> GetOverViewShowsTransactions()
        {
            try
            {
                string token = (Request.Headers)["Authorization"].ToString().Split(" ")[1];
                var result = await _showTransactionService.GetShowTransactions(token);

                var shows = await _showService.GetShowsByOrganizer(token);
                shows = shows.Where(x => x.Status.Equals(Commons.PUBLIC) || x.Status.Equals(Commons.ENDED)).ToList();

                List<ShowAndRevenueViewModel> showsAndRevenueListViewModel = new();

                foreach (var show in shows)
                {
                    var transacs = result.Where(x => x.ShowId.Equals(show.Id)).ToList();

                    var totalMoneyCustomerPaid = 0.0;
                    var totalArtistCommission = 0.0;
                    var totalRevenue = 0.0;

                    foreach(var transac in transacs)
                    {
                        totalMoneyCustomerPaid += transac.Revenue;
                        totalRevenue += (transac.Revenue - transac.ArtistCommission);
                        totalArtistCommission += transac.ArtistCommission;
                    }

                    var isAdminTransferRevenue = await _showService.IsAdminTrasferShowRevenueToOrganizer(token, show.Id);

                    ShowAndRevenueViewModel showAndRevenue = new()
                    {
                        ShowId = show.Id,
                        ShowName = show.ShowName,
                        TotalCustomerPaid = totalMoneyCustomerPaid,
                        ArtistCommission = totalArtistCommission,
                        Revenue = totalRevenue,
                        IsAdminTransfer = isAdminTransferRevenue
                    };

                    showsAndRevenueListViewModel.Add(showAndRevenue);

                };

                showsAndRevenueListViewModel = showsAndRevenueListViewModel.OrderByDescending(x => x.TotalCustomerPaid).ToList();

                return Ok(showsAndRevenueListViewModel);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("get-overview-shows-transactions-admin")]
        [Authorize(Roles = "Admin")]
        [SwaggerOperation(Summary = "get overview shows transactions by organizer for admin")]
        public async Task<IActionResult> GetOverViewShowsTransactionsAdmin(Guid organizerID)
        {
            try
            {
                string token = (Request.Headers)["Authorization"].ToString().Split(" ")[1];
                var result = await _showTransactionService.GetShowTransactionsByOrganizerID(token, organizerID);

                var shows = await _showService.GetShowsByOrganizerID(token,organizerID);
                shows = shows.Where(x => x.Status.Equals(Commons.PUBLIC) || x.Status.Equals(Commons.ENDED)).ToList();

                List<ShowAndRevenueViewModel> showsAndRevenueListViewModel = new();

                foreach (var show in shows)
                {
                    var transacs = result.Where(x => x.ShowId.Equals(show.Id)).ToList();

                    var totalMoneyCustomerPaid = 0.0;
                    var totalArtistCommission = 0.0;
                    var totalRevenue = 0.0;

                    foreach (var transac in transacs)
                    {
                        totalMoneyCustomerPaid += transac.Revenue;
                        totalRevenue += (transac.Revenue - transac.ArtistCommission);
                        totalArtistCommission += transac.ArtistCommission;
                    }

                    var isAdminTransferRevenue = await _showService.IsAdminTrasferShowRevenueToOrganizer(token, show.Id);

                    ShowAndRevenueViewModel showAndRevenue = new()
                    {
                        ShowId = show.Id,
                        ShowName = show.ShowName,
                        TotalCustomerPaid = totalMoneyCustomerPaid,
                        ArtistCommission = totalArtistCommission,
                        Revenue = totalRevenue,
                        IsAdminTransfer = isAdminTransferRevenue
                    };

                    showsAndRevenueListViewModel.Add(showAndRevenue);

                };

                showsAndRevenueListViewModel = showsAndRevenueListViewModel.OrderByDescending(x => x.TotalCustomerPaid).ToList();

                return Ok(showsAndRevenueListViewModel);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("get-12-months-shows-transactions")]
        [Authorize(Roles = "Organizer")]
        [SwaggerOperation(Summary = "get 12months shows transactions for organizer")]
        public async Task<IActionResult> Get12MonthsShowsTransactions()
        {
            try
            {
                string token = (Request.Headers)["Authorization"].ToString().Split(" ")[1];
                var result = await _showTransactionService.GetShowTransactions(token);

                List<RevenueByMonthModel> revenueByMonths = new();

                for (int i = 1; i <= 12; i++)
                {
                    RevenueByMonthModel revenue = new();
                    revenue.Month = i+"";

                    var month = result.First().TransactionDate.Month;

                    var transByMonth = result.Where(x => x.TransactionDate.Month.Equals(i)).ToList();

                    var totalRevenue = 0.0;

                    foreach(var tran in transByMonth)
                    {
                        totalRevenue += (tran.Revenue - tran.ArtistCommission);
                    }

                    revenue.Revenue = totalRevenue;

                    revenueByMonths.Add(revenue);
                };

                return Ok(revenueByMonths);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("get-12-months-shows-transactions-admin")]
        [Authorize(Roles = "Admin")]
        [SwaggerOperation(Summary = "get 12months shows transactions for admin")]
        public async Task<IActionResult> Get12MonthsShowsTransactionsAdmin()
        {
            try
            {
                string token = (Request.Headers)["Authorization"].ToString().Split(" ")[1];
                var result = await _showTransactionService.GetShowTransactionsForAdmin(token);

                List<RevenueByMonthModel> revenueByMonths = new();

                for (int i = 1; i <= 12; i++)
                {
                    RevenueByMonthModel revenue = new();
                    revenue.Month = i + "";

                    var month = result.First().TransactionDate.Month;

                    var transByMonth = result.Where(x => x.TransactionDate.Month.Equals(i)).ToList();

                    var totalRevenue = 0.0;

                    foreach (var tran in transByMonth)
                    {
                        totalRevenue += (tran.Revenue - tran.ArtistCommission);
                    }

                    revenue.Revenue = totalRevenue;

                    revenueByMonths.Add(revenue);
                };

                return Ok(revenueByMonths);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("get-show-transactions-for-admin")]
        [Authorize(Roles = "Admin")]
        [SwaggerOperation(Summary = "get show transactions for admin")]
        public async Task<IActionResult> GetShowTransactionsByAdmin()
        {
            try
            {
                string token = (Request.Headers)["Authorization"].ToString().Split(" ")[1];
                var result = await _showTransactionService.GetShowTransactionsForAdmin(token);
                return (result != null) ? Ok(result) : StatusCode(500, "Internal Server Exception");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("get-show-transactions-by-organizer-for-admin")]
        [Authorize(Roles = "Admin")]
        [SwaggerOperation(Summary = "get show transactions for admin")]
        public async Task<IActionResult> GetShowTransactionsbyorganizerForAdmin(Guid organizerID)
        {
            try
            {
                string token = (Request.Headers)["Authorization"].ToString().Split(" ")[1];
                var result = await _showTransactionService.GetShowTransactionsByOrganizerForAdmin(token, organizerID);
                return (result != null) ? Ok(result) : StatusCode(500, "Internal Server Exception");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("get-show-transactions-by-show-id")]
        [Authorize(Roles = "Artist,Organizer,Admin")]
        [SwaggerOperation(Summary = "get show transactions of a show for organizer and artist and admin")]
        public async Task<IActionResult> GetShowTransactionsByShowId(Guid showId)
        {
            try
            {
                string token = (Request.Headers)["Authorization"].ToString().Split(" ")[1];
                var result = await _showTransactionService.GetShowTransactions(token);
                result = result.Where(x => x.ShowId.Equals(showId)).ToList();

                foreach(var tran in result)
                {
                    tran.Revenue = tran.Amount - tran.ArtistCommission;
                    tran.CustomerName = await _userService.GetNameById(tran.CustomerId);
                }

                return (result != null) ? Ok(result) : StatusCode(500, "Internal Server Exception");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("get-show-orders-for-customer")]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> GetShowOrders()
        {
            try
            {
                string token = (Request.Headers)["Authorization"].ToString().Split(" ")[1];
                var result = await _showOrderService.GetShowOrders(token);
                result = result.OrderByDescending(x => x.OrderDate).ToList();
                return (result != null) ? Ok(result) : StatusCode(500, "Internal Server Exception");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("get-all-show-order")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllShowOrders()
        {
            try
            {
                string token = (Request.Headers)["Authorization"].ToString().Split(" ")[1];
                var result = await _showOrderService.GetAllShowOrders(token);
                result = result.OrderByDescending(x => x.OrderDate).ToList();
                return (result != null) ? Ok(result) : StatusCode(500, "Internal Server Exception");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("get-show-order-for-staff")]
        [Authorize(Roles = "Staff,TicketInspector")]
        public async Task<IActionResult> GetShowOrderById(Guid orderId)
        {
            try
            {
                string token = (Request.Headers)["Authorization"].ToString().Split(" ")[1];
                var result = await _showOrderService.GetShowOrderById(token, orderId);
                return (result != null) ? Ok(result) : StatusCode(500, "Internal Server Exception");
            }
            catch (ArgumentException e)
            {
                return StatusCode(404, e.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("scan-show-order-for-staff")]
        [Authorize(Roles = "Staff,TicketInspector")]
        public async Task<IActionResult> ScanShowOrder(Guid orderId)
        {
            try
            {
                string token = (Request.Headers)["Authorization"].ToString().Split(" ")[1];
                var result = await _showOrderService.ScanShowOrderById(token, orderId);
                return Ok(result);
            }
            catch (ArgumentException e)
            {
                return StatusCode(404, e.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("get-all-show-orders-for-organizer")]
        [Authorize(Roles = "Organizer")]
        public async Task<IActionResult> GetShowOrdersForOrganizer()
        {
            try
            {
                string token = (Request.Headers)["Authorization"].ToString().Split(" ")[1];
                var result = await _showOrderService.GetShowOrdersForOrganizer(token);
                return (result != null) ? Ok(result) : StatusCode(500, "Internal Server Exception");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("get-all-show-orders-for-organizer-by-id")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetShowOrdersForOrganizerByID(Guid organizerID)
        {
            try
            {
                string token = (Request.Headers)["Authorization"].ToString().Split(" ")[1];
                var result = await _showOrderService.GetShowOrdersForOrganizerByID(organizerID, token);
                return (result != null) ? Ok(result) : StatusCode(500, "Internal Server Exception");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("get-show-orders-by-show-id-for-organizer")]
        [Authorize(Roles = "Organizer")]
        public async Task<IActionResult> GetShowOrdersByShowId(Guid showId)
        {
            try
            {
                string token = (Request.Headers)["Authorization"].ToString().Split(" ")[1];
                var result = await _showOrderService.GetShowOrdersForOrganizer(token);
                result = result.Where(x => x.ShowId.Equals(showId)).ToList();

                foreach(var order in result)
                {
                    order.CustomerName = await _userService.GetNameById(order.CustomerId);
                }

                return (result != null) ? Ok(result) : StatusCode(500, "Internal Server Exception");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("get-tickettype-chart")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetTicketTypeChart(Guid showId)
        {
            try
            {
                string token = (Request.Headers)["Authorization"].ToString().Split(" ")[1];
                var result = await _showService.GetTicketTypeChartViewsByShowID(token, showId);
                return (result != null) ? Ok(result) : StatusCode(500, "Internal Server Exception");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("get-all-shows")]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllShow(string type = "all", string location = "all", string category = "all", int page = 1)
        {
            try
            {
                List<ShowViewModel> result = new();

                if (!string.IsNullOrEmpty(type) && !string.IsNullOrEmpty(location) && !string.IsNullOrEmpty(category))
                {
                    result = await _showService.GetPublicShow();

                    if (result.Count == 0)
                    {
                        ShowPagingViewModel showPagingViewModel = new();
                        showPagingViewModel.Shows = result;
                        showPagingViewModel.CurrentPage = 1;
                        showPagingViewModel.PageCount = 1;

                        return Ok(showPagingViewModel);
                    }
                    var categoryId = new Guid();
                    var categoryName = string.Empty;
                    if (category != "all")
                    {
                        categoryId = Guid.Parse(category);
                        categoryName = await _showCategoryService.GetCategoryNameByID(categoryId);
                    }

                    //Case type = all or location = all or category = all
                    if (type.Trim().ToLower().Equals(Commons.ALL.Trim().ToLower()) || location.Trim().ToLower().Equals(Commons.ALL.Trim().ToLower()) || category.Trim().ToLower().Equals(Commons.ALL.Trim().ToLower()))
                    {

                        //Case type = all, location = all and category = all
                        if (type.Trim().ToLower().Equals(Commons.ALL.Trim().ToLower()) && location.Trim().ToLower().Equals(Commons.ALL.Trim().ToLower()) && category.Trim().ToLower().Equals(Commons.ALL.Trim().ToLower()))
                        {
                            var showResponse = PagingShows(result, page);

                            return (showResponse != null) ? Ok(showResponse) : StatusCode(500, "Internal Server Exception - Paging Result Failed ");
                        }
                        //Case type = all or location = all
                        else if (type.Trim().ToLower().Equals(Commons.ALL.Trim().ToLower()) || location.Trim().ToLower().Equals(Commons.ALL.Trim().ToLower()))
                        {
                            //Case type = all and location = all
                            if (type.Trim().ToLower().Equals(Commons.ALL.Trim().ToLower()) && location.Trim().ToLower().Equals(Commons.ALL.Trim().ToLower()))
                            {
                                result = result.Where(show => show.CategoryID.Equals(categoryId)).ToList();
                            }
                            //Case type = all and location != all
                            else if (type.Trim().ToLower().Equals(Commons.ALL.Trim().ToLower()))
                            {
                                result = result.Where(show => show.Location.RemoveDiacritics().Trim().ToLower().Contains(location.RemoveDiacritics().Trim().ToLower())).ToList();

                                //Case category != all
                                if (!category.Trim().ToLower().Equals(Commons.ALL.Trim().ToLower()))
                                {
                                    result = result.Where(show => show.CategoryID.Equals(categoryId)).ToList();
                                }
                            }
                            //Case type != all and location = all
                            else if (location.Trim().ToLower().Equals(Commons.ALL.Trim().ToLower()))
                            {
                                result = result.Where(show => show.ShowTypeName.Trim().ToLower().Contains(type.Trim().ToLower())).ToList();

                                //Case category != all
                                if (!category.Trim().ToLower().Equals(Commons.ALL.Trim().ToLower()))
                                {
                                    result = result.Where(show => show.CategoryID.Equals(categoryId)).ToList();
                                }
                            }
                        }
                    }
                    //Case type = indoor/outdoor and location = ha noi, tp hcm, da nang,.. and category = sth
                    else
                    {
                        result = result.Where(show => show.ShowTypeName.Trim().ToLower().Contains(type.Trim().ToLower())).ToList();
                        result = result.Where(show => show.Location.RemoveDiacritics().Trim().ToLower().Contains(location.RemoveDiacritics().Trim().ToLower())).ToList();
                        result = result.Where(show => show.CategoryID.Equals(categoryId)).ToList();
                    }
                }
                else
                {
                    return StatusCode(404, "Fields Required Are Empty Or Null");
                }

                if (result == null) return BadRequest("Internal Server Error - No Result Found");

                if (result.Count == 0)
                {
                    ShowPagingViewModel showPagingViewModel = new();
                    showPagingViewModel.Shows = result;
                    showPagingViewModel.CurrentPage = 1;
                    showPagingViewModel.PageCount = 1;

                    return Ok(showPagingViewModel);
                }

                var response = PagingShows(result, page);

                return (response != null) ? Ok(response) : StatusCode(500, "Internal Server Exception - Paging Result Failed ");
            }
            catch (ArgumentException ex)
            {
                return StatusCode(500, ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("get-all-shows-no-paging")]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllShowNoPaging(string type = "all", string location = "all", string category = "all")
        {
            try
            {
                List<ShowViewModel> result = new();

                if (!string.IsNullOrEmpty(type) && !string.IsNullOrEmpty(location) && !string.IsNullOrEmpty(category))
                {
                    result = await _showService.GetPublicShow();

                    var categoryId = new Guid();
                    var categoryName = string.Empty;
                    if (category != "all")
                    {
                        categoryId = Guid.Parse(category);
                        categoryName = await _showCategoryService.GetCategoryNameByID(categoryId);
                    }

                    //Case type = all or location = all or category = all
                    if (type.Trim().ToLower().Equals(Commons.ALL.Trim().ToLower()) || location.Trim().ToLower().Equals(Commons.ALL.Trim().ToLower()) || category.Trim().ToLower().Equals(Commons.ALL.Trim().ToLower()))
                    {

                        //Case type = all, location = all and category = all
                        if (type.Trim().ToLower().Equals(Commons.ALL.Trim().ToLower()) && location.Trim().ToLower().Equals(Commons.ALL.Trim().ToLower()) && category.Trim().ToLower().Equals(Commons.ALL.Trim().ToLower()))
                        {
                            return (result != null) ? Ok(result) : StatusCode(500, "Internal Server Exception - Paging Result Failed ");
                        }
                        //Case type = all or location = all
                        else if (type.Trim().ToLower().Equals(Commons.ALL.Trim().ToLower()) || location.Trim().ToLower().Equals(Commons.ALL.Trim().ToLower()))
                        {
                            //Case type = all and location = all
                            if (type.Trim().ToLower().Equals(Commons.ALL.Trim().ToLower()) && location.Trim().ToLower().Equals(Commons.ALL.Trim().ToLower()))
                            {
                                result = result.Where(show => show.CategoryID.Equals(categoryId)).ToList();
                            }
                            //Case type = all and location != all
                            else if (type.Trim().ToLower().Equals(Commons.ALL.Trim().ToLower()))
                            {
                                result = result.Where(show => show.Location.RemoveDiacritics().Trim().ToLower().Contains(location.RemoveDiacritics().Trim().ToLower())).ToList();

                                //Case category != all
                                if (!category.Trim().ToLower().Equals(Commons.ALL.Trim().ToLower()))
                                {
                                    result = result.Where(show => show.CategoryID.Equals(categoryId)).ToList();
                                }
                            }
                            //Case type != all and location = all
                            else if (location.Trim().ToLower().Equals(Commons.ALL.Trim().ToLower()))
                            {
                                result = result.Where(show => show.ShowTypeName.Trim().ToLower().Contains(type.Trim().ToLower())).ToList();

                                //Case category != all
                                if (!category.Trim().ToLower().Equals(Commons.ALL.Trim().ToLower()))
                                {
                                    result = result.Where(show => show.CategoryID.Equals(categoryId)).ToList();
                                }
                            }
                        }
                    }
                    //Case type = indoor/outdoor and location = ha noi, tp hcm, da nang,.. and category = sth
                    else
                    {
                        result = result.Where(show => show.ShowTypeName.Trim().ToLower().Contains(type.Trim().ToLower())).ToList();
                        result = result.Where(show => show.Location.RemoveDiacritics().Trim().ToLower().Contains(location.RemoveDiacritics().Trim().ToLower())).ToList();
                        result = result.Where(show => show.CategoryID.Equals(categoryId)).ToList();
                    }
                }
                else
                {
                    return StatusCode(404, "Fields Required Are Empty Or Null");
                }

                if (result == null) return BadRequest("Internal Server Error - No Result Found");

                return (result != null) ? Ok(result) : StatusCode(500, "Internal Server Exception - Paging Result Failed ");
            }
            catch (ArgumentException ex)
            {
                return StatusCode(500, ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("get-banners")]
        [AllowAnonymous]
        public async Task<IActionResult> GetBanners()
        {
            try
            {
                var publicShows = await _showService.GetPublicShow();
                if (publicShows == null) return BadRequest("Internal Server Error - No Result Found");

                List<ShowViewModel> result = new();
                List<BannerViewModel> banners = new();

                for (int i = 0; i < 5; i++)
                {

                    Random random = new();
                    int index = random.Next(publicShows.Count);
                    result.Add(publicShows[index]);

                }

                foreach (var show in result)
                {
                    BannerViewModel banner = new()
                    {
                        Id = show.Id,
                        BannerImgUrl = show.ImgUrl,
                        Organizer = show.OrganizerName,
                        Title = show.ShowName
                    };

                    banners.Add(banner);
                }

                return (banners != null) ? Ok(banners) : StatusCode(500, "Internal Server Exception - Paging Result Failed ");
            }
            catch (ArgumentException ex)
            {
                return StatusCode(500, ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //Get Show Noi Bat
        [HttpGet("get-popular-shows")]
        [AllowAnonymous]
        public async Task<IActionResult> GetPopularShows()
        {
            try
            {
                var result = await _showService.GetPopularShows();

                if (result == null) return BadRequest("Internal Server Error - No Result Found");

                return (result != null) ? Ok(result) : StatusCode(500, "Internal Server Exception - Paging Result Failed ");
            }
            catch (ArgumentException ex)
            {
                return StatusCode(500, ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("get-random-shows")]
        [AllowAnonymous]
        public async Task<IActionResult> GetRandomShows()
        {
            try
            {
                var publicShows = await _showService.GetPublicShow();
                if (publicShows == null) return BadRequest("Internal Server Error - No Result Found");

                List<ShowViewModel> result = new();

                for (int i = 0; i < 10; i++)
                {

                    Random random = new();
                    int index = random.Next(publicShows.Count);
                    result.Add(publicShows[index]);

                }

                return (result != null) ? Ok(result) : StatusCode(500, "Internal Server Exception - Paging Result Failed ");
            }
            catch (ArgumentException ex)
            {
                return StatusCode(500, ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("get-newest-shows")]
        [AllowAnonymous]
        public async Task<IActionResult> GetNewestShows()
        {
            try
            {
                var result = await _showService.GetNewestShows();

                return (result != null) ? Ok(result) : StatusCode(500, "Internal Server Exception - Paging Result Failed ");
            }
            catch (ArgumentException ex)
            {
                return StatusCode(500, ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("get-upcoming-shows")]
        [AllowAnonymous]
        public async Task<IActionResult> GetUpCommingShows()
        {
            try
            {
                var result = await _showService.GetUpCommingShows();

                return (result != null) ? Ok(result) : StatusCode(500, "Internal Server Exception - Paging Result Failed ");
            }
            catch (ArgumentException ex)
            {
                return StatusCode(500, ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("get-shows-by-location")]
        [Authorize(Roles = "Admin,Artist,Customer,Staff,Organizer")]
        public async Task<IActionResult> GetShowsByLocation(string Location)
        {
            try
            {
                string token = (Request.Headers)["Authorization"].ToString().Split(" ")[1];
                var result = await _showService.GetShowsByLocation(token, Location);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("get-shows-by-type")]
        [AllowAnonymous]
        public async Task<IActionResult> GetShowsByType(string ShowType)
        {
            try
            {
                if (!IsShowTypeValid(ShowType)) throw new ArgumentException("INVALID SHOW TYPE");

                var result = await _showService.GetShowsByType(ShowType);
                result = result.Where(x => x.Status.Equals(Commons.PUBLIC)).ToList();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("get-shows-created-by-staff")]
        [Authorize(Roles = "Staff")]
        public async Task<IActionResult> GetShowsCreatedByStaff()
        {
            try
            {
                string token = (Request.Headers)["Authorization"].ToString().Split(" ")[1];

                var result = await _showService.GetShowsCreatedByStaff(token);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("get-show-detail-by-id")]
        [AllowAnonymous]
        public async Task<IActionResult> GetShowDetailByID(Guid showId, Guid? campaignId)
        {
            try
            {
                var result = await _showService.GetShowDetailById(showId, campaignId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("get-show-detail-by-show-id-for-mobile")]
        [AllowAnonymous]
        public async Task<IActionResult> GetShowDetailByIDForMobile(Guid showId)
        {
            try
            {
                var result = await _showService.GetShowDetailByIdForMobile(showId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("get-sale-stage-by-show-id-for-mobile")]
        [AllowAnonymous]
        public async Task<IActionResult> GetSaleStageByIDForMobile(Guid showId)
        {
            try
            {
                var result = await _showService.GetSaleStageDetailsByIdForMobile(showId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("get-campaign-by-show-id-and-campaign-id-for-mobile")]
        [AllowAnonymous]
        public async Task<IActionResult> GetCampaignByShowIDAndCampaignIDForMobile(Guid showId, Guid? campaignId)
        {
            try
            {
                var result = await _showService.GetCampaignDetailsByIdForMobile(showId, campaignId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("get-show-detail-by-id-by-manager")]
        [SwaggerOperation(Summary = "Get show detail for admin, organizer, staff, artist role")]
        [Authorize(Roles = "Admin,Artist,Staff,Organizer")]
        public async Task<IActionResult> GetShowDetailByIDByMananger(Guid showId)
        {
            try
            {
                string token = (Request.Headers)["Authorization"].ToString().Split(" ")[1];
                var result = await _showService.GetShowDetailByIdByManager(token, showId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("request-show")]
        [Authorize(Roles = "Staff")]
        public async Task<IActionResult> RequestShow(ShowRequestModel request)
        {

            try
            {
                string token = (Request.Headers)["Authorization"].ToString().Split(" ")[1];

                var reqEntity = await _showRequestService.RequestShow(token, request);
                if (reqEntity != null)
                {
                    JsonSerializerSettings jsonSerializerSettings = new();
                    jsonSerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                    var reqJson = JsonConvert.SerializeObject(reqEntity, Formatting.Indented,
                        jsonSerializerSettings);
                    await _hubContext.Clients.All.SendAsync("ReceiveRequest", reqJson);
                }

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return Ok(request);
        }

        [HttpPost("approve-show")]
        [Authorize(Roles = "Organizer")]
        public async Task<IActionResult> ApproveShow(ShowRequestedResponseModel request)
        {
            try
            {
                string token = (Request.Headers)["Authorization"].ToString().Split(" ")[1];

                var isAprpovedSuccess = await _showRequestService.ApproveShow(token, request);

                if (isAprpovedSuccess)
                {

                    var isGenerated = await _showService.GenerateCampaignBookingLink(token, request.ShowId);

                    if (isGenerated)
                    {
                        var e = await _showService.GetShowDetailByIdByManager(token, request.ShowId);
                        var organizer = await _userService.GetUserProfileByToken(token);
        
                        foreach(var camp in e.CampaignViews)
                        {
                            var artistEmail = await _userService.GetEmailById(camp.ArtistId);
                            var isSent = await _emailService.SendArtistIsAddedToShowNotification(artistEmail, e.ShowName, organizer.FullName);
                        }
                        
                    }
                    

                    var requestEntity = await _showRequestService.GetByID(request.RequestID);

                    requestEntity.State = Commons.APPROVE;

                    JsonSerializerSettings jsonSerializerSettings = new()
                    {
                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                    };

                    var reqJson = JsonConvert.SerializeObject(requestEntity, Formatting.Indented,
                        jsonSerializerSettings);
                    await _hubContext.Clients.All.SendAsync("Receive RequestReponseData", reqJson);
                }

            }
            catch (ArgumentException argEx)
            {
                return StatusCode(400, argEx.Message + "\n" + argEx.InnerException);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return Ok(request);
        }

        [HttpPost("reject-show")]
        [Authorize(Roles = "Organizer")]
        public async Task<IActionResult> RejectShow(ShowRequestedResponseModel request)
        {

            try
            {
                string token = (Request.Headers)["Authorization"].ToString().Split(" ")[1];

                var isRejectedSuccess = await _showRequestService.RejectShow(token, request);

                if (isRejectedSuccess)
                {
                    var requestEntity = await _showRequestService.GetByID(request.RequestID);
                    requestEntity.State = Commons.REJECT;
                    JsonSerializerSettings jsonSerializerSettings = new()
                    {
                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                    };
                    var reqJson = JsonConvert.SerializeObject(requestEntity, Formatting.Indented,
                        jsonSerializerSettings);
                    await _hubContext.Clients.All.SendAsync("Receive RequestReponseData", reqJson);
                }

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return Ok(request);
        }

        [HttpGet("get-show-requests")]
        [Authorize(Roles = "Organizer")]
        public async Task<IActionResult> GetShowRequests()
        {
            try
            {
                string token = (Request.Headers)["Authorization"].ToString().Split(" ")[1];

                var result = await _showRequestService.GetShowRequests(token);

                result = result.OrderByDescending(x => x.RequestDate).ToList();

                return Ok(result);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        private static bool IsShowTypeValid(string showType)
        {
            foreach (var type in Commons.SHOWTYPES)
            {
                if (showType.ToLower().Trim().Equals(type.ToLower().Trim()))
                {
                    return true;
                }
            }
            return false;
        }

        private ShowPagingViewModel PagingShows(List<ShowViewModel> listShow, int page)
        {
            var pageResults = 10f;
            var pageCount = Math.Ceiling(listShow.Count() / pageResults);

            if (page > pageCount) throw new ArgumentException("Page is higher than number of pages");

            var showsResult = listShow.Skip((page - 1) * (int)pageResults).Take((int)pageResults).ToList();

            ShowPagingViewModel reponse = new()
            {
                Shows = showsResult,
                CurrentPage = page,
                PageCount = (int)pageCount
            };

            return reponse;
        }
        [HttpGet("get-show-category")]
        [AllowAnonymous]
        public async Task<IActionResult> GetShowCategoryIndoor(Guid showTypeID)
        {

            try
            {
                var result = await _showCategoryService.GetShowCategory(showTypeID);
                return Ok(result);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("get-all-show-categories")]
        [AllowAnonymous]
        public async Task<IActionResult> GetShowCategories()
        {

            try
            {
                var result = await _showCategoryService.GetShowCategories();
                return Ok(result);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("get-shows")]
        [AllowAnonymous]
        public async Task<IActionResult> GetShows()
        {
            try
            {
                List<ShowViewModel> result = new();

                result = await _showService.GetShows();



                return (result != null) ? Ok(result) : StatusCode(500, "Internal Server Exception");
            }
            catch (ArgumentException ex)
            {
                return StatusCode(500, ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("review-show")]
        [SwaggerOperation(Summary = "Review a show for customer")]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> ReviewShow([FromBody] ShowReviewInsertRequestModel request)
        {
            try
            {
                string token = (Request.Headers)["Authorization"].ToString().Split(" ")[1];
                var result = await _showService.AddShowReview(token, request);
                return Ok(result);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (SqlException)
            {
                return StatusCode(500, "Internal Server Exception");
            }
            catch (DbUpdateException)
            {
                return StatusCode(500, "Internal Server Exception");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("get-show-reviews-for-admin")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetShowReviewsForAdmin()
        {
            try
            {
                string token = (Request.Headers)["Authorization"].ToString().Split(" ")[1];
                var result = await _showService.GetShowReviewsForAdmin(token);

                if (result == null) result = new();

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("get-show-reviews-by-show-id")]
        [AllowAnonymous]
        public async Task<IActionResult> GetShowReviews(Guid showId)
        {
            try
            {
                var result = await _showService.GetShowReviews();
                result = result.Where(x => x.ShowId.Equals(showId)).ToList();
                result = result.OrderByDescending(x => x.DateTimeReview).ToList();
                if (result == null) result = new();

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("get-show-reviews-overview-by-show-id")]
        [Authorize(Roles = "Organizer,Admin")]
        public async Task<IActionResult> GetShowReviewsByOrganizer(Guid showId)
        {
            try
            {
                string token = (Request.Headers)["Authorization"].ToString().Split(" ")[1];
                var result = await _showService.GetShowReviewsForOrganizer(token, showId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("get-show-reviews-by-show-id-by-user-id")]
        [AllowAnonymous]
        public async Task<IActionResult> GetShowReviewsForUser(Guid showId)
        {
            try
            {
                string token = (Request.Headers)["Authorization"].ToString().Split(" ")[1];
                var userId = _decodeToken.DecodeID(token, Commons.JWTClaimID);
                var result = await _showService.GetShowReviews();
                result = result.Where(x => x.ShowId.Equals(showId)).ToList();
                result = result.Where(x => x.ReviewerId.Equals(userId)).ToList();
                if (result == null) result = new();

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("get-show-reviews-for-organizer")]
        [Authorize(Roles = "Organizer")]
        public async Task<IActionResult> GetShowReviewsForOrganizer()
        {
            try
            {
                string token = (Request.Headers)["Authorization"].ToString().Split(" ")[1];

                var result = await _showService.GetShowReviewsForOrganizer(token);

                if (result == null) result = new();

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("get-show-reviews-for-organizer-by-show-id")]
        [Authorize(Roles = "Organizer")]
        public async Task<IActionResult> GetShowReviewsForOrganizerByShowId(Guid showId)
        {
            try
            {
                string token = (Request.Headers)["Authorization"].ToString().Split(" ")[1];

                var result = await _showService.GetShowReviewsForOrganizer(token);

                result = result.Where(x => x.ShowId.Equals(showId)).ToList();

                if (result == null) result = new();

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("get-ticket-types-by-show-id")]
        [SwaggerOperation(Summary = "Get show tickettypes by ShowID for organizer, staff, artist")]
        [Authorize(Roles = "Staff,Organizer,Artist")]
        public async Task<IActionResult> GetTicketTypes(Guid showId)
        {
            try
            {
                string token = (Request.Headers)["Authorization"].ToString().Split(" ")[1];

                var result = await _showService.GetTicketTypesByShowId(token, showId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("get-sale-stages-by-show-id")]
        [SwaggerOperation(Summary = "Get show salestages by ShowID for organizer, staff, artist")]
        [Authorize(Roles = "Staff,Organizer,Artist")]
        public async Task<IActionResult> GetSaleStages(Guid showId)
        {
            try
            {
                string token = (Request.Headers)["Authorization"].ToString().Split(" ")[1];

                var result = await _showService.GetSaleStagesByShowId(token, showId);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("get-campaigns-by-show-id")]
        [SwaggerOperation(Summary = "Get show campaigns by ShowID for organizer, staff, artist")]
        [Authorize(Roles = "Staff,Organizer,Artist")]
        public async Task<IActionResult> GetCampaigns(Guid showId)
        {
            try
            {
                string token = (Request.Headers)["Authorization"].ToString().Split(" ")[1];

                var result = await _showService.GetCampaignByShowId(token, showId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /*
        [HttpPost("test-dynamic-link")]
        [AllowAnonymous]
        public async Task<IActionResult> TestDynamicLink(String link)
        {
            try
            {
                var client = new HttpClient();
                Uri uri = new("https://firebasedynamiclinks.googleapis.com/v1/shortLinks?key=AIzaSyCopthNKjHMIgHQBGwqYJNxcbVux2tGcCk");
                DynamicLinkRequestModel dynamicLinkRequestModel = new();

                dynamicLinkRequestModel.dynamicLinkInfo.domainUriPrefix = "https://ultratix.page.link";
                dynamicLinkRequestModel.dynamicLinkInfo.link = link;
                dynamicLinkRequestModel.dynamicLinkInfo.androidInfo.androidPackageName = "com.example.ultratix_mobile_v03";
                dynamicLinkRequestModel.dynamicLinkInfo.iosInfo.iosBundleId = "com.ultratix.ultratixMobile";

                JsonSerializerSettings jsonSerializerSettings = new()
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                };

                var reqJson = JsonConvert.SerializeObject(dynamicLinkRequestModel, Formatting.Indented,
                    jsonSerializerSettings);

                var content = new StringContent(reqJson, Encoding.UTF8);

                var request = new HttpRequestMessage(HttpMethod.Post, uri);
                request.Headers.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                request.Content = content;
                request.RequestUri = uri;

                HttpResponseMessage response = await client.SendAsync(request);
                response.EnsureSuccessStatusCode();
                var responseContent = await response.Content.ReadAsStringAsync();
                var resJson = JsonConvert.DeserializeObject<DynamicLinkReponseModel>(responseContent) ?? new();

                var dynamicLink = resJson.shortLink ?? string.Empty;

                return Ok(dynamicLink);
            }catch(Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        */
    }
}
