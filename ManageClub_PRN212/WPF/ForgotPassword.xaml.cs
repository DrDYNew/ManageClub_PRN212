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
using ManageClub_PRN212.DAO;

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
                MessageBox.Show("Please do not leave the email empty.", "Input Email", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            User user = UserDAO.GetUserByEmail(txtEmail.Text);

            if (user == null)
            {
                MessageBox.Show("The email does not exist in the system.", "Input Email", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            string newpassword = Mail.GenerateRandomPassword(6);

            UserDAO.UpdateForgotPassword(user, newpassword);

            var mailSettings = new MailSettings
            {
                Mail = "hightech05vn@gmail.com",
                DisplayName = "Ashin Mail System",
                Password = "hhhbasicfoexmpyw",
                Host = "smtp.gmail.com",
                Port = 587
            };
            Mail.SendMailService sendMailService = new Mail.SendMailService(Options.Create(mailSettings));
            sendMailService.SendEmailAsync(user.Email, "New PasswordReset", $"Your new password is: <b>{newpassword}</b>");
            MessageBox.Show("The new password has been sent to your email!", "Notification");
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

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
