using DATN.Data.Entities;
using DATN.ViewModels.Common;
using DATN.ViewModels.DTOs.Authenticate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace DATN.Aplication.Extentions
{
    public class MailExtention
    {
        public async Task<string> SendMailAccountStaffAsync(string userMail, UserLoginView userLogin)
        {
            try
            {
                MailAddress mailFrom = new MailAddress("shoppet79@gmail.com", "MewShop");
                MailAddress mailTo = new MailAddress(userMail);
                MailMessage message = new MailMessage(mailFrom, mailTo);
                message.Subject = "Tài khoản đăng nhập của bạn";
                message.Body = $"<body style=\"font-family: Arial, sans-serif;\">\r\n\r\n    <h2>Tài khoản đăng nhập cá nhân vui lòng không để lộ!!</h2>\r\n\r\n    <p>Cuộc sống vốn có rất nhiều lựa chọn, cảm ơn bạn vì đã chọn làm việc với chúng tôi.<br> Chúc bạn đồng hành vui vẻ cùng MewShop.\n Dưới đây là thông tin tài khoản của bạn:</p>\r\n\r\n Tài khoản:  <p style=\"font-size: 20px; font-weight: bold; color: #007BFF;\">[{userLogin.UserName}]</p>\r\n\r\n Mật khẩu: <p style=\"font-size: 20px; font-weight: bold; color: #007BFF;\">[{userLogin.Password}]</p>\r\n\r\n    <b>Vui lòng không chia sẻ thông tin này với người khác.\n Sau khi đăng nhập thành công để bảo mật thông tin và tài khoản cá nhân của bạn, vui lòng thay đổi mật khẩu cho tài khoản của bạn.</b>\r\n    <p>Nếu bạn có bất kỳ thắc mắc hãy liên hệ qua: <b>shoppet79@gmail.com</b>, <br>\r\n    Hoặc liên hệ qua fanpage của chúng tôi [Địa chỉ fanpage]</p>\r\n    <p>Trân trọng,<br>\r\n    MewShop</p>\r\n</body>";
                message.IsBodyHtml = true;
                message.Priority = MailPriority.High;
                SmtpClient client = new SmtpClient();
                client.Host = "smtp.gmail.com";
                client.Port = 587;
                client.EnableSsl = true;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.UseDefaultCredentials = false;
                client.Credentials = new NetworkCredential("shoppet79@gmail.com", "tznx twfq hclm ysok");
                await client.SendMailAsync(message);
                return $"Mã xác thực đã gửi tới email của bạn!!";
            }
            catch (Exception e)
            {

                return e.Message;
            }
        }
        public async Task<ResponseMail> SendMailCodeForgot(string userMail, string code)
        {
            try
            {
                MailAddress mailFrom = new MailAddress("shoppet79@gmail.com", "MewShop");
                MailAddress mailTo = new MailAddress(userMail);
                MailMessage message = new MailMessage(mailFrom, mailTo);
                message.Subject = "Mã xác nhận của bạn";
                message.Body = $"<body style=\"font-family: Arial, sans-serif;\">\r\n\r\n    <h2>Đây là mail cung cấp mã xác nhận khi người dùng quên mật khẩu</h2>\r\n\r\n    <p>Cuộc sống vốn có rất nhiều lựa chọn, cảm ơn bạn vì đã chọn luôn tin tưởng chúng tôi.<br> Cảm ơn bạn đã đồng hành trong suốt thời gian qua. Dưới đây là mã xác nhận của bạn:</p>\r\n\r\n    <p style=\"font-size: 20px; font-weight: bold; color: #007BFF;\">[{code}]</p><b>Chỉ có hiệu lực trong vòng 5 phút</b>\r\n\r\n <br>   <b>Vui lòng không chia sẻ mã xác nhận này với người khác</b>\r\n    <p>Nếu bạn có bất kỳ thắc mắc hãy liên hệ qua: <b>shoppet79@gmail.com</b>, <br>\r\n    Hoặc liên hệ qua fanpage của chúng tôi [Địa chỉ fanpage]</p>\r\n    <p>Trân trọng,<br>\r\n    MeowShop</p>\r\n</body>";
                message.IsBodyHtml = true;
                message.Priority = MailPriority.High;
                SmtpClient client = new SmtpClient();
                client.Host = "smtp.gmail.com";
                client.Port = 587;
                client.EnableSsl = true;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.UseDefaultCredentials = false;
                client.Credentials = new NetworkCredential("shoppet79@gmail.com", "tznx twfq hclm ysok");
                await client.SendMailAsync(message);
                return new ResponseMail { IsSuccess = true, Notifications = "Mã xác nhận đã được gửi tới mail của bạn!!" };
            }
            catch (Exception e)
            {

                return new ResponseMail { IsSuccess = true, Notifications = "Mã xác nhận chưa được gửi đi!!", Error = e.Message };
            }
        }
        public async Task<string> SendMailVerificationAsync(string userMail,string user, string pass, string Verification)
        {
            try
            {
                MailAddress mailFrom = new MailAddress("shoppet79@gmail.com", "MewShop");
                MailAddress mailTo = new MailAddress(userMail);
                MailMessage message = new MailMessage(mailFrom, mailTo);
                message.Subject = "Tài khoản đăng nhập của bạn";
                string verificationLink = $"https://localhost:7039/api/GuestManager/verify-user?verifyConstring={Verification}&mail={userMail}";
                message.Body = $@"
                <body style=""font-family: Arial, sans-serif;"">
                    <h2>Tài khoản đăng nhập cá nhân vui lòng không để lộ!!</h2>
                    <p>Cuộc sống vốn có rất nhiều lựa chọn, cảm ơn bạn vì đã chọn làm việc với chúng tôi.<br> Chúc bạn đồng hành vui vẻ cùng MewShop.<br> Dưới đây là thông tin tài khoản của bạn:</p>
                    <p>Tài khoản: <span style=""font-size: 20px; font-weight: bold; color: #007BFF;"">{HttpUtility.HtmlEncode(user)}</span></p>
                    <p>Mật khẩu: <span style=""font-size: 20px; font-weight: bold; color: #007BFF;"">{HttpUtility.HtmlEncode(pass)}</span></p>
                    <p>Vui lòng <a href=""{HttpUtility.HtmlEncode(verificationLink)}"" style=""color: #007BFF; text-decoration: none;"">nhấn vào đây</a> để xác minh tài khoản của bạn.</p>
                    <b>Vui lòng không chia sẻ thông tin này với người khác.<br> Sau khi đăng nhập thành công để bảo mật thông tin và tài khoản cá nhân của bạn, vui lòng thay đổi mật khẩu cho tài khoản của bạn.</b>
                    <p>Nếu bạn có bất kỳ thắc mắc hãy liên hệ qua: <b>shoppet79@gmail.com</b>,<br> Hoặc liên hệ qua fanpage của chúng tôi [Địa chỉ fanpage]</p>
                    <p>Trân trọng,<br> MewShop</p>
                </body>";

                message.IsBodyHtml = true;
                message.Priority = MailPriority.High;
                SmtpClient client = new SmtpClient();
                client.Host = "smtp.gmail.com";
                client.Port = 587;
                client.EnableSsl = true;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.UseDefaultCredentials = false;
                client.Credentials = new NetworkCredential("shoppet79@gmail.com", "tznx twfq hclm ysok");
                await client.SendMailAsync(message);
                return $"Mã xác thực đã gửi tới email của bạn!!";
            }
            catch (Exception e)
            {

                return e.Message;
            }
        }
    }
}
