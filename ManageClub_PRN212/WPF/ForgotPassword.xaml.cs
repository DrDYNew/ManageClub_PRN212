using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using ManageClub_PRN212.Models;
using static ManageClub_PRN212.Models.Mail;
using Microsoft.Extensions.Options;

namespace ManageClub_PRN212.WPF
{
    /// <summary>
    /// Interaction logic for ForgotPassword.xaml
    /// </summary>
    public partial class ForgotPassword : Window
    {
        ManageClubContext _context;
        public ForgotPassword()
        {
            InitializeComponent();
            _context = new ManageClubContext();
        }

        private void btnSubmit_Click(object sender, RoutedEventArgs e)
        {
            if (txtEmail.Text == string.Empty)
            {
                MessageBox.Show("Vui lòng không bỏ trống email", "Input Email");
                return;
            }

            User user = _context.Users.FirstOrDefault(x => x.Email == txtEmail.Text);

            if (user == null)
            {
                MessageBox.Show("Email không tồn tại trong hệ thống", "Input Email");
                return;
            }

            string newpassword = Mail.GenerateRandomPassword(6);

            user.Password = newpassword;
            _context.SaveChanges();

            var mailSettings = new MailSettings
            {
                Mail = "hightech05vn@gmail.com",
                DisplayName = "Ashin Mail System",
                Password = "hhhbasicfoexmpyw",
                Host = "smtp.gmail.com",
                Port = 587
            };
            Mail.SendMailService sendMailService = new Mail.SendMailService(Options.Create(mailSettings));
            sendMailService.SendEmailAsync(user.Email, "New PasswordReset", $"Mật khẩu mới của bạn là: <b>{newpassword}</b>");
            MessageBox.Show("Mật khẩu mới đã được gửi qua email!", "Thông báo");
        }

        private void tbRegister_Click(object sender, RoutedEventArgs e)
        {
            Register registerWindown = new Register();
            registerWindown.Show();
            this.Close();
        }

        private void tbLogin_Click(object sender, RoutedEventArgs e)
        {
            Login loginWindown = new Login();
            loginWindown.Show();
            this.Close();
        }
    }
}
