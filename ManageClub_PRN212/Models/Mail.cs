using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MailKit.Security;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MimeKit;

namespace ManageClub_PRN212.Models
{
    public class Mail
    {
        public class MailSettings
        {
            public string Mail { get; set; }
            public string DisplayName { get; set; }
            public string Password { get; set; }
            public string Host { get; set; }
            public int Port { get; set; }

        }

        public interface IEmailSender
        {
            Task SendEmailAsync(string email, string subject, string htmlMessage);
        }

        private static Random random = new Random();

        public static string GenerateRandomPassword(int length = 6)
        {
            if (length < 6) length = 6; // Đảm bảo độ dài tối thiểu là 6

            const string uppercase = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            const string lowercase = "abcdefghijklmnopqrstuvwxyz";
            const string digits = "0123456789";
            const string allChars = uppercase + lowercase + digits;

            // Chọn ít nhất 1 chữ hoa, 1 chữ thường, 1 số
            char upper = uppercase[random.Next(uppercase.Length)];
            char lower = lowercase[random.Next(lowercase.Length)];
            char digit = digits[random.Next(digits.Length)];

            // Tạo các ký tự ngẫu nhiên còn lại
            char[] password = new char[length];
            password[0] = upper;
            password[1] = lower;
            password[2] = digit;

            for (int i = 3; i < length; i++)
            {
                password[i] = allChars[random.Next(allChars.Length)];
            }

            // Xáo trộn chuỗi để không bị đoán vị trí dễ dàng
            return new string(password.OrderBy(x => random.Next()).ToArray());
        }

        public class SendMailService : IEmailSender
        {
            private readonly MailSettings mailSettings;

            private readonly ILogger<SendMailService> logger;

            public SendMailService(IOptions<MailSettings> _mailSettings)
            {
                mailSettings = _mailSettings.Value;
            }

            public async Task SendEmailAsync(string email, string subject, string htmlMessage)
            {
                var message = new MimeMessage();
                message.Sender = new MailboxAddress(mailSettings.DisplayName, mailSettings.Mail);
                message.From.Add(new MailboxAddress(mailSettings.DisplayName, mailSettings.Mail));
                message.To.Add(MailboxAddress.Parse(email));
                message.Subject = subject;

                var builder = new BodyBuilder();

                if (subject.Equals("New PasswordReset"))
                {
                    builder.HtmlBody = $@"<!DOCTYPE html>
                        <html lang=""en"">
                        <head>
                            <meta charset=""UTF-8"">
                            <meta http-equiv=""X-UA-Compatible"" content=""IE=edge"">
                            <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
                            <title>Ashin</title>
                        </head>
                        <body>
                            <div style=""font-family: Arial, sans-serif; max-width: 600px; margin: 0 auto;"">
                                <h2>{subject}</h2>
                                <hr>
                                <p>{htmlMessage}</p>
                            </div>
                        </body>
                        </html>";
                }

                message.Body = builder.ToMessageBody();

                using var smtp = new MailKit.Net.Smtp.SmtpClient();

                try
                {
                    smtp.Connect(mailSettings.Host, mailSettings.Port, SecureSocketOptions.StartTls);
                    smtp.Authenticate(mailSettings.Mail, mailSettings.Password);
                    await smtp.SendAsync(message);
                }
                catch (Exception ex)
                {
                    System.IO.Directory.CreateDirectory("mailssave");
                    var emailsavefile = string.Format(@"mailssave/{0}.eml", Guid.NewGuid());
                    await message.WriteToAsync(emailsavefile);

                    logger.LogInformation("Lỗi gửi mail, lưu tại - " + emailsavefile);
                    logger.LogError(ex.Message);
                }

                smtp.Disconnect(true);

                logger.LogInformation("send mail to: " + email);
            }
        }
    }
}
