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
using ManageClub_PRN212.DAO;
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
        public Login()
        {
            InitializeComponent();
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
                MessageBox.Show(msg, "Validation", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            User user = UserDAO.GetUserByLogIn(txtEmail.Text, txtPassword.Password);

            if (user == null)
            {
                MessageBox.Show("Email or password wrong! input again", "Validation", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            else
            {
                if (user.Status == "Inactive")
                {
                    MessageBox.Show("The account has been disabled.", "Validation", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                SessionDataUser.users.Add(user);

                switch (user.RoleId)
                {
                    case 1:
                        AccountWindow accountWindow = new AccountWindow();
                        accountWindow.Show();
                        this.Close();
                        break;
                    case 2:
                        break;
                    case 3:
                        break;
                    case 4:
                        break;
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

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
