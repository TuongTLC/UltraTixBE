namespace UltraTix2022.API.UltraTix2022.Business.Services.EmailService
{
    public interface IEmailService
    {
        public Task<string> SendEmailVerificationOTP(string email, string OTP);
        public Task<string> SendNewOrderNotification(string email, string showName, string totalTicketBuys, string tickets, string totalPay);
        public Task<string> SendArtistIsAddedToShowNotification(string email, string showName, string organizerName);
    }
}
