using System.Windows;
using System.Windows.Controls;
using ManageClub_PRN212.DAO;
using ManageClub_PRN212.Models;
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
            
            var result = MessageBox.Show("Are you sure you want to logout?", "Logout", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                SessionDataUser.users.Clear();
                Login loginWindown = new Login();
                loginWindown.Show();
                this.Close();
            }
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

        private void BtnReport_Click(object sender, RoutedEventArgs e)
        {
            ReportWPF report = new ReportWPF();
            report.Show();
            this.Close();
        }

        private void BtnUserManagement_Click(object sender, RoutedEventArgs e)
        {
                new UserManagementWPF(SessionDataUser.users[0]).Show();
                this.Close();
        }

        private void BtnAttendance_Click(object sender, RoutedEventArgs e)
        {
            new AttendanceWPF(SessionDataUser.users[0]).ShowDialog();
        }
    }
}
