using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using UltraTix2022.API.UltraTix2022.Data.Repositories.ImplementedRepositories.TickTypeRepo;

namespace UltraTix2022.API.UltraTix2022.Business.Services.EmailService
{
    public class EmailService : IEmailService
    {
        private readonly ITicketTypeRepo _ticketTypeRepo;


        public EmailService(ITicketTypeRepo ticketTypeRepo)
        {
            _ticketTypeRepo = ticketTypeRepo;
        }

        public async Task<string> SendEmailVerificationOTP(string email, string OTP)
        {
            string from = "lilo190516@gmail.com";
            string password = "hlizgiggrjgtxakn";

            MimeMessage message = new();
            message.From.Add(MailboxAddress.Parse(from));
            message.Subject = "UltraTix - UltraTix account email verification code";
            message.To.Add(MailboxAddress.Parse(email));
            message.Body = new TextPart(MimeKit.Text.TextFormat.Html) { Text = "<html><body> Your code is: " + OTP + "</body></html>" };

            using var smtp = new SmtpClient();
            await smtp.ConnectAsync("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
            await smtp.AuthenticateAsync(from, password);
            await smtp.SendAsync(message);
            await smtp.DisconnectAsync(true);

            return OTP;
        }

        public async Task<string> SendNewOrderNotification(string email, string showName, string totalTicketBuys, string tickets, string totalPay)
        {
            string from = "lilo190516@gmail.com";
            string password = "hlizgiggrjgtxakn";

            MimeMessage message = new();
            message.From.Add(MailboxAddress.Parse(from));
            message.Subject = "UltraTix - Thông báo xác nhận đơn hàng";
            message.To.Add(MailboxAddress.Parse(email));
            message.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text =
                "<html>" +
                "<body>" +
                "<h1>UltraTix<h1><br/>" +
                "<h3>Cảm ơn quý khách đã ủng hộ UltraTix.Net </h3>" +
                "<p>Xin chào, UltraTix rất vui đã nhận được sự ủng hộ của quý khách.</p>" +
                "<p>Chúng tôi xin gửi thông tin đơn hàng cho quý khách:</p>" +
                "<p>Sự kiện: " + showName + "</p>" +
                "<p>Chi tiết đơn hàng: " + tickets + "</p>" +
                "<p>Tổng số lượng vé đã mua: " + totalTicketBuys + "</p>" +
                "<p>Tổng tiền thanh toán: " + totalPay + " VND</p>" +
                "</body>" +
                "</html>"
            };

            using var smtp = new SmtpClient();
            await smtp.ConnectAsync("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
            await smtp.AuthenticateAsync(from, password);
            await smtp.SendAsync(message);
            await smtp.DisconnectAsync(true);

            return email;
        }

        public async Task<string> SendArtistIsAddedToShowNotification(string email, string showName, string organizerName)
        {
            string from = "lilo190516@gmail.com";
            string password = "hlizgiggrjgtxakn";

            MimeMessage message = new();
            message.From.Add(MailboxAddress.Parse(from));
            message.Subject = "UltraTix - Thông báo bạn đã được thêm vào sự kiện "+showName;
            message.To.Add(MailboxAddress.Parse(email));
            message.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text =
                "<html>" +
                "<body>" +
                "<h1>UltraTix<h1><br/>" +
                "<h3>Bạn đã được thêm vào sự kiện "+showName+" với tư cách nghệ sĩ</h3>" +
                "<p>Xin chào, UltraTix xin thông báo bạn đã được nhà tổ chức "+organizerName+" thêm vào sự kiện "+showName+" với tư cách là nghệ sĩ.</p>" +
                "<p>Bạn hãy vào trang UltraTix.net để xem thêm thông tin chi tiết về sự kiện.</p>" +
                "<p>Nếu bạn nhận thấy có bất kỳ sai sót nào hoặc thông tin hiển thị không chính xác, <br/>bạn hãy liên lạc với chúng tôi qua địa chỉ email này hoặc qua số điện thoại 0866058578 - Ultratix, xin cảm ơn !</p>" +
                "<p>Xin chân thành cảm ơn !</p>" +
                "<p>UltraTix</p>" +
                "</body>" +
                "</html>"
            };

            using var smtp = new SmtpClient();
            await smtp.ConnectAsync("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
            await smtp.AuthenticateAsync(from, password);
            await smtp.SendAsync(message);
            await smtp.DisconnectAsync(true);

            return email;
        }
    }
}
