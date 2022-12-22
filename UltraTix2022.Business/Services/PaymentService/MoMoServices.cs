using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UltraTix2022.API.UltraTix2022.Business.Services.EmailService;
using UltraTix2022.API.UltraTix2022.Business.Services.SecretServices;
using UltraTix2022.API.UltraTix2022.Business.Utilities;
using UltraTix2022.API.UltraTix2022.Data.Models.Commons;
using UltraTix2022.API.UltraTix2022.Data.Models.Entities;
using UltraTix2022.API.UltraTix2022.Data.Models.RequestModel.Entities.MoMo;
using UltraTix2022.API.UltraTix2022.Data.Models.RequestModel.Entities.ShowOrder;
using UltraTix2022.API.UltraTix2022.Data.Repositories.ImplementedRepositories.AppTransactionRepo;
using UltraTix2022.API.UltraTix2022.Data.Repositories.ImplementedRepositories.CampaignDetailRepo;
using UltraTix2022.API.UltraTix2022.Data.Repositories.ImplementedRepositories.CampaignRepo;
using UltraTix2022.API.UltraTix2022.Data.Repositories.ImplementedRepositories.CustomerPurchaseTransactionRepo;
using UltraTix2022.API.UltraTix2022.Data.Repositories.ImplementedRepositories.CustomerWalletRepo;
using UltraTix2022.API.UltraTix2022.Data.Repositories.ImplementedRepositories.MoMoRepo;
using UltraTix2022.API.UltraTix2022.Data.Repositories.ImplementedRepositories.SaleStageDetailRepo;
using UltraTix2022.API.UltraTix2022.Data.Repositories.ImplementedRepositories.ShowOrderDetailRepo;
using UltraTix2022.API.UltraTix2022.Data.Repositories.ImplementedRepositories.ShowOrderRepo;
using UltraTix2022.API.UltraTix2022.Data.Repositories.ImplementedRepositories.ShowRepo;
using UltraTix2022.API.UltraTix2022.Data.Repositories.ImplementedRepositories.ShowTransactionHistoryRepo;
using UltraTix2022.API.UltraTix2022.Data.Repositories.ImplementedRepositories.TickTypeRepo;
using UltraTix2022.API.UltraTix2022.Data.Repositories.ImplementedRepositories.UserRepo;

namespace UltraTix2022.API.UltraTix2022.Business.Services.PaymentService
{
    public class MoMoServices : IMoMoService
    {
        private readonly IMoMoRepo _moMoRepo;
        private readonly IShowOrderRepo _showOrderRepo;
        private readonly IShowRepo _showRepo;
        private readonly IShowOrderDetailRepo _showOrderDetailRepo;
        private readonly ITicketTypeRepo _ticketTypeRepo;
        private readonly ICampaignRepo _campaignRepo;
        private readonly ICampaignDetailRepo _campaignDetailRepo;
        private readonly ISaleStageDetailRepo _saleStageDetailRepo;
        private readonly IShowTransactionHistoryRepo _showTransactionHistoryRepo;
        private readonly IAppTransactionRepo _appTransactionRepo;
        private readonly ICustomerPurchaseTransactionRepo _customerPurchaseTransactionRepo;
        private readonly ICustomerWalletRepo _customerWalletRepo;
        private readonly DecodeToken _decodeToken;
        private readonly IEmailService _emailService;
        private readonly IUserRepo _userRepo;


        public MoMoServices(
            IMoMoRepo moMoRepo, IShowOrderRepo showOrderRepo,
            IShowOrderDetailRepo showOrderDetailRepo, ITicketTypeRepo ticketTypeRepo,
            ICampaignRepo campaignRepo, ICampaignDetailRepo campaignDetailRepo,
            ISaleStageDetailRepo saleStageDetailRepo,
            IShowTransactionHistoryRepo showTransactionHistoryRepo,
            IAppTransactionRepo appTransactionRepo,
            ICustomerPurchaseTransactionRepo customerPurchaseTransactionRepo,
            ICustomerWalletRepo customerWalletRepo,
            IShowRepo showRepo,
            IEmailService emailService,
            IUserRepo userRepo
            )
        {
            _moMoRepo = moMoRepo;
            _showOrderRepo = showOrderRepo;
            _showOrderDetailRepo = showOrderDetailRepo;
            _ticketTypeRepo = ticketTypeRepo;
            _campaignRepo = campaignRepo;
            _campaignDetailRepo = campaignDetailRepo;
            _saleStageDetailRepo = saleStageDetailRepo;
            _showTransactionHistoryRepo = showTransactionHistoryRepo;
            _appTransactionRepo = appTransactionRepo;
            _customerPurchaseTransactionRepo = customerPurchaseTransactionRepo;
            _customerWalletRepo = customerWalletRepo;
            _decodeToken = new DecodeToken();
            _showRepo = showRepo;
            _emailService = emailService;
            _userRepo = userRepo;
        }

