using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using ManageClub_PRN212.DAO;
using ManageClub_PRN212.Models;
using System.Net.Mail;
using System.Net;
using System.Windows.Media;
using static ManageClub_PRN212.Models.User;

namespace ManageClub_PRN212.WPF.Member
{
    public partial class UserEventWPF : Window
    {
        private readonly EventDAO _eventDAO;
        private readonly User _currentUser;
        public ObservableCollection<Event> Events { get; set; }
        private Button _selectedButton;

        public UserEventWPF(User currentUser)
        {
            InitializeComponent();
            _eventDAO = new EventDAO();
            _currentUser = currentUser ?? throw new ArgumentNullException(nameof(currentUser));

            LoadEvents();
            dgEvents.ItemsSource = Events;

            txtFilter.TextChanged += TxtFilter_TextChanged;
            dpStartDateFilter.SelectedDateChanged += DpStartDateFilter_SelectedDateChanged;
            dpEndDateFilter.SelectedDateChanged += DpEndDateFilter_SelectedDateChanged;

            _selectedButton = BtnJoinEvents;
            _selectedButton.Style = (Style)FindResource("SelectedNavbarButtonStyle");
        }

        private void LoadEvents()
        {
            try
            {
                Events = new ObservableCollection<Event>(_eventDAO.GetEventsForMember(_currentUser.UserId));
                dgEvents.ItemsSource = Events;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading events: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void DgEvents_Loaded(object sender, RoutedEventArgs e)
        {
            UpdateButtonStates();
        }

        private void UpdateButtonStates()
        {
            foreach (var item in dgEvents.Items)
            {
                if (item is Event ev)
                {
                    var row = (DataGridRow)dgEvents.ItemContainerGenerator.ContainerFromItem(item);
                    if (row != null)
                    {
                        var cell = dgEvents.Columns[7].GetCellContent(row) as ContentPresenter;
                        if (cell != null)
                        {
                            var button = (Button)VisualTreeHelper.GetChild(cell, 0);
                            if (_eventDAO.IsUserRegistered(ev.EventId, _currentUser.UserId))
                            {
                                button.Content = "Đã Join Sự kiện";
                                button.IsEnabled = false;
                            }
                            else
                            {
                                button.Content = "Join Event";
                                button.IsEnabled = true;
                            }
                        }
                    }
                }
            }
        }

        private void BtnJoinEvent_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag is Event selectedEvent)
            {
                int currentParticipants = _eventDAO.GetParticipantCount(selectedEvent.EventId);
                if (currentParticipants >= selectedEvent.MaxParticipants)
                {
                    MessageBox.Show("This event has reached its maximum participants.", "Event Full", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (_eventDAO.IsUserRegistered(selectedEvent.EventId, _currentUser.UserId))
                {
                    MessageBox.Show("You have already registered for this event.", "Already Registered", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                var participant = new EventParticipant
                {
                    EventId = selectedEvent.EventId,
                    UserId = _currentUser.UserId,
                    RegistrationDate = DateTime.Now,
                    Status = "Registered"
                };

                _eventDAO.AddEventParticipant(participant);
                SendRegistrationEmail(selectedEvent.EventName, _currentUser.Email);

                MessageBox.Show($"Successfully registered for {selectedEvent.EventName}!\nEmail: {_currentUser.Email}", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                LoadEvents();
                UpdateButtonStates();
            }
        }

        private void SendRegistrationEmail(string eventName, string userEmail)
        {
            try
            {
                var fromAddress = new MailAddress("dungbd07@gmail.com", "Club Management");
                var toAddress = new MailAddress(userEmail);
                const string fromPassword = "qxuu rbkd khoy ubwu";
                const string subject = "Event Registration Confirmation";
                string body = $"Dear {_currentUser.FullName},\n\n" +
                              $"You have successfully registered for the event: {eventName}.\n" +
                              $"Event Date: {Events.FirstOrDefault(e => e.EventName == eventName)?.EventDate.ToString("dd/MM/yyyy")}\n" +
                              "Thank you for joining us!\n\n" +
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
            txtFilter.Text = "";
            dpStartDateFilter.SelectedDate = null;
            dpEndDateFilter.SelectedDate = null;
            LoadEvents();
            UpdateButtonStates();
        }

        private void TxtFilter_TextChanged(object sender, TextChangedEventArgs e)
        {
            FilterEvents();
        }

        private void DpStartDateFilter_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            FilterEvents();
        }

        private void DpEndDateFilter_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            FilterEvents();
        }

        private void FilterEvents()
        {
            try
            {
                var filteredEvents = Events.ToList();

                if (!string.IsNullOrWhiteSpace(txtFilter.Text))
                {
                    filteredEvents = filteredEvents
                        .Where(ev => ev.EventName != null && ev.EventName.ToLower().Contains(txtFilter.Text.ToLower()))
                        .ToList();
                }

                var startDate = dpStartDateFilter.SelectedDate?.Date;
                var endDate = dpEndDateFilter.SelectedDate?.Date;

                if (startDate.HasValue || endDate.HasValue)
                {
                    if (startDate.HasValue && endDate.HasValue && startDate > endDate)
                    {
                        MessageBox.Show("Start date cannot be greater than end date.", "Invalid Date Range", MessageBoxButton.OK, MessageBoxImage.Warning);
                        dgEvents.ItemsSource = new ObservableCollection<Event>();
                        return;
                    }

                    filteredEvents = filteredEvents.Where(ev =>
                    {
                        var eventDate = ev.EventDate.Date;
                        bool isInRange = true;

                        if (startDate.HasValue)
                            isInRange &= eventDate >= startDate.Value;
                        if (endDate.HasValue)
                            isInRange &= eventDate <= endDate.Value;

                        return isInRange;
                    }).ToList();
                }

                dgEvents.ItemsSource = new ObservableCollection<Event>(filteredEvents);
                UpdateButtonStates();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error filtering events: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnUpcomingEvents_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedButton != null)
                _selectedButton.Style = (Style)FindResource("NavbarButtonStyle");
            _selectedButton = (Button)sender;
            _selectedButton.Style = (Style)FindResource("SelectedNavbarButtonStyle");
        }

        private void BtnJoinClubs_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedButton != null)
                _selectedButton.Style = (Style)FindResource("NavbarButtonStyle");
            _selectedButton = (Button)sender;
            _selectedButton.Style = (Style)FindResource("SelectedNavbarButtonStyle");

            // Mở màn hình JoinClubWPF
            JoinClubWPF joinClubWindow = new JoinClubWPF(_currentUser);
            joinClubWindow.Show();
            this.Close(); // Đóng UserEventWPF sau khi mở JoinClubWPF
        }

        private void BtnLogout_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("Are you sure you want to logout?", "Logout", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                SessionDataUser.users.Clear();
                Login loginWindow = new Login();
                loginWindow.Show();
                this.Close();
            }
        }

        private void BtnMyProfile_Click(object sender, RoutedEventArgs e)
        {
            new ProfileWindow().ShowDialog();
        }

        private void BtnMyClubs_Click(object sender, RoutedEventArgs e)
        {
            new MyClub(_currentUser).Show();
            this.Close();
        }

        private void BtnAttendance_Click(object sender, RoutedEventArgs e)
        {
            new AttendanceWPF(SessionDataUser.users[0]).ShowDialog();
        }

        private void BtnViewFeedback_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (sender is Button button && button.Tag is Event selectedEvent)
                {
                    EventFeedbackWPF feedbackWPF = new EventFeedbackWPF(_currentUser, selectedEvent);
                    feedbackWPF.Show();
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error opening feedback: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}