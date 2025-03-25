using ManageClub_PRN212.DAO;
using ManageClub_PRN212.Models;
using ManageClub_PRN212.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Animation;
using static ManageClub_PRN212.Models.User;

namespace ManageClub_PRN212.WPF.President
{
    public partial class ListJoinEvent : Window
    {
        private readonly EventDAO _eventDAO;
        private readonly User _currentUser;
        private ObservableCollection<EventParticipantViewModel> _participants;
        private List<Event> _managedEvents;
        private Button _selectedButton;

        public double SidebarWidth
        {
            get => (double)GetValue(SidebarWidthProperty);
            set => SetValue(SidebarWidthProperty, value);
        }
        public static readonly DependencyProperty SidebarWidthProperty =
            DependencyProperty.Register("SidebarWidth", typeof(double), typeof(ListJoinEvent), new PropertyMetadata(250.0));

        public bool IsSidebarCollapsed
        {
            get => (bool)GetValue(IsSidebarCollapsedProperty);
            set => SetValue(IsSidebarCollapsedProperty, value);
        }
        public static readonly DependencyProperty IsSidebarCollapsedProperty =
            DependencyProperty.Register("IsSidebarCollapsed", typeof(bool), typeof(ListJoinEvent), new PropertyMetadata(false));

        public ListJoinEvent(User currentUser)
        {
            InitializeComponent();
            _eventDAO = new EventDAO();
            _currentUser = currentUser ?? throw new ArgumentNullException(nameof(currentUser));

            SidebarWidth = 250;
            IsSidebarCollapsed = false;

            _selectedButton = BtnJoinListEvent; // Highlight "Join List Event" button by default
            _selectedButton.Style = (Style)FindResource("SelectedSidebarButtonStyle");

            LoadEventsAndParticipants();
        }

