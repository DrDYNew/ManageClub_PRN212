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
using Microsoft.EntityFrameworkCore;
using Microsoft.Win32;
using static ManageClub_PRN212.Models.User;

namespace ManageClub_PRN212.WPF
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : Window
    {
        ManageClubContext _context;
        public Login()
        {
            InitializeComponent();
            _context = new ManageClubContext();
        }

        private void btnSubmit_Click(object sender, RoutedEventArgs e)
        {
            string msg = string.Empty;
            if (txtEmail.Text == string.Empty)
            {
                msg += "Email is required. \n";
            }
            if (txtPassword.Password == string.Empty)
            {
                msg += "Password is required.";
            }
            if (msg.Length > 0)
            {
                MessageBox.Show(msg);
                return;
            }

            User user = _context.Users.Include(x => x.Role).Where(x => x.Email == txtEmail.Text && x.Password == txtPassword.Password).FirstOrDefault();

            if (user == null)
            {
                MessageBox.Show("Email or password wrong! input again");
                return;
            }
            else
            {
                if (user.Status == "Inactive")
                {
                    MessageBox.Show("Tài khoản bị vô hiệu hóa");
                    return;
                }

                SessionDataUser.users.Add(user);
                if (user.RoleId == 1 || user.RoleId == 3)
                {
                    AccountWindow accountWindown = new AccountWindow();
                    accountWindown.Show();
                    this.Close();
                }
                else
                {
                    ProfileWindow accountWindown = new ProfileWindow();
                    accountWindown.Show();
                    this.Close();
                }
            }
        }

        private void tbForgotPassword_Click(object sender, RoutedEventArgs e)
        {
            ForgotPassword ForgotPasswordWindow = new ForgotPassword();
            ForgotPasswordWindow.Show();
            this.Close();
        }

        private void tbRegister_Click(object sender, RoutedEventArgs e)
        {
            Register registerWindow = new Register();
            registerWindow.Show();
            this.Close();
        }
    }
}
