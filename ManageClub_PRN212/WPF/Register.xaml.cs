using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
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
using ManageClub_PRN212.DAO;
using ManageClub_PRN212.Models;

namespace ManageClub_PRN212.WPF
{
    /// <summary>
    /// Interaction logic for Register.xaml
    /// </summary>
    public partial class Register : Window
    {

        public Register()
        {
            InitializeComponent();
        }

        private void btnRegister_Click(object sender, RoutedEventArgs e)
        {
            string fullName = txtFullName.Text.Trim();
            string email = txtEmail.Text.Trim();
            string password = txtPassword.Password;
            string confirmPassword = txtConfirmPassword.Password;
            DateTime? dateOfBirth = dpDateOfBirth.SelectedDate;
            string phoneNumber = txtPhoneNumber.Text.Trim();
            string address = txtAddress.Text.Trim();

            if (!ValidateInputs(fullName, email, password, confirmPassword, dateOfBirth, phoneNumber, address))
            {
                return;
            }

            var newUser = new User
            {
                FullName = fullName,
                Email = email,
                Password = password,
                DateOfBirth = dateOfBirth.Value,
                PhoneNumber = phoneNumber,
                Address = address,
                DateJoined = DateTime.Now,
                Status = "Active",
                RoleId = 4
            };

            try
            {
                UserDAO.RegisterUser(newUser);
                SendConfirmationEmail(email, fullName);
                MessageBox.Show("Registration successful! A confirmation email has been sent.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                this.Close();
                Login loginWindow = new Login();
                loginWindow.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Registration failed: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        private bool ValidateInputs(string fullName, string email, string password, string confirmPassword, DateTime? dateOfBirth, string phoneNumber, string address)
        {
            // Reset border colors
            ResetFieldBorders();

            bool isValid = true;

            // Validate FullName
            if (string.IsNullOrEmpty(fullName) || fullName.Length < 2)
            {
                txtFullName.BorderBrush = Brushes.Red;
                MessageBox.Show("Full Name must be at least 2 characters long!", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                isValid = false;
            }

            // Validate Email
            if (string.IsNullOrEmpty(email) || !Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
            {
                txtEmail.BorderBrush = Brushes.Red;
                MessageBox.Show("Please enter a valid email address!", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                isValid = false;
            }

            // Validate Password
            if (string.IsNullOrEmpty(password) || password.Length < 6)
            {
                txtPassword.BorderBrush = Brushes.Red;
                MessageBox.Show("Password must be at least 6 characters long!", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                isValid = false;
            }

            // Validate Confirm Password
            if (password != confirmPassword)
            {
                txtConfirmPassword.BorderBrush = Brushes.Red;
                MessageBox.Show("Passwords do not match!", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                isValid = false;
            }

            // Validate Date of Birth
            if (!dateOfBirth.HasValue || dateOfBirth.Value > DateTime.Now.AddYears(-10))
            {
                dpDateOfBirth.BorderBrush = Brushes.Red;
                MessageBox.Show("Please select a valid Date of Birth (at least 10 years ago)!", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                isValid = false;
            }

            // Validate Phone Number
            if (string.IsNullOrEmpty(phoneNumber) || !Regex.IsMatch(phoneNumber, @"^\d{10}$"))
            {
                txtPhoneNumber.BorderBrush = Brushes.Red;
                MessageBox.Show("Phone Number must be exactly 10 digits!", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                isValid = false;
            }

            // Validate Address (optional, nhưng nếu nhập thì phải có nội dung hợp lệ)
            if (!string.IsNullOrEmpty(address) && address.Length < 5)
            {
                txtAddress.BorderBrush = Brushes.Red;
                MessageBox.Show("Address must be at least 5 characters long if provided!", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                isValid = false;
            }

            return isValid;
        }

        private void ResetFieldBorders()
        {
            txtFullName.BorderBrush = Brushes.Gray;
            txtEmail.BorderBrush = Brushes.Gray;
            txtPassword.BorderBrush = Brushes.Gray;
            txtConfirmPassword.BorderBrush = Brushes.Gray;
            dpDateOfBirth.BorderBrush = Brushes.Gray;
            txtPhoneNumber.BorderBrush = Brushes.Gray;
            txtAddress.BorderBrush = Brushes.Gray;
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void SendConfirmationEmail(string toEmail, string fullName)
        {
            var fromAddress = new MailAddress("dungbd07@gmail.com", "Club Management");
            var toAddress = new MailAddress(toEmail, fullName);
            const string fromPassword = "qxuu rbkd khoy ubwu";
            const string subject = "Welcome to Club Management!";
            string body = $"Dear {fullName},\n\nYour account has been successfully registered!\n\nBest regards,\nClub Management Team";

            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new System.Net.NetworkCredential(fromAddress.Address, fromPassword)
            };

            using (var message = new MailMessage(fromAddress, toAddress)
            {
                Subject = subject,
                Body = body
            })
            {
                smtp.Send(message);
            }
        }

        private void dpDateOfBirth_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            // Mở lịch khi nhấp vào DatePicker
            if (sender is DatePicker datePicker)
            {
                datePicker.IsDropDownOpen = true;
            }
        }

        private void tbLogin_Click(object sender, RoutedEventArgs e)
        {
            Login loginWindown = new Login();
            loginWindown.Show();
            this.Close();
        }

        private void tbForgotPassword_Click(object sender, RoutedEventArgs e)
        {
            ForgotPassword ForgotPasswordWindow = new ForgotPassword();
            ForgotPasswordWindow.Show();
            this.Close();
        }
    }
}
