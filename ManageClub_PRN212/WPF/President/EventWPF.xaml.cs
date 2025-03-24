using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using ManageClub_PRN212.DAO;
using ManageClub_PRN212.Models;

namespace ManageClub_PRN212.WPF.President
{
    public partial class EventWPF : Window
    {
        private readonly EventDAO _eventDAO;
        private readonly User _currentUser;
        public ObservableCollection<Event> Events { get; set; }
        public ObservableCollection<User> Users { get; set; }
        public ObservableCollection<Club> Clubs { get; set; }
        private Event selectedEvent;
        private Button _selectedButton;

        public double SidebarWidth
        {
            get => (double)GetValue(SidebarWidthProperty);
            set => SetValue(SidebarWidthProperty, value);
        }
        public static readonly DependencyProperty SidebarWidthProperty =
            DependencyProperty.Register("SidebarWidth", typeof(double), typeof(EventWPF), new PropertyMetadata(250.0));

        public bool IsSidebarCollapsed
        {
            get => (bool)GetValue(IsSidebarCollapsedProperty);
            set => SetValue(IsSidebarCollapsedProperty, value);
        }
        public static readonly DependencyProperty IsSidebarCollapsedProperty =
            DependencyProperty.Register("IsSidebarCollapsed", typeof(bool), typeof(EventWPF), new PropertyMetadata(false));

        public EventWPF(User currentUser)
        {
            InitializeComponent();
            _eventDAO = new EventDAO();
            _currentUser = currentUser ?? throw new ArgumentNullException(nameof(currentUser));

            Clubs = new ObservableCollection<Club>(_eventDAO.GetClubsManagedByUser(_currentUser.UserId));
            Users = new ObservableCollection<User> { _currentUser };
            cmbOrganizer.ItemsSource = Users;
            cmbOrganizer.SelectedItem = _currentUser;
            cmbOrganizer.IsEnabled = false;

            LoadEvents();

            cmbClubs.ItemsSource = Clubs;
            cmbClubFilter.ItemsSource = Clubs;
            dgEvents.ItemsSource = Events;

            dgEvents.SelectionChanged += DgEvents_SelectionChanged;
            dpStartDateFilter.SelectedDateChanged += DpStartDateFilter_SelectedDateChanged;
            dpEndDateFilter.SelectedDateChanged += DpEndDateFilter_SelectedDateChanged;

            _selectedButton = BtnEventManagement;
            _selectedButton.Style = (Style)FindResource("SelectedSidebarButtonStyle");
            SidebarWidth = 250;
            IsSidebarCollapsed = false;
        }

        private void BtnToggleSidebar_Click(object sender, RoutedEventArgs e)
        {
            ToggleSidebar();
        }

        private void ToggleSidebar()
        {
            var border = (Border)FindName("sidebarBorder");
            if (border != null)
            {
                DoubleAnimation animation = new DoubleAnimation
                {
                    Duration = TimeSpan.FromSeconds(0.3)
                };

                if (!IsSidebarCollapsed)
                {
                    animation.From = 250;
                    animation.To = 50;
                    IsSidebarCollapsed = true;
                }
                else
                {
                    animation.From = 50;
                    animation.To = 250;
                    IsSidebarCollapsed = false;
                }

                border.BeginAnimation(Border.WidthProperty, animation);
                SidebarWidth = animation.To.Value;
            }
        }

