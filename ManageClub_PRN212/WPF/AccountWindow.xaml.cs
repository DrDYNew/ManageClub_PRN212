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
using Microsoft.IdentityModel.Tokens;
using static ManageClub_PRN212.Models.User;

namespace ManageClub_PRN212.WPF
{
    /// <summary>
    /// Interaction logic for AccountWindow.xaml
    /// </summary>
    public partial class AccountWindow : Window
    {
        ManageClubContext _context;
        public AccountWindow()
        {
            InitializeComponent();
            _context = new ManageClubContext();
            LoadList();
            LoadComboBox();
        }

        public void LoadList()
        {
            IEnumerable<User> users = _context.Users.Include(x => x.Role).ToList();
            lvUser.ItemsSource = users;
        }

        public void LoadComboBox()
        {
            IEnumerable<Role> roles = _context.Roles.ToList();
            cbRoles.ItemsSource = roles;
        }

        private void lvUser_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lvUser.SelectedItem != null)
            {
                User user = (User)lvUser.SelectedItem;

                txtId.Text = user.UserId.ToString();
                if (user.Status == "Active")
                {
                    cbStatus.IsChecked = true;
                }
                else
                {
                    cbStatus.IsChecked = false;
                }
                cbRoles.SelectedValue = user.RoleId;
            }
        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            if (txtId.Text.IsNullOrEmpty() == true)
            {
                MessageBox.Show("Vui lòng chọn User để Update", "Get User");
                return;
            }

            User user = _context.Users.FirstOrDefault(x => x.UserId == int.Parse(txtId.Text));

            if (user == null)
            {
                MessageBox.Show("Không thể lấy user", "Get User");
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

                _context.Users.Update(user);
                _context.SaveChanges();
                LoadList();
                MessageBox.Show($"{user.UserId} update successfully", "Update");
            }

        }

        private void btnProfile_Click(object sender, RoutedEventArgs e)
        {
            ProfileWindow profileWindown = new ProfileWindow();
            profileWindown.Show();
            this.Close();
        }

        private void btnLogout_Click(object sender, RoutedEventArgs e)
        {
            SessionDataUser.users.Clear();
            Login loginWindown = new Login();
            loginWindown.Show();
            this.Close();
        }
    }
}
