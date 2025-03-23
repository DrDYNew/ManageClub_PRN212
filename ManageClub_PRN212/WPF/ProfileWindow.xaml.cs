using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
using static ManageClub_PRN212.Models.User;

namespace ManageClub_PRN212.WPF
{
    /// <summary>
    /// Interaction logic for ProfileWindow.xaml
    /// </summary>
    public partial class ProfileWindow : Window
    {
        ManageClubContext _context;
        public ProfileWindow()
        {
            InitializeComponent();
            _context = new ManageClubContext();
            LoadProfile();
        }

        public void LoadProfile()
        {
            // Kiểm tra xem danh sách users có dữ liệu không
            if (SessionDataUser.users == null || SessionDataUser.users.Count == 0)
            {
                MessageBox.Show("User session expired. Please log in again.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                this.Close();
                return;
            }

            User userSession = SessionDataUser.users[0];


            User user = _context.Users.FirstOrDefault(x => x.UserId == userSession.UserId);


            if (user != null)
            {
                txtName.Text = user.FullName;
                txtEmail.Text = user.Email;
                txtDob.SelectedDate = user.DateOfBirth;
                txtPhone.Text = user.PhoneNumber.ToString();
                txtAddress.Text = user.Address;
                txtAvatarURL.Text = string.IsNullOrEmpty(user.AvatarUrl) ? "https://example.com/avatar4.jpg" : user.AvatarUrl;
                if (user.RoleId == 2)
                {
                    tbBack.Visibility = Visibility.Collapsed;
                }
            }
            else
            {
                MessageBox.Show("User not found in the database.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnSubmit_Click(object sender, RoutedEventArgs e)
        {
            string name = txtName.Text.Trim();
            string email = txtEmail.Text.Trim();
            string phone = txtPhone.Text.Trim();
            string address = txtAddress.Text.Trim();
            string avatarUrl = txtAvatarURL.Text.Trim();
            DateTime? dob = txtDob.SelectedDate;

            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(email) || string.IsNullOrEmpty(phone) || string.IsNullOrEmpty(address) || string.IsNullOrEmpty(avatarUrl) || dob == null)
            {
                MessageBox.Show("Vui lòng điền đầy đủ thông tin!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
            {
                MessageBox.Show("Email không hợp lệ!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!Regex.IsMatch(phone, @"^\d{10,11}$"))
            {
                MessageBox.Show("Số điện thoại không hợp lệ!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Kiểm tra xem danh sách users có dữ liệu không
            if (SessionDataUser.users == null || SessionDataUser.users.Count == 0)
            {
                MessageBox.Show("User session expired. Please log in again.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                this.Close();
                return;
            }

            User userSession = SessionDataUser.users[0];

            User user = _context.Users.FirstOrDefault(x => x.UserId == userSession.UserId);

            user.FullName = name;
            user.Email = email;
            user.PhoneNumber = phone;
            user.Address = address;
            user.AvatarUrl = avatarUrl;
            user.DateOfBirth = dob;

            _context.Users.Update(user);
            _context.SaveChanges();
            LoadProfile();
            MessageBox.Show("Thay đổi thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void tbLogout_Click(object sender, RoutedEventArgs e)
        {
            SessionDataUser.users.Clear();
            Login loginWindown = new Login();
            loginWindown.Show();
            this.Close();
        }

        private void tbBack_Click(object sender, RoutedEventArgs e)
        {
            AccountWindow AccountWindown = new AccountWindow();
            AccountWindown.Show();
            this.Close();
        }
    }
}