        private void BtnEventManagement_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedButton != null)
                _selectedButton.Style = (Style)FindResource("SidebarButtonStyle");
            _selectedButton = (Button)sender;
            _selectedButton.Style = (Style)FindResource("SelectedSidebarButtonStyle");
        }

        private void BtnParticipants_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedButton != null)
                _selectedButton.Style = (Style)FindResource("SidebarButtonStyle");
            _selectedButton = (Button)sender;
            _selectedButton.Style = (Style)FindResource("SelectedSidebarButtonStyle");
            new EventParticipantsWPF(_currentUser).Show();
            this.Close();
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

        private void BtnManageClubFinance_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedButton != null)
                _selectedButton.Style = (Style)FindResource("SidebarButtonStyle");
            _selectedButton = (Button)sender;
            _selectedButton.Style = (Style)FindResource("SelectedSidebarButtonStyle");
            new ManageClubFinance(_currentUser).Show();
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
            if (_selectedButton != null)
                _selectedButton.Style = (Style)FindResource("SidebarButtonStyle");
            _selectedButton = (Button)sender;
            _selectedButton.Style = (Style)FindResource("SelectedSidebarButtonStyle");

            var result = MessageBox.Show("Are you sure you want to logout?", "Logout", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                Login loginWindow = new Login();
                loginWindow.Show();
                this.Close();
            }
        }

        private void LoadEvents()
        {
            try
            {
                var events = _eventDAO.GetEventsByUser(_currentUser.UserId);

                foreach (var ev in events)
                {
                    if (ev.EventDate.Date == DateTime.Today && ev.EventStatus != "Processing")
                    {
                        ev.EventStatus = "Processing";
                        _eventDAO.UpdateEvent(ev); 
                    }
                }

                Events = new ObservableCollection<Event>(events);
                dgEvents.ItemsSource = Events;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading events: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void DgEvents_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dgEvents.SelectedItem is Event selected)
            {
                selectedEvent = selected;
                txtEventName.Text = selected.EventName;
                txtDescription.Text = selected.Description;
                dpEventDate.SelectedDate = selected.EventDate;
                txtLocation.Text = selected.Location;
                cmbOrganizer.SelectedItem = Users.FirstOrDefault(u => u.UserId == selected.OrganizerId);
                cmbClubs.SelectedItem = Clubs.FirstOrDefault(c => c.ClubId == selected.ClubId);
                txtMaxParticipants.Text = selected.MaxParticipants?.ToString() ?? "";
            }
            else
            {
                ClearInputFields();
                selectedEvent = null;
            }
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtEventName.Text) || dpEventDate.SelectedDate == null ||
                string.IsNullOrWhiteSpace(txtLocation.Text) || cmbClubs.SelectedItem == null ||
                !int.TryParse(txtMaxParticipants.Text, out int maxParticipants))
            {
                MessageBox.Show("Please fill in all required fields with valid data.", "Invalid Input", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Kiểm tra EventDate phải lớn hơn hoặc bằng ngày hiện tại
            if (dpEventDate.SelectedDate.Value.Date < DateTime.Today)
            {
                MessageBox.Show("Event date cannot be in the past!", "Invalid Date", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var newEvent = new Event
            {
                EventName = txtEventName.Text,
                Description = txtDescription.Text,
                EventDate = dpEventDate.SelectedDate.Value,
                Location = txtLocation.Text,
                OrganizerId = _currentUser.UserId,
                ClubId = (cmbClubs.SelectedItem as Club)?.ClubId,
                MaxParticipants = maxParticipants,
                EventStatus = dpEventDate.SelectedDate.Value.Date == DateTime.Today ? "Processing" : "Coming soon" // Gán trạng thái dựa trên ngày
            };

            _eventDAO.AddEvent(newEvent);
            LoadEvents();
            ClearInputFields();
            MessageBox.Show("Event added successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            if (selectedEvent == null)
            {
                MessageBox.Show("Please select an event to edit.", "No Selection", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(txtEventName.Text) || dpEventDate.SelectedDate == null ||
                string.IsNullOrWhiteSpace(txtLocation.Text) || cmbClubs.SelectedItem == null ||
                !int.TryParse(txtMaxParticipants.Text, out int maxParticipants))
            {
                MessageBox.Show("Please fill in all required fields with valid data.", "Invalid Input", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Kiểm tra EventDate phải lớn hơn hoặc bằng ngày hiện tại
            if (dpEventDate.SelectedDate.Value.Date < DateTime.Today)
            {
                MessageBox.Show("Event date cannot be in the past!", "Invalid Date", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Cập nhật thông tin
            selectedEvent.EventName = txtEventName.Text;
            selectedEvent.Description = txtDescription.Text;
            selectedEvent.EventDate = dpEventDate.SelectedDate.Value;
            selectedEvent.Location = txtLocation.Text;
            selectedEvent.OrganizerId = _currentUser.UserId;
            selectedEvent.ClubId = (cmbClubs.SelectedItem as Club)?.ClubId;
            selectedEvent.MaxParticipants = maxParticipants;
            // Cập nhật trạng thái nếu ngày hiện tại bằng EventDate
            if (selectedEvent.EventDate.Date == DateTime.Today)
            {
                selectedEvent.EventStatus = "Processing";
            }

            _eventDAO.UpdateEvent(selectedEvent);
            LoadEvents();
            ClearInputFields();
            selectedEvent = null;
            dgEvents.SelectedItem = null;
            MessageBox.Show("Event updated successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (selectedEvent == null)
            {
                MessageBox.Show("Please select an event to delete.", "No Selection", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                var participants = _eventDAO.GetAllEventParticipants()
                    .Where(ep => ep.EventId == selectedEvent.EventId)
                    .ToList();

                if (participants.Any())
                {
                    MessageBox.Show("Cannot delete this event because there are participants registered for it.",
                        "Delete Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                var result = MessageBox.Show("Are you sure you want to delete this event?",
                    "Confirm Delete", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    _eventDAO.DeleteEvent(selectedEvent.EventId);
                    LoadEvents();
                    ClearInputFields();
                    selectedEvent = null;
                    dgEvents.SelectedItem = null;
                    MessageBox.Show("Event deleted successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error deleting event: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void BtnClearFilter_Click(object sender, RoutedEventArgs e)
        {
            txtFilter.Text = "";
            cmbClubFilter.SelectedIndex = -1;
            dpStartDateFilter.SelectedDate = null;
            dpEndDateFilter.SelectedDate = null;
            LoadEvents();
        }

        private void TxtFilter_TextChanged(object sender, TextChangedEventArgs e)
        {
            FilterEvents();
        }

        private void CmbClubFilter_SelectionChanged(object sender, SelectionChangedEventArgs e)
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
                if (Events == null || !Events.Any())
                {
                    dgEvents.ItemsSource = new ObservableCollection<Event>();
                    return;
                }

                var filteredEvents = Events.ToList();

                if (!string.IsNullOrWhiteSpace(txtFilter.Text))
                {
                    filteredEvents = filteredEvents
                        .Where(ev => ev.EventName != null && ev.EventName.ToLower().Contains(txtFilter.Text.ToLower()))
                        .ToList();
                }

                if (cmbClubFilter.SelectedItem != null)
                {
                    var selectedClub = cmbClubFilter.SelectedItem as Club;
                    filteredEvents = filteredEvents
                        .Where(ev => ev.ClubId == selectedClub.ClubId)
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
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error filtering events: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ClearInputFields()
        {
            txtEventName.Text = "";
            txtDescription.Text = "";
            dpEventDate.SelectedDate = null;
            txtLocation.Text = "";
            cmbOrganizer.SelectedItem = _currentUser;
            cmbClubs.SelectedIndex = -1;
            txtMaxParticipants.Text = "";
        }
    }
}