        private void BtnEventManagement_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedButton != null)
                _selectedButton.Style = (Style)FindResource("SidebarButtonStyle");
            _selectedButton = (Button)sender;
            _selectedButton.Style = (Style)FindResource("SelectedSidebarButtonStyle");
            new EventWPF(_currentUser).Show();
            Close();
        }

        private void BtnParticipants_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedButton != null)
                _selectedButton.Style = (Style)FindResource("SidebarButtonStyle");
            _selectedButton = (Button)sender;
            _selectedButton.Style = (Style)FindResource("SelectedSidebarButtonStyle");
            new EventParticipantsWPF(_currentUser).Show();
            Close();
        }

        private void BtnJoinListEvent_Click(object sender, RoutedEventArgs e)
        {
            // Already on this page, just update the button style
            if (_selectedButton != null)
                _selectedButton.Style = (Style)FindResource("SidebarButtonStyle");
            _selectedButton = (Button)sender;
            _selectedButton.Style = (Style)FindResource("SelectedSidebarButtonStyle");
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
            if (_selectedButton != null)
                _selectedButton.Style = (Style)FindResource("SidebarButtonStyle");
            _selectedButton = (Button)sender;
            _selectedButton.Style = (Style)FindResource("SelectedSidebarButtonStyle");

            var result = MessageBox.Show("Are you sure you want to logout?", "Logout", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                SessionDataUser.users.Clear();
                new Login().Show();
                this.Close();
            }
        }

        private void LoadEventsAndParticipants()
        {
            try
            {
                _managedEvents = _eventDAO.GetEventsByUser(_currentUser.UserId);

                if (_managedEvents != null && _managedEvents.Any())
                {
                    if (_managedEvents.Count > 1)
                    {
                        EventButtonsPanel.Visibility = Visibility.Visible;
                        foreach (var ev in _managedEvents)
                        {
                            var button = new Button
                            {
                                Content = ev.EventName,
                                Style = (Style)FindResource("ContentButtonStyle"),
                                Tag = ev.EventId,
                                Margin = new Thickness(5, 0, 0, 0)
                            };
                            button.Click += EventButton_Click;
                            EventButtonsPanel.Children.Add(button);
                        }
                        LoadParticipantsForEvent(_managedEvents[0].EventId);
                    }
                    else if (_managedEvents.Count == 1)
                    {
                        LoadParticipantsForEvent(_managedEvents[0].EventId);
                    }
                }
                else
                {
                    MessageBox.Show("You are not managing any events.", "No Events", MessageBoxButton.OK, MessageBoxImage.Information);
                    dgParticipants.ItemsSource = new ObservableCollection<EventParticipantViewModel>();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading events: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void LoadParticipantsForEvent(int eventId)
        {
            try
            {
                var participants = _eventDAO.GetAllEventParticipants()
                    .Where(ep => ep.EventId == eventId && ep.Status == "Accepted")
                    .Select((ep, index) => new EventParticipantViewModel
                    {
                        Index = index + 1,
                        EventParticipantId = ep.EventParticipantId,
                        User = ep.User,
                        Event = ep.Event,
                        RegistrationDate = ep.RegistrationDate
                    })
                    .ToList();

                _participants = new ObservableCollection<EventParticipantViewModel>(participants);
                dgParticipants.ItemsSource = _participants;
                FilterParticipants(); // Apply initial filters if any
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading participants: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void EventButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.Tag is int eventId)
            {
                LoadParticipantsForEvent(eventId);
            }
        }

        private void UserName_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (sender is TextBlock textBlock && textBlock.DataContext is EventParticipantViewModel participant)
            {
                var user = participant.User;
                var userDetails = new Window
                {
                    Title = $"User Details - {user.FullName}",
                    Width = 300,
                    Height = 250,
                    WindowStartupLocation = WindowStartupLocation.CenterScreen,
                    Content = new StackPanel
                    {
                        Margin = new Thickness(10),
                        Children =
                        {
                            new TextBlock { Text = $"User ID: {user.UserId}" },
                            new TextBlock { Text = $"Full Name: {user.FullName}" },
                            new TextBlock { Text = $"Email: {user.Email}" },
                            new TextBlock { Text = $"Role ID: {user.RoleId}" },
                            new TextBlock { Text = $"Date of Birth: {user.DateOfBirth?.ToString("d") ?? "N/A"}" },
                            new TextBlock { Text = $"Phone Number: {user.PhoneNumber ?? "N/A"}" },
                            new TextBlock { Text = $"Address: {user.Address ?? "N/A"}" }
                        }
                    }
                };
                userDetails.ShowDialog();
            }
        }

        private void EventName_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (sender is TextBlock textBlock && textBlock.DataContext is EventParticipantViewModel participant)
            {
                var ev = participant.Event;
                var eventDetails = new Window
                {
                    Title = $"Event Details - {ev.EventName}",
                    Width = 400,
                    Height = 300,
                    WindowStartupLocation = WindowStartupLocation.CenterScreen,
                    Content = new StackPanel
                    {
                        Margin = new Thickness(10),
                        Children =
                        {
                            new TextBlock { Text = $"Event ID: {ev.EventId}" },
                            new TextBlock { Text = $"Event Name: {ev.EventName}" },
                            new TextBlock { Text = $"Description: {ev.Description ?? "N/A"}" },
                            new TextBlock { Text = $"Date: {ev.EventDate.ToString("d")}" },
                            new TextBlock { Text = $"Location: {ev.Location}" },
                            new TextBlock { Text = $"Organizer: {ev.Organizer.FullName}" },
                            new TextBlock { Text = $"Club: {ev.Club?.ClubName ?? "N/A"}" },
                            new TextBlock { Text = $"Max Participants: {ev.MaxParticipants?.ToString() ?? "N/A"}" }
                        }
                    }
                };
                eventDetails.ShowDialog();
            }
        }

        private void TxtEventFilter_TextChanged(object sender, TextChangedEventArgs e)
        {
            FilterParticipants();
        }

        private void DpStartDateFilter_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            FilterParticipants();
        }

        private void DpEndDateFilter_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            FilterParticipants();
        }

        private void FilterParticipants()
        {
            try
            {
                if (_participants == null || !_participants.Any())
                {
                    dgParticipants.ItemsSource = new ObservableCollection<EventParticipantViewModel>();
                    return;
                }

                var filteredParticipants = _participants.ToList();

                // Filter by Event Name
                if (!string.IsNullOrWhiteSpace(txtEventFilter.Text))
                {
                    filteredParticipants = filteredParticipants
                        .Where(p => p.Event != null && p.Event.EventName != null && p.Event.EventName.ToLower().Contains(txtEventFilter.Text.ToLower()))
                        .ToList();
                }

                // Filter by Registration Date Range
                var startDate = dpStartDateFilter.SelectedDate?.Date;
                var endDate = dpEndDateFilter.SelectedDate?.Date;

                if (startDate.HasValue || endDate.HasValue)
                {
                    if (startDate.HasValue && endDate.HasValue && startDate > endDate)
                    {
                        MessageBox.Show("Start date cannot be greater than end date.", "Invalid Date Range", MessageBoxButton.OK, MessageBoxImage.Warning);
                        dgParticipants.ItemsSource = new ObservableCollection<EventParticipantViewModel>();
                        return;
                    }

                    filteredParticipants = filteredParticipants
                        .Where(p =>
                        {
                            if (!p.RegistrationDate.HasValue) return false; // Skip if RegistrationDate is null
                            var regDate = p.RegistrationDate.Value.Date;
                            bool isInRange = true;

                            if (startDate.HasValue)
                                isInRange &= regDate >= startDate.Value;
                            if (endDate.HasValue)
                                isInRange &= regDate <= endDate.Value;

                            return isInRange;
                        })
                        .ToList();
                }

                // Update Index after filtering
                filteredParticipants = filteredParticipants
                    .Select((p, index) => new EventParticipantViewModel
                    {
                        Index = index + 1,
                        EventParticipantId = p.EventParticipantId,
                        User = p.User,
                        Event = p.Event,
                        RegistrationDate = p.RegistrationDate
                    })
                    .ToList();

                dgParticipants.ItemsSource = new ObservableCollection<EventParticipantViewModel>(filteredParticipants);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error filtering participants: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnClearFilter_Click(object sender, RoutedEventArgs e)
        {
            txtEventFilter.Text = "";
            dpStartDateFilter.SelectedDate = null;
            dpEndDateFilter.SelectedDate = null;
            FilterParticipants();
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
        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void BtnClubMemberManagement_Click(object sender, RoutedEventArgs e)
        {
            new ClubMemberManagement(_currentUser).Show();
            this.Close();
        }

        private void BtnAttendance_Click(object sender, RoutedEventArgs e)
        {
            new AttendanceWPF(SessionDataUser.users[0]).ShowDialog();
        }
    }

   

   
    
}