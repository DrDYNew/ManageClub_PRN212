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

namespace ManageClub_PRN212.WPF
{
    /// <summary>
    /// Interaction logic for Register.xaml
    /// </summary>
    public partial class Register : Window
    {
        ManageClubContext _context;
        public Register()
        {
            InitializeComponent();
            _context = new ManageClubContext();
        }


        private void btnSubmit_Click(object sender, RoutedEventArgs e)
        {
            string name = txtName.Text.Trim();
            string email = txtEmail.Text.Trim();
            string phone = txtPhone.Text.Trim();
            string address = txtAddress.Text.Trim();
            string avatarUrl = txtAvatarURL.Text.Trim();
            string password = txtPassword.Password.Trim();
            string rePassword = txtRepassword.Password.Trim();
            DateTime? dob = txtDob.SelectedDate;




            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(email) || string.IsNullOrEmpty(phone) ||
    string.IsNullOrEmpty(address) || string.IsNullOrEmpty(avatarUrl) || string.IsNullOrEmpty(password) ||
    string.IsNullOrEmpty(rePassword) || dob == null)
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

            if (!Regex.IsMatch(password, @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{6,}$"))
            {
                MessageBox.Show("Mật khẩu phải có ít nhất 6 ký tự, bao gồm chữ hoa, chữ thường và số!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (password != rePassword)
            {
                MessageBox.Show("Mật khẩu nhập lại không khớp!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            User checkEmail = _context.Users.FirstOrDefault(x => x.Email == email);
            if (checkEmail != null)
            {
                MessageBox.Show("Email đã có trong hệ thống! Vui lòng nhập Email khác", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            User user = new User
            {
                FullName = name,
                Email = email,
                RoleId = 2,
                PhoneNumber = phone,
                Address = address,
                AvatarUrl = avatarUrl,
                Password = password,
                DateOfBirth = dob,
                DateJoined = DateTime.Now,
                Status = "Active"
            };


            _context.Users.Add(user);
            _context.SaveChanges();
            MessageBox.Show("Đăng ký thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
            Login login = new Login();
            login.Show();
            this.Close();
        }

        private void tbLogin_Click(object sender, RoutedEventArgs e)
        {
            Login login = new Login();
            login.Show();
            this.Close();
        }

        private void tbForgotPassword_Click(object sender, RoutedEventArgs e)
        {
            ForgotPassword ForgotPasswordWindown = new ForgotPassword();
            ForgotPasswordWindown.Show();
            this.Close();
        }

    }
}
