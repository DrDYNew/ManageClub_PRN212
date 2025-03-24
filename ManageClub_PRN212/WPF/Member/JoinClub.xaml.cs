using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using ManageClub_PRN212.DAO;
using ManageClub_PRN212.Models;
using System.Windows.Media;
using System.Net.Mail;
using System.Net;

namespace ManageClub_PRN212.WPF.Member
{
    public partial class JoinClubWPF : Window
    {
        private readonly ClubDAO _clubDAO;
        private readonly ClubMemberDAO _memberDAO;
        private readonly User _currentUser;
        public ObservableCollection<Club> Clubs { get; set; }
        private Button _selectedButton;
        private bool _isDataLoaded;

        public JoinClubWPF(User currentUser)
        {
            InitializeComponent();
            _clubDAO = new ClubDAO();
            _currentUser = currentUser ?? throw new ArgumentNullException(nameof(currentUser));

            Clubs = new ObservableCollection<Club>();
            dgClubs.ItemsSource = Clubs;

            _isDataLoaded = false;

            LoadClubs();
            LoadPresidents();

            _isDataLoaded = true;

            txtFilterClubName.TextChanged += TxtFilterClubName_TextChanged;
            cbFilterPresident.SelectionChanged += CbFilterPresident_SelectionChanged;

            _selectedButton = BtnJoinClubs;
            _selectedButton.Style = (Style)FindResource("SelectedNavbarButtonStyle");
        }

        private void LoadClubs()
        {
            try
            {
                var activeClubs = _clubDAO.GetActiveClubs();
                if (activeClubs != null && activeClubs.Any())
                {
                    Clubs.Clear();
                    foreach (var club in activeClubs)
                    {
                        if (club != null)
                        {
                            Clubs.Add(club);
                        }
                    }
                }
                else
                {
                    MessageBox.Show("No active clubs found.", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading clubs: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void LoadPresidents()
        {
            try
            {
                var presidents = _clubDAO.GetActiveClubs()
                    .Where(c => c != null && c.President != null)
                    .Select(c => c.President)
                    .DistinctBy(p => p.UserId)
                    .ToList();

                foreach (var president in presidents)
                {
                    if (president != null)
                    {
                        cbFilterPresident.Items.Add(president);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading presidents: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void DgClubs_Loaded(object sender, RoutedEventArgs e)
        {
            UpdateButtonStates();
        }

        private void UpdateButtonStates()
        {
            if (Clubs == null || !Clubs.Any()) return;

            foreach (var item in dgClubs.Items)
            {
                if (item is Club club && club != null)
                {
                    var row = (DataGridRow)dgClubs.ItemContainerGenerator.ContainerFromItem(item);
                    if (row != null)
                    {
                        var cell = dgClubs.Columns[5].GetCellContent(row) as ContentPresenter;
                        if (cell != null)
                        {
                            var button = (Button)VisualTreeHelper.GetChild(cell, 0);
                            if (_memberDAO.IsUserMember(club.ClubId, _currentUser.UserId))
                            {
                                button.Content = "Đã Join";
                                button.IsEnabled = false;
                            }
                            else
                            {
                                button.Content = "Join Club";
                                button.IsEnabled = true;
                            }
                        }
                    }
                }
            }
        }

        private void BtnJoinClub_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag is Club selectedClub && selectedClub != null)
            {
                if (_memberDAO.IsUserMember(selectedClub.ClubId, _currentUser.UserId))
                {
                    MessageBox.Show("You have already requested to join this club or are a member.", "Already Requested", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                var clubMember = new ClubMember
                {
                    ClubId = selectedClub.ClubId,
                    UserId = _currentUser.UserId,
                    JoinDate = DateTime.Now,
                    MemberStatus = "Pending"
                };

                _memberDAO.AddClubMember1(clubMember);
                SendJoinRequestEmail(selectedClub.ClubName, _currentUser.Email);

                MessageBox.Show($"Your request to join {selectedClub.ClubName} has been submitted!\nAn email has been sent to {_currentUser.Email}.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                LoadClubs();
                UpdateButtonStates();
            }
        }

        private void SendJoinRequestEmail(string clubName, string userEmail)
        {
            try
            {
                var fromAddress = new MailAddress("dungbd07@gmail.com", "Club Management");
                var toAddress = new MailAddress(userEmail);
                const string fromPassword = "qxuu rbkd khoy ubwu";
                const string subject = "Club Join Request Confirmation";
                string body = $"Dear {_currentUser.FullName},\n\n" +
                              $"You have successfully submitted a request to join the club: {clubName}.\n" +
                              $"Your request is currently pending approval by the club president.\n" +
                              "We will notify you once your request is approved.\n\n" +
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

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void BtnClearFilter_Click(object sender, RoutedEventArgs e)
        {
            txtFilterClubName.Text = "";
            cbFilterPresident.SelectedIndex = 0;
            LoadClubs();
            UpdateButtonStates();
        }

        private void TxtFilterClubName_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!_isDataLoaded) return;
            FilterClubs();
        }

        private void CbFilterPresident_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!_isDataLoaded) return;
            FilterClubs();
        }

        private void FilterClubs()
        {
            try
            {
                if (Clubs == null)
                {
                    Clubs = new ObservableCollection<Club>();
                }

                var filteredClubs = Clubs.Where(c => c != null).ToList();

                if (!string.IsNullOrWhiteSpace(txtFilterClubName.Text))
                {
                    string filterText = txtFilterClubName.Text.ToLower();
                    filteredClubs = filteredClubs
                        .Where(c => c.ClubName != null && c.ClubName.ToLower().Contains(filterText))
                        .ToList();
                }

                var selectedPresident = cbFilterPresident.SelectedItem as User;
                if (selectedPresident != null && cbFilterPresident.SelectedIndex > 0)
                {
                    filteredClubs = filteredClubs
                        .Where(c => c.President != null && c.President.UserId == selectedPresident.UserId)
                        .ToList();
                }

                dgClubs.ItemsSource = new ObservableCollection<Club>(filteredClubs ?? new List<Club>());
                UpdateButtonStates();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error filtering clubs: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnJoinClubs_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedButton != null)
                _selectedButton.Style = (Style)FindResource("NavbarButtonStyle");
            _selectedButton = (Button)sender;
            _selectedButton.Style = (Style)FindResource("SelectedNavbarButtonStyle");
        }

        private void BtnJoinEvents_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedButton != null)
                _selectedButton.Style = (Style)FindResource("NavbarButtonStyle");
            _selectedButton = (Button)sender;
            _selectedButton.Style = (Style)FindResource("SelectedNavbarButtonStyle");

            // Mở màn hình UserEventWPF
            UserEventWPF userEventWindow = new UserEventWPF(_currentUser);
            userEventWindow.Show();
            this.Close(); // Đóng JoinClubWPF sau khi mở UserEventWPF
        }

        private void BtnLogout_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("Are you sure you want to logout?", "Logout", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                Login loginWindow = new Login();
                loginWindow.Show();
                this.Close();
            }
        }
    }
}