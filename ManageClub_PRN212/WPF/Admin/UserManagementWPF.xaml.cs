using ManageClub_PRN212.DAO;
using ManageClub_PRN212.Models;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using ManageClub_PRN212.WPF.Admin;

namespace ManageClub_PRN212.WPF.Admin
{
    public partial class UserManagementWPF : Window
    {
        private readonly UserDAO _userDAO;
        private readonly RoleDAO _roleDAO;
        private Button _selectedButton;
        private readonly User _currentUser;
        public ObservableCollection<User> Users { get; set; }
        private User _selectedUser;

        public UserManagementWPF(User user)
        {
            InitializeComponent();
            _currentUser = user;
            _userDAO = new UserDAO();
            LoadUsers();
            dgUsers.ItemsSource = Users;
            cbRole.ItemsSource = _roleDAO.GetAllRoles();
            cbStatus.ItemsSource = new[] { "Active", "Inactive" };
            _selectedButton = BtnUserManagement;
            _selectedButton.Style = (Style)FindResource("SelectedSidebarButtonStyle");
        }

        private void LoadUsers()
        {
            try
            {
                Users = new ObservableCollection<User>(_userDAO.GetAllUsers());
                if (!Users.Any())
                {
                    MessageBox.Show("No users found.", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading users: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void DgUsers_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dgUsers.SelectedItem is User user)
            {
                _selectedUser = user;
                txtFullName.Text = user.FullName;
                txtEmail.Text = user.Email;
                cbRole.SelectedItem = _roleDAO.GetAllRoles().FirstOrDefault(r => r.RoleId == user.RoleId);
                dpDateOfBirth.SelectedDate = user.DateOfBirth.HasValue ? DateTime.Parse(user.DateOfBirth.Value.ToString()) : (DateTime?)null;
                txtPhoneNumber.Text = user.PhoneNumber;
                txtAddress.Text = user.Address;
                cbStatus.SelectedItem = user.Status;
                btnUpdate.IsEnabled = true;
            }
            else
            {
                ClearFields();
                btnUpdate.IsEnabled = false;
            }
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtFullName.Text) || string.IsNullOrWhiteSpace(txtEmail.Text) || cbRole.SelectedItem == null)
                {
                    MessageBox.Show("Please fill in all required fields (Full Name, Email, Role).", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                var password = GenerateRandomPassword();
                var newUser = new User
                {
                    FullName = txtFullName.Text,
                    Email = txtEmail.Text,
                    RoleId = ((Role)cbRole.SelectedItem).RoleId,
                    Password = HashPassword(password),
                    DateOfBirth = dpDateOfBirth.SelectedDate.HasValue ? DateOnly.FromDateTime(dpDateOfBirth.SelectedDate.Value) : (DateOnly?)null,
                    PhoneNumber = txtPhoneNumber.Text,
                    Address = txtAddress.Text,
                    Status = "Active",
                    DateJoined = DateTime.Now
                };

                _userDAO.AddUser(newUser);
                SendEmail(newUser, password);

                MessageBox.Show("User added successfully. Password sent to email.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                LoadUsers();
                dgUsers.ItemsSource = Users;
                ClearFields();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error adding user: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnUpdate_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedUser != null)
            {
                try
                {
                    _selectedUser.FullName = txtFullName.Text;
                    _selectedUser.Email = txtEmail.Text;
                    _selectedUser.RoleId = ((Role)cbRole.SelectedItem).RoleId;
                    _selectedUser.DateOfBirth = dpDateOfBirth.SelectedDate.HasValue ? DateOnly.FromDateTime(dpDateOfBirth.SelectedDate.Value) : (DateOnly?)null;
                    _selectedUser.PhoneNumber = txtPhoneNumber.Text;
                    _selectedUser.Address = txtAddress.Text;
                    _selectedUser.Status = cbStatus.SelectedItem.ToString();

                    UserDAO.UpdateUser(_selectedUser);
                    MessageBox.Show("User updated successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    LoadUsers();
                    dgUsers.ItemsSource = Users;
                    ClearFields();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error updating user: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void BtnClear_Click(object sender, RoutedEventArgs e)
        {
            ClearFields();
        }

        private void ClearFields()
        {
            _selectedUser = null;
            txtFullName.Text = string.Empty;
            txtEmail.Text = string.Empty;
            cbRole.SelectedItem = null;
            dpDateOfBirth.SelectedDate = null;
            txtPhoneNumber.Text = string.Empty;
            txtAddress.Text = string.Empty;
            cbStatus.SelectedItem = null;
            btnUpdate.IsEnabled = false;
        }

        private string GenerateRandomPassword(int length = 8)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var random = new Random();
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        private string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(hashedBytes);
            }
        }

        private void SendEmail(User user, string password)
        {
            try
            {
                var fromAddress = new MailAddress("dungbd07@gmail.com", "Club Management");
                var toAddress = new MailAddress(user.Email);
                const string fromPassword = "qxuu rbkd khoy ubwu"; // App Password từ Gmail
                string subject = "Your Club Management Account";
                string body = $"Dear {user.FullName},\n\n" +
                              $"Your account has been created successfully.\n" +
                              $"Email: {user.Email}\n" +
                              $"Password: {password}\n" +
                              $"Date Joined: {user.DateJoined?.ToString("dd/MM/yyyy")}\n" +
                              $"Please change your password after logging in.\n\n" +
                              "Best regards,\nClub Management Team";

                var smtp = new SmtpClient
                {
                    Host = "smtp.gmail.com",
                    Port = 587,
                    EnableSsl = true,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
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
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to send email: {ex.Message}", "Email Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnUserManagement_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedButton != null)
                _selectedButton.Style = (Style)FindResource("SidebarButtonStyle");
            _selectedButton = (Button)sender;
            _selectedButton.Style = (Style)FindResource("SelectedSidebarButtonStyle");
        }

        private void BtnClubManagement_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedButton != null)
                _selectedButton.Style = (Style)FindResource("SidebarButtonStyle");
            _selectedButton = (Button)sender;
            _selectedButton.Style = (Style)FindResource("SelectedSidebarButtonStyle");
            new ClubManagement(_currentUser).Show();
            this.Close();
        }

        private void BtnReport_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedButton != null)
                _selectedButton.Style = (Style)FindResource("SidebarButtonStyle");
            _selectedButton = (Button)sender;
            _selectedButton.Style = (Style)FindResource("SelectedSidebarButtonStyle");
            new ReportWPF().Show();
            this.Close();
        }

        private void BtnLogout_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("Are you sure you want to logout?", "Logout", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                new Login().Show();
                this.Close();
            }
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}