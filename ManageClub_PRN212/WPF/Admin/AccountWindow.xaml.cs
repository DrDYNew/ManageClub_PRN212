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
using Microsoft.IdentityModel.Tokens;
using static ManageClub_PRN212.Models.User;

namespace ManageClub_PRN212.WPF.Admin
{
    /// <summary>
    /// Interaction logic for AccountWindow.xaml
    /// </summary>
    public partial class AccountWindow : Window
    {
        public AccountWindow()
        {
            InitializeComponent();
            LoadList();
            LoadComboBox();
        }

        public void LoadList()
        {
            IEnumerable<User> users = UserDAO.GetUserWithRole();
            dgAccounts.ItemsSource = users;
        }

        public void LoadComboBox()
        {
            IEnumerable<Role> roles = RoleDAO.GetRoles();
            cbRoles.ItemsSource = roles;
        }

        private void BtnClubManagement_Click(object sender, RoutedEventArgs e)
        {
            ClubManagement clubManagement = new ClubManagement(SessionDataUser.users[0]);
            clubManagement.Show();
            this.Close();
        }

        private void BtnLogout_Click(object sender, RoutedEventArgs e)
        {
            SessionDataUser.users.Clear();
            Login loginWindown = new Login();
            loginWindown.Show();
            this.Close();
        }

        private void dgAccounts_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dgAccounts.SelectedItem != null)
            {
                User user = (User)dgAccounts.SelectedItem;

                txtId.Text = user.UserId.ToString();
                if (user.Status == "Active")
                {
                    cbStatus.IsChecked = true;
                }
                else
                {
                    cbStatus.IsChecked = false;
                }
                dpEventDate.SelectedDate = user.DateJoined;
                cbRoles.SelectedValue = user.RoleId;
            }
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            if (txtId.Text.IsNullOrEmpty() == true)
            {
                MessageBox.Show("Please select a user to update.", "Get User");
                return;
            }

            User user = UserDAO.GetUserById(Int32.Parse(txtId.Text));

            if (user == null)
            {
                MessageBox.Show("Cannot get user", "Get User");
                return;
            }
            else
            {
                if (cbStatus.IsChecked == true)
                {
                    user.Status = "Active";
                }
                else
                {
                    user.Status = "Inactive";
                }

                user.RoleId = int.Parse(cbRoles.SelectedValue.ToString());

                UserDAO.UpdateUser(user);

                LoadList();
                MessageBox.Show($"{user.UserId} update successfully", "Update");
            }

        }

        private void BtnMyProfile_Click(object sender, RoutedEventArgs e)
        {
            ProfileWindow profileWindow = new ProfileWindow();
            profileWindow.ShowDialog();
        }
    }
}
