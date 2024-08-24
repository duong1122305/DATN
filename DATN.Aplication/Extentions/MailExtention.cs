using DATN.ViewModels.Common;
using DATN.ViewModels.DTOs.Authenticate;
using System.Net;
using System.Net.Mail;
using System.Web;

namespace DATN.Aplication.Extentions
{
    public class MailExtention
    {

        private string _hosting = $@"http://localhost:5173/redirect";
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
        public async Task<string> SendMailNotificationAddStaffInShift(List<string> email, DateTime dateWoking, string shiftName)
        {
            try
            {
                foreach (string s in email)
                {
                    MailAddress mailFrom = new MailAddress("shoppet79@gmail.com", "MewShop");
                    MailAddress mailTo = new MailAddress(s);
                    MailMessage message = new MailMessage(mailFrom, mailTo);
                    message.Subject = "Thông báo bạn vừa được thêm ca khẩn cấp";
                    message.Body = $"" +
                        $"<body style=\"font-family: Arial, sans-serif;\">\r\n\r\n" +
                        $"<p>Cuộc sống vốn có rất nhiều lựa chọn, cảm ơn bạn vì đã chọn làm việc với chúng tôi.<br> " +
                        $"Chúc bạn đồng hành vui vẻ cùng MewShop.\n" +
                        $"Dưới đây là thông tin về lịch làm việc được thêm khẩn cấp:</p>\r\n\r\n " +
                        $"<b>Vui lòng liên hệ chủ cửa hàng để đổi ca nếu ngày ca này bạn không thể đi làm.</b>\n " +
                        $"<p>Nếu bạn có bất kỳ thắc mắc hãy liên hệ qua: <b>shoppet79@gmail.com</b>, <br>\r\n    " +
                        $"Hoặc liên hệ qua fanpage của chúng tôi [Địa chỉ fanpage]</p>\r\n    " +
                        $"<p>Trân trọng,<br>\r\n    " +
                        $"MewShop</p>\r\n" +
                        $"</body>";
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
                }
                return $"Mail thông báo thêm ca khẩn cấp đã được gửi đi!!";

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
                string verificationLink = @$"{_hosting}?verifyCode={code}";
                message.Subject = "Yêu cầu đặt lại mật khẩu";
                message.Body = $@"
                        <body style=""font-family: Arial, sans-serif;"">
                            <h2>Yêu cầu đặt lại mật khẩu</h2>
                            <p>Chúng tôi đã nhận được yêu cầu đặt lại mật khẩu cho tài khoản của bạn tại MewShop.<br>
                               Nếu bạn đã yêu cầu đặt lại mật khẩu, vui lòng <a href=""{HttpUtility.HtmlEncode(verificationLink)}"" style=""color: #007BFF; text-decoration: none;"">nhấn vào đây</a> để thay đổi mật khẩu của bạn.</p>
                            <p>Nếu bạn không phải là người yêu cầu, hãy bỏ qua email này và mật khẩu của bạn sẽ không thay đổi.</p>
                            <p>Nếu bạn có bất kỳ thắc mắc nào, hãy liên hệ qua: <b>shoppet79@gmail.com</b>,<br>
                               hoặc liên hệ qua sđt của chúng tôi 0975825324</p>
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
                return new ResponseMail { IsSuccess = true, Notifications = "Mã xác nhận đã được gửi tới mail của bạn!!" };
            }
            catch (Exception e)
            {

                return new ResponseMail { IsSuccess = true, Notifications = "Mã xác nhận chưa được gửi đi!!", Error = e.Message };
            }
        }
        public async Task<ResponseMail> SendCodeForgotOfUser(string userMail, string code)
        {
            try
            {
                MailAddress mailFrom = new MailAddress("shoppet79@gmail.com", "MewShop");
                MailAddress mailTo = new MailAddress(userMail);
                MailMessage message = new MailMessage(mailFrom, mailTo);
                message.Subject = "Yêu cầu đặt lại mật khẩu";
                message.Body = $@"
                        <body style=""font-family: Arial, sans-serif;"">
                            <h2>Yêu cầu đặt lại mật khẩu</h2>
                            <p>Chúng tôi đã nhận được yêu cầu đặt lại mật khẩu cho tài khoản của bạn tại MewShop.<br>
                               Nếu bạn đã yêu cầu đặt lại mật khẩu, đây là mã xác minh của bạn <p style=""color: #007BFF; text-decoration: none;"">{code}</p></p>
                            <p>Nếu bạn không phải là người yêu cầu, hãy bỏ qua email này và mật khẩu của bạn sẽ không thay đổi.</p>
                            <p>Nếu bạn có bất kỳ thắc mắc nào, hãy liên hệ qua: <b>shoppet79@gmail.com</b>,<br>
                               hoặc liên hệ qua sđt của chúng tôi 0975825324</p>
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
                return new ResponseMail { IsSuccess = true, Notifications = "Mã xác nhận đã được gửi tới mail của bạn!!" };
            }
            catch (Exception e)
            {

                return new ResponseMail { IsSuccess = true, Notifications = "Mã xác nhận chưa được gửi đi!!", Error = e.Message };
            }
        }
        public async Task<string> SendMailVerificationGuestAsync(string userMail, string verifyCode)
        {
            try
            {

                MailAddress mailFrom = new MailAddress("shoppet79@gmail.com", "MewShop");
                MailAddress mailTo = new MailAddress(userMail);
                MailMessage message = new MailMessage(mailFrom, mailTo);
                message.Subject = "Tài khoản đăng nhập của bạn";
                string verificationLink = @$"{_hosting}?verifyCode={verifyCode}";
                message.Body = $@"
                <body style=""font-family: Arial, sans-serif;"">
                    <h2>Tài khoản đăng nhập cá nhân vui lòng không để lộ!!</h2>
                    <p>Cuộc sống vốn có rất nhiều lựa chọn, cảm ơn bạn vì đã chọn làm việc với chúng tôi.<br> Chúc bạn đồng hành vui vẻ cùng MewShop.<br>
                    <p>Vui lòng <a href=""{HttpUtility.HtmlEncode(verificationLink)}"" style=""color: #007BFF; text-decoration: none;"">nhấn vào đây</a> để xác minh tài khoản của bạn.</p>
                    <b>Vui lòng không chia sẻ thông tin này với người khác.<br></b>
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
        public async Task<string> SendMailVerificationAsync(string userMail, string user, string pass, string verifyCode)
        {
            try
            {

                MailAddress mailFrom = new MailAddress("shoppet79@gmail.com", "MewShop");
                MailAddress mailTo = new MailAddress(userMail);
                MailMessage message = new MailMessage(mailFrom, mailTo);
                message.Subject = "Yêu cầu xác minh tài khoản";
                string verificationLink = @$"{_hosting}?verifyCode={verifyCode}";
                message.Body = $@"
                <body style=""font-family: Arial, sans-serif;"">
                    <h2>Đây là email để xác minh tài khoản của bạn!!</h2>
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
        public string GennarateVerifyCode(string ID)
        {
            Guid randomGuid = Guid.NewGuid();

            string verifyString = ID + "|" + DateTime.Now.AddMinutes(3).ToString();
            PasswordExtensitons hasCode = new PasswordExtensitons();
            string verifyCode = hasCode.HashCode(verifyString);
            return verifyCode;
        }
    }
}
