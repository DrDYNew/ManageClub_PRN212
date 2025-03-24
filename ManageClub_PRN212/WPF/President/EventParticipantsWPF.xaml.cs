using ManageClub_PRN212.DAO;
using ManageClub_PRN212.Models;
using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Net.Mail;
using System.Net;
using ManageClub_PRN212.WPF.President;

namespace ManageClub_PRN212.WPF.President
{
    public partial class EventParticipantsWPF : Window
    {
        private readonly EventDAO _eventDAO;
        private readonly User _currentUser;
        private Button _selectedButton;
        public ObservableCollection<EventParticipant> Participants { get; set; }

        public EventParticipantsWPF(User currentUser)
        {
            InitializeComponent();
            _eventDAO = new EventDAO();
            _currentUser = currentUser ?? throw new ArgumentNullException(nameof(currentUser));
            LoadParticipants();
            dgParticipants.ItemsSource = Participants;
            // Mặc định chọn "Participants"
            _selectedButton = BtnParticipants;
            _selectedButton.Style = (Style)FindResource("SelectedSidebarButtonStyle");
        }

        private void LoadParticipants()
        {
            try
            {
                var allParticipants = _eventDAO.GetAllEventParticipants();
                Participants = new ObservableCollection<EventParticipant>(
                    allParticipants.Where(ep =>
                        ep.Event.OrganizerId == _currentUser.UserId ||
                        ep.Event.Club != null && ep.Event.Club.PresidentId == _currentUser.UserId
                    ).ToList()
                );

                if (!Participants.Any())
                {
                    MessageBox.Show("No participants found for events you manage.", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading participants: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnAccept_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag is EventParticipant participant)
            {
                try
                {
                    participant.Status = "Accepted";
                    _eventDAO.UpdateEventParticipant(participant);

                    SendEmail(participant, "Accepted");

                    MessageBox.Show($"Participant accepted for {participant.Event.EventName}.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    LoadParticipants();
                    dgParticipants.ItemsSource = Participants;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error accepting participant: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void BtnReject_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag is EventParticipant participant)
            {
                Window rejectWindow = new Window
                {
                    Title = "Reject Participant",
                    Width = 300,
                    Height = 200,
                    WindowStartupLocation = WindowStartupLocation.CenterOwner,
                    Owner = this
                };

                var stackPanel = new StackPanel { Margin = new Thickness(10) };
                var lblReason = new TextBlock { Text = "Enter reason for rejection:", Margin = new Thickness(0, 0, 0, 10) };
                var txtReason = new TextBox { Name = "txtReason", Height = 80, AcceptsReturn = true, VerticalScrollBarVisibility = ScrollBarVisibility.Auto };
                var btnSubmit = new Button { Content = "Submit", Width = 80, Margin = new Thickness(0, 10, 0, 0) };

                btnSubmit.Click += (s, ev) =>
                {
                    try
                    {
                        if (string.IsNullOrWhiteSpace(txtReason.Text))
                        {
                            MessageBox.Show("Please provide a reason for rejection.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                            return;
                        }
                        participant.Reason = txtReason.Text;
                        participant.Status = "Rejected";
                        _eventDAO.UpdateEventParticipant(participant);

                        // Gửi email thông báo khi Reject
                        SendEmail(participant, "Rejected");

                        rejectWindow.Close();
                        MessageBox.Show($"Participant rejected for {participant.Event.EventName}.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                        LoadParticipants();
                        dgParticipants.ItemsSource = Participants;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error rejecting participant: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                };

                stackPanel.Children.Add(lblReason);
                stackPanel.Children.Add(txtReason);
                stackPanel.Children.Add(btnSubmit);
                rejectWindow.Content = stackPanel;

                rejectWindow.ShowDialog();
            }
        }

        private void SendEmail(EventParticipant participant, string action)
        {
            try
            {
                var fromAddress = new MailAddress("dungbd07@gmail.com", "Club Management");
                var toAddress = new MailAddress(participant.User.Email);
                const string fromPassword = "qxuu rbkd khoy ubwu";
                string subject = $"Event Participation {action}";
                string body = $"Dear {participant.User.FullName},\n\n" +
                              $"Your registration for the event '{participant.Event.EventName}' has been {action.ToLower()}.\n" +
                              $"Event Date: {participant.Event.EventDate.ToString("dd/MM/yyyy")}\n";

                if (action == "Rejected" && !string.IsNullOrEmpty(participant.Reason))
                {
                    body += $"Reason for rejection: {participant.Reason}\n";
                }

                body += "\nBest regards,\nClub Management Team";

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
                MessageBox.Show($"Failed to send email: {ex.Message}\nInner Exception: {ex.InnerException?.Message}", "Email Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnUserName_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.DataContext is EventParticipant participant)
            {
                var user = participant.User;
                Window userInfoWindow = new Window
                {
                    Title = "User Information",
                    Width = 400,
                    Height = 300,
                    WindowStartupLocation = WindowStartupLocation.CenterOwner,
                    Owner = this,
                    Content = new StackPanel
                    {
                        Margin = new Thickness(10),
                        Children =
                        {
                            new TextBlock { Text = $"Full Name: {user.FullName}" },
                            new TextBlock { Text = $"Email: {user.Email}" },
                            new TextBlock { Text = $"Date of Birth: {user.DateOfBirth?.ToString("dd/MM/yyyy") ?? "N/A"}" },
                            new TextBlock { Text = $"Phone Number: {user.PhoneNumber ?? "N/A"}" },
                            new TextBlock { Text = $"Address: {user.Address ?? "N/A"}" },
                            new TextBlock { Text = $"Date Joined: {user.DateJoined?.ToString("dd/MM/yyyy") ?? "N/A"}" },
                            new TextBlock { Text = $"Status: {user.Status}" }
                        }
                    }
                };
                userInfoWindow.ShowDialog();
            }
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void BtnEventManagement_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedButton != null)
                _selectedButton.Style = (Style)FindResource("SidebarButtonStyle");
            _selectedButton = (Button)sender;
            _selectedButton.Style = (Style)FindResource("SelectedSidebarButtonStyle");
            new EventWPF(_currentUser).Show();
            Hide();
        }

        private void BtnParticipants_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedButton != null)
                _selectedButton.Style = (Style)FindResource("SidebarButtonStyle");
            _selectedButton = (Button)sender;
            _selectedButton.Style = (Style)FindResource("SelectedSidebarButtonStyle");
        }

        private void BtnJoinListEvent_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedButton != null)
                _selectedButton.Style = (Style)FindResource("SidebarButtonStyle");
            _selectedButton = (Button)sender;
            _selectedButton.Style = (Style)FindResource("SelectedSidebarButtonStyle");
            new ListJoinEvent(_currentUser).Show();
            Close();
        }

        private void BtnListMemberJoinClub_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedButton != null)
                _selectedButton.Style = (Style)FindResource("SidebarButtonStyle");
            _selectedButton = (Button)sender;
            _selectedButton.Style = (Style)FindResource("SelectedSidebarButtonStyle");
            new ListMemberJoinClub(_currentUser).Show();
            Close();
        }

        private void BtnLogout_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("Are you sure you want to logout?", "Logout", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                Login loginWindow = new Login();
                loginWindow.Show();
                Close();
            }
        }
        private void BtnManageClubFinance_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedButton != null)
                _selectedButton.Style = (Style)FindResource("SidebarButtonStyle");
            _selectedButton = (Button)sender;
            _selectedButton.Style = (Style)FindResource("SelectedSidebarButtonStyle");
            new ManageClubFinance(_currentUser).Show();
            Close();
        }
    }
}