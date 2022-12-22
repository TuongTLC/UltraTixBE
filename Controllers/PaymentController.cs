using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UltraTix2022.API.UltraTix2022.Business.Services.EmailService;
using UltraTix2022.API.UltraTix2022.Business.Services.PaymentService;
using UltraTix2022.API.UltraTix2022.Business.Services.ShowService;
using UltraTix2022.API.UltraTix2022.Business.Services.UserService;
using UltraTix2022.API.UltraTix2022.Data.Models.RequestModel.Entities.MoMo;
using UltraTix2022.API.UltraTix2022.Data.Models.RequestModel.Entities.ShowOrder;

namespace UltraTix2022.API.Controllers
{
    [Route("api/v1/payment")]
    [ApiController]
    [ApiVersion("1.0")]
    [Authorize]
    public class PaymentController : ControllerBase
    {
        private readonly IMoMoService _moMoService;
        private readonly IEmailService _emailSerivce;
        private readonly IShowService _showService;
        private readonly IUserService _userService;


        public PaymentController(IMoMoService moMoServices,
            IEmailService emailSerivce,
            IShowService showService,
            IUserService userService)
        {
            _moMoService = moMoServices;
            _emailSerivce = emailSerivce;
            _showService = showService;
            _userService = userService;
        }

        [HttpPost("create-payment")]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> CreatePayment(ShowOrderRequestModel orderModel)
        {
            try
            {
                string token = (Request.Headers)["Authorization"].ToString().Split(" ")[1];

                if(!await _showService.IsTicketsBookedTempClose(orderModel)) {
                    return BadRequest();
                }
                var result = await _moMoService.CreatePayment(token, orderModel);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }

        }

        [HttpPost("check-order-available-mobile")]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> CheckOrderAvailableMobile(ShowOrderRequestModel orderModel)
        {
            try
            {
                string token = (Request.Headers)["Authorization"].ToString().Split(" ")[1];

                if (!await _showService.IsTicketsBookedTempClose(orderModel))
                {
                    return BadRequest();
                }
                return Ok(true);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }

        }

        [HttpPost("re-open-order-mobile")]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> ReOpenOrderMobile(ShowOrderRequestModel orderModel)
        {
            try
            {
                string token = (Request.Headers)["Authorization"].ToString().Split(" ")[1];

                if (!await _showService.ReOpenBookingOrder(orderModel))
                {
                    return BadRequest();
                }
                return Ok(true);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }

        }

        [HttpPost("response-payment")]
        [AllowAnonymous]
        public async Task<IActionResult> ResponsePayment(MoMoResponseModel moMoResponseModel)
        {
            try
            {
                //Get Extra Data -> store to DB
                var orderModel = await _moMoService.ReceiveResponse(moMoResponseModel);

                if (orderModel != null)
                {
                    if(moMoResponseModel.resultCode != 0)
                    {
                        await _showService.ReOpenBookingOrder(orderModel);
                        return NoContent();
                    }
                    
                    int totalTicketBuys = 0;

                    var show = await _showService.GetShowDetailByIdForThirdParty(orderModel.ShowId);

                    string tickets = "<br/><br/>Các loại vé đã mua: <br/><hr>";

                    foreach (var item in orderModel.ShowOrderDetail)
                    {
                        var ticketName = "Loại vé: ";
                        var ticketTypeDetail = show.TicketTypeViews.Where(x => x.Id.Equals(item.TicketTypeId)).FirstOrDefault();
                        if (ticketTypeDetail != null)
                        {
                            ticketName += ticketTypeDetail.TicketTypeName;
                        }
                        totalTicketBuys += item.QuantityBuy;
                        tickets += ticketName + "<br/> Số lượng: " + item.QuantityBuy + "" + "<br/> Tạm tính: " + item.SubTotal + "" + " VND" + "<br/><hr>";
                    }

                    string totalQuantity = totalTicketBuys + "";

                    var totalPay = orderModel.TotalPay + "";

                    var email = await _userService.GetEmailById(orderModel.CustomerId);

                    await _emailSerivce.SendNewOrderNotification(email, show.ShowName, totalQuantity, tickets, totalPay);
                }
                return NoContent();
            }
            catch (Exception ex)
            {
                Console.Write(ex.ToString());
                throw new ArgumentException(ex.Message);

            }

        }

        [HttpPost("response-payment-mobile")]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> ResponsePaymentMobile(ShowOrderRequestInsertModel orderResponseModel)
        {
            try
            {
                string token = (Request.Headers)["Authorization"].ToString().Split(" ")[1];
                //Get Extra Data -> store to DB
                var orderModel = await _moMoService.ReceiveResponseMobile(orderResponseModel, token);

                if (orderModel != null)
                {
                    if(orderResponseModel.ResultCode != 0)
                    {
                        await _showService.ReOpenBookingOrder(orderModel);
                        return NoContent();
                    }
                    int totalTicketBuys = 0;

                    var show = await _showService.GetShowDetailByIdForThirdParty(orderModel.ShowId);

                    string tickets = "<br/><br/>Các loại vé đã mua: <br/><hr>";

                    foreach (var item in orderModel.ShowOrderDetail)
                    {
                        if (item.QuantityBuy > 0)
                        {
                            var ticketName = "Loại vé: ";
                            var ticketTypeDetail = show.TicketTypeViews.Where(x => x.Id.Equals(item.TicketTypeId)).FirstOrDefault();
                            if (ticketTypeDetail != null)
                            {
                                ticketName += ticketTypeDetail.TicketTypeName;
                            }
                            totalTicketBuys += item.QuantityBuy;
                            tickets += ticketName + "<br/> Số lượng: " + item.QuantityBuy + "" + "<br/> Tạm tính: " + item.SubTotal + "" + " VND" + "<br/><hr>";
                        }
                    }

                    string totalQuantity = totalTicketBuys + "";

                    var totalPay = orderModel.TotalPay + "";

                    var email = await _userService.GetEmailById(orderModel.CustomerId);

                    await _emailSerivce.SendNewOrderNotification(email, show.ShowName, totalQuantity, tickets, totalPay);
                }
                return NoContent();
            }
            catch (Exception ex)
            {
                Console.Write(ex.ToString());
                throw new ArgumentException(ex.Message);

            }

        }
    }
}
