using UltraTix2022.API.UltraTix2022.Data.Models.RequestModel.Entities.MoMo;
using UltraTix2022.API.UltraTix2022.Data.Models.RequestModel.Entities.ShowOrder;

namespace UltraTix2022.API.UltraTix2022.Business.Services.PaymentService
{
    public interface IMoMoService
    {
        public Task<string> CreatePayment(string token, ShowOrderRequestModel orderModel);
        public Task<ShowOrderRequestInsertModel?> ReceiveResponse(MoMoResponseModel moMoResponseModel);
        public Task<ShowOrderRequestInsertModel?> ReceiveResponseMobile(ShowOrderRequestInsertModel orderModel, string token);
    }
}