        public async Task<string> CreatePayment(string token, ShowOrderRequestModel orderModel)
        {

            int roleID = _decodeToken.Decode(token, Commons.JWTClaimRoleID);
            if (!roleID.Equals(Commons.CUSTOMER)) throw new ArgumentException(Commons.ERROR_403_FORBIDDEN_MSG);
            Guid userID = _decodeToken.DecodeID(token, Commons.JWTClaimID);

            ShowOrderRequestInsertModel orderInsertModel = new()
            {
                TotalPay = orderModel.TotalPay,
                CampaignId = orderModel.CampaignId,
                OrderDescription = orderModel.OrderDescription,
                ShowOrderDetail = orderModel.ShowOrderDetail,
                CustomerId = userID,
                ShowId = orderModel.ShowId
            };

            JsonSerializerSettings jsonSerializerSettings = new()
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            };

            var orderJsonStringRaw = JsonConvert.SerializeObject(orderInsertModel, Formatting.Indented,
                jsonSerializerSettings);
            var orderTextBytes = System.Text.Encoding.UTF8.GetBytes(orderJsonStringRaw);
            var base64OrderString = Convert.ToBase64String(orderTextBytes);

            List<string> secrets = KeyVaultServices.GetPaymentSecrets();
            //request params need to request to MoMo system
            string endpoint = "https://test-payment.momo.vn/v2/gateway/api/create";
            string partnerCode = secrets[0];
            string accessKey = secrets[1];
            string serectkey = secrets[2];
            string orderInfo = "test";
            string redirectUrl = "https://ultratix.net/thanks";
            string ipnUrl = "https://ultratixapi.azurewebsites.net/api/v1/payment/response-payment";
            string requestType = "captureWallet";
            string orderId = Guid.NewGuid().ToString();
            string requestId = Guid.NewGuid().ToString();
            string extraData = base64OrderString;

            //String to  object json

            //var base64OrderBytes = Convert.FromBase64String(base64OrderString);
            //var orderJsonString = System.Text.Encoding.UTF8.GetString(base64OrderBytes);
            //var showOrderInsertModel = JsonConvert.DeserializeObject<ShowOrderRequestInsertModel>(orderJsonString);
            var amountPay = 0.0;

            if (orderModel != null)
            {
                amountPay = orderModel.TotalPay;
            }
            //Before sign HMAC SHA256 signature
            string rawHash = "accessKey=" + accessKey +
                "&amount=" + amountPay.ToString() +
                "&extraData=" + extraData +
                "&ipnUrl=" + ipnUrl +
                "&orderId=" + orderId +
                "&orderInfo=" + orderInfo +
                "&partnerCode=" + partnerCode +
                "&redirectUrl=" + redirectUrl +
                "&requestId=" + requestId +
            "&requestType=" + requestType
                ;

            Console.Write("rawHash = " + rawHash);

            string signatureRawHash = "accessKey=" + accessKey +
               "&orderId=" + orderId +
               "&partnerCode=" + partnerCode +
            "&requestId=" + requestId
            ;

            Console.Write("signatureRawHash = " + rawHash);

            MoMoSecurity crypto = new MoMoSecurity();
            //sign signature SHA256
            string signature = crypto.signSHA256(rawHash, serectkey);
            Console.Write("Signature = " + signature);
            string querySignature = crypto.signSHA256(rawHash, serectkey);
            Console.Write("QuerySignature = " + querySignature);

            //build body json request
            JObject message = new JObject
            {
                { "partnerCode", partnerCode },
                { "partnerName", "Test" },
                { "storeId", "MomoTestStore" },
                { "requestId", requestId },
                { "amount", amountPay.ToString() },
                { "orderId", orderId },
                { "orderInfo", orderInfo },
                { "redirectUrl", redirectUrl },
                { "ipnUrl", ipnUrl },
                { "lang", "en" },
                { "extraData", extraData },
                { "requestType", requestType },
                { "signature", signature }

            };
            Console.Write("Json request to MoMo: " + message.ToString());
            try
            {
                string responseFromMomo = await Task.FromResult(PaymentRequest.sendPaymentRequest(endpoint, message.ToString()));
                return responseFromMomo;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<ShowOrderRequestInsertModel?> ReceiveResponse(MoMoResponseModel moMoResponseModel)
        {
            try
            {
                var base64OrderBytes = Convert.FromBase64String(moMoResponseModel.extraData ?? "");
                var orderJson = System.Text.Encoding.UTF8.GetString(base64OrderBytes);
                var orderModel = JsonConvert.DeserializeObject<ShowOrderRequestInsertModel>(orderJson);
                bool isBuyViaLink = false;

                double revenue = 0;
                double artistCommission = 0;
                int totalTickets = 0;

                if (orderModel != null && moMoResponseModel.resultCode == 0)
                {
                    //Add show order and show order detail and update ticket type unit sold to DB
                    ShowOrder orderEntity = new()
                    {
                        Id = Guid.NewGuid(),
                        CustomerId = orderModel.CustomerId,
                        OrderDate = DateTime.Now,
                        TotalPay = orderModel.TotalPay,
                        OrderDescription = orderModel.OrderDescription,
                        ShowId = orderModel.ShowId,
                        IsUsed = false,
                    };

                    if (orderModel.CampaignId != null)
                    {
                        orderEntity.CampaignId = orderModel.CampaignId;
                        isBuyViaLink = true;
                    }
                    else
                    {
                        orderEntity.CampaignId = null;
                        isBuyViaLink = false;
                    }

                    foreach (var orderDetail in orderModel.ShowOrderDetail)
                    {
                        ShowOrderDetail showOrderDetailEntity = new()
                        {
                            Id = Guid.NewGuid(),
                            QuantityBuy = orderDetail.QuantityBuy,
                            ShowOrderId = orderEntity.Id,
                            SubTotal = orderDetail.SubTotal,

                        };

                        if (orderDetail.SaleStageDetailId != null)
                        {
                            showOrderDetailEntity.SaleStageDetailId = orderDetail.SaleStageDetailId;
                        }

                        if (orderDetail.CampaignDetailId != null)
                        {
                            showOrderDetailEntity.CampaignDetailId = orderDetail.CampaignDetailId;
                        }
                        else
                        {
                            orderDetail.CampaignDetailId = null;
                        }

                        orderEntity.ShowOrderDetails.Add(showOrderDetailEntity);

                        //Update unitSold for ticket type in DB                        
                        //var testResult = await _ticketTypeRepo.UpdateUnitSold(orderDetail.TicketTypeId, orderDetail.QuantityBuy, isBuyViaLink);

                        if (orderDetail.SaleStageDetailId != null)
                        {
                            //var testResult2 = await _saleStageDetailRepo.UpdateUnitSold(orderDetail.SaleStageDetailId ?? new Guid(), orderDetail.QuantityBuy, isBuyViaLink);
                        }
                        //Update unitSold for salestageDetail in DB


                        //Update unitSold for campaignDetail if buy via link
                        if (isBuyViaLink)
                        {
                            var campaignDetail = await _campaignDetailRepo.GetCampaignDetailsByCampaignID(orderModel.CampaignId ?? new Guid());
                            var campaignDetailEnti = campaignDetail.Where(x => x.SaleStageDetailId.Equals(orderDetail.SaleStageDetailId)).First();
                            //var testResult3 = await _campaignDetailRepo.UpdateUnitSold(campaignDetailEnti.Id, orderDetail.QuantityBuy);

                            //Cong doanh thu cho ca si neu mua qua link cua ca si
                            artistCommission += orderDetail.SubTotal * (campaignDetailEnti.ArtistDiscount / 100);
                        }

                        // Cong doanh thu cho nha to chuc theo loai ve
                        revenue += orderDetail.SubTotal;

                        // tinh tong so luong ve
                        totalTickets += orderDetail.QuantityBuy;

                    }

                    AppTransaction appTransaction = new()
                    {
                        Id = Guid.NewGuid(),
                        Amount = orderEntity.TotalPay,
                        PaymentId = Guid.Parse("eb6e2009-03a0-4fa9-8405-2e3702e8357c"),
                        TransactionDate = orderEntity.OrderDate,
                        TransactionDescription = orderEntity.OrderDescription,
                    };

                    CustomerPurchaseTransaction customerPurchaseTransaction = new()
                    {
                        Id = appTransaction.Id,
                        Amount = appTransaction.Amount,
                        TransactionDate = appTransaction.TransactionDate,
                        TransactionDescription = appTransaction.TransactionDescription,
                        CustomerWalletId = Guid.NewGuid(), //await _customerWalletRepo.GetCustomerWalletIdByCustomerId(orderEntity.CustomerId),
                        ShowOrderId = orderEntity.Id,
                        SystemWalletId = Guid.NewGuid(),
                    };

                    ShowTransactionHisotry showTransactionHisotry = new()
                    {
                        Id = Guid.NewGuid(),
                        IsBuyViaArtistLink = isBuyViaLink,
                        Amount = appTransaction.Amount,
                        CustomerId = orderEntity.CustomerId,
                        ShowId = orderEntity.ShowId,
                        TotalTicketsBuy = totalTickets,
                        Revenue = revenue,
                        ShowOrderId = orderEntity.Id,
                    };

                    var show = await _showRepo.Get(orderEntity.ShowId);

                    showTransactionHisotry.ShowName = show.ShowName;

                    if (isBuyViaLink)
                    {
                        showTransactionHisotry.ArtistCommission = artistCommission;
                        showTransactionHisotry.CampaignId = orderModel.CampaignId;
                    }

                    //Add show order and show order detail to DB
                    await _showOrderRepo.Insert(orderEntity);
                    await _appTransactionRepo.Insert(appTransaction);
                    await _customerPurchaseTransactionRepo.Insert(customerPurchaseTransaction);
                    await _showTransactionHistoryRepo.Insert(showTransactionHisotry);

                    MoMoResponse momoEntity = new()
                    {
                        Id = Guid.NewGuid(),
                        OrderId = moMoResponseModel.orderId,
                        PartnerCode = moMoResponseModel.partnerCode,
                        RequestId = moMoResponseModel.requestId,
                        Amount = moMoResponseModel.amount.ToString(),
                        OrderInfo = moMoResponseModel.orderInfo,
                        OrderType = moMoResponseModel.orderType,
                        TransId = moMoResponseModel.transId.ToString(),
                        ResultCode = moMoResponseModel.resultCode.ToString(),
                        Message = moMoResponseModel.message,
                        PayType = moMoResponseModel.payType,
                        ResponseTime = moMoResponseModel.responseTime.ToString(),
                        ExtraData = moMoResponseModel.extraData,
                        Signature = moMoResponseModel.signature
                    };

                    await _moMoRepo.Insert(momoEntity);
                }              
                return orderModel;
            }
            catch (Exception e)
            {
                Console.Write(e.ToString());
                throw new ArgumentException(e.Message);
            }
        }

        public async Task<ShowOrderRequestInsertModel?> ReceiveResponseMobile(ShowOrderRequestInsertModel orderModel, string token)
        {
            try
            {
                int roleID = _decodeToken.Decode(token, Commons.JWTClaimRoleID);
                if (!roleID.Equals(Commons.CUSTOMER)) throw new ArgumentException(Commons.ERROR_403_FORBIDDEN_MSG);
                Guid userID = _decodeToken.DecodeID(token, Commons.JWTClaimID);
                //var base64OrderBytes = Convert.FromBase64String(moMoResponseModel.extraData ?? "");
                //var orderJson = System.Text.Encoding.UTF8.GetString(base64OrderBytes);
                //var orderModel = JsonConvert.DeserializeObject<ShowOrderRequestInsertModel>(orderJson);
                bool isBuyViaLink = false;

                double revenue = 0;
                double artistCommission = 0;
                int totalTickets = 0;

                if (orderModel != null && orderModel.ResultCode == 0)
                {
                    //Add show order and show order detail and update ticket type unit sold to DB
                    ShowOrder orderEntity = new()
                    {
                        Id = Guid.NewGuid(),
                        CustomerId = userID,
                        OrderDate = DateTime.Now,
                        TotalPay = orderModel.TotalPay,
                        OrderDescription = orderModel.OrderDescription,
                        ShowId = orderModel.ShowId,
                        IsUsed = false,
                    };

                    orderModel.CustomerId = userID;

                    if (orderModel.CampaignId != null)
                    {
                        orderEntity.CampaignId = orderModel.CampaignId;
                        isBuyViaLink = true;
                    }
                    else
                    {
                        orderEntity.CampaignId = null;
                        isBuyViaLink = false;
                    }

                    foreach (var orderDetail in orderModel.ShowOrderDetail)
                    {
                        if (orderDetail.QuantityBuy > 0 && orderDetail.SubTotal > 0)
                        {
                            ShowOrderDetail showOrderDetailEntity = new()
                            {
                                Id = Guid.NewGuid(),
                                QuantityBuy = orderDetail.QuantityBuy,
                                ShowOrderId = orderEntity.Id,
                                SubTotal = orderDetail.SubTotal,

                            };

                            if (orderDetail.SaleStageDetailId != null)
                            {
                                showOrderDetailEntity.SaleStageDetailId = orderDetail.SaleStageDetailId;
                            }

                            if (orderDetail.CampaignDetailId != null)
                            {
                                showOrderDetailEntity.CampaignDetailId = orderDetail.CampaignDetailId;
                            }
                            else
                            {
                                orderDetail.CampaignDetailId = null;
                            }

                            orderEntity.ShowOrderDetails.Add(showOrderDetailEntity);

                            //Update unitSold for ticket type in DB                        
                            //var testResult = await _ticketTypeRepo.UpdateUnitSold(orderDetail.TicketTypeId, orderDetail.QuantityBuy, isBuyViaLink);

                            if (orderDetail.SaleStageDetailId != null)
                            {
                                //var testResult2 = await _saleStageDetailRepo.UpdateUnitSold(orderDetail.SaleStageDetailId ?? new Guid(), orderDetail.QuantityBuy, isBuyViaLink);
                            }
                            //Update unitSold for salestageDetail in DB


                            //Update unitSold for campaignDetail if buy via link
                            if (isBuyViaLink)
                            {
                                var campaignDetail = await _campaignDetailRepo.GetCampaignDetailsByCampaignID(orderModel.CampaignId ?? new Guid());
                                var campaignDetailEnti = campaignDetail.Where(x => x.SaleStageDetailId.Equals(orderDetail.SaleStageDetailId)).First();
                                //var testResult3 = await _campaignDetailRepo.UpdateUnitSold(campaignDetailEnti.Id, orderDetail.QuantityBuy);

                                //Cong doanh thu cho ca si neu mua qua link cua ca si
                                artistCommission += orderDetail.SubTotal * (campaignDetailEnti.ArtistDiscount / 100);
                            }

                            // Cong doanh thu cho nha to chuc theo loai ve
                            revenue += orderDetail.SubTotal;

                            // tinh tong so luong ve
                            totalTickets += orderDetail.QuantityBuy;
                        }

                    }

                    AppTransaction appTransaction = new()
                    {
                        Id = Guid.NewGuid(),
                        Amount = orderEntity.TotalPay,
                        PaymentId = Guid.Parse("eb6e2009-03a0-4fa9-8405-2e3702e8357c"),
                        TransactionDate = orderEntity.OrderDate,
                        TransactionDescription = orderEntity.OrderDescription,
                    };

                    CustomerPurchaseTransaction customerPurchaseTransaction = new()
                    {
                        Id = appTransaction.Id,
                        Amount = appTransaction.Amount,
                        TransactionDate = appTransaction.TransactionDate,
                        TransactionDescription = appTransaction.TransactionDescription,
                        CustomerWalletId = Guid.NewGuid(), //await _customerWalletRepo.GetCustomerWalletIdByCustomerId(orderEntity.CustomerId),
                        ShowOrderId = orderEntity.Id,
                        SystemWalletId = Guid.NewGuid(),
                    };

                    ShowTransactionHisotry showTransactionHisotry = new()
                    {
                        Id = Guid.NewGuid(),
                        IsBuyViaArtistLink = isBuyViaLink,
                        Amount = appTransaction.Amount,
                        CustomerId = orderEntity.CustomerId,
                        ShowId = orderEntity.ShowId,
                        TotalTicketsBuy = totalTickets,
                        Revenue = revenue,
                        ShowOrderId = orderEntity.Id,
                    };

                    var show = await _showRepo.Get(orderEntity.ShowId);

                    showTransactionHisotry.ShowName = show.ShowName;

                    if (isBuyViaLink)
                    {
                        showTransactionHisotry.ArtistCommission = artistCommission;
                        showTransactionHisotry.CampaignId = orderModel.CampaignId;
                    }

                    //Add show order and show order detail to DB
                    await _showOrderRepo.Insert(orderEntity);
                    await _appTransactionRepo.Insert(appTransaction);
                    await _customerPurchaseTransactionRepo.Insert(customerPurchaseTransaction);
                    await _showTransactionHistoryRepo.Insert(showTransactionHisotry);

                }              
                return orderModel;
            }
            catch (Exception e)
            {
                Console.Write(e.ToString());
                throw new ArgumentException(e.Message);
            }
        }
    }
}
