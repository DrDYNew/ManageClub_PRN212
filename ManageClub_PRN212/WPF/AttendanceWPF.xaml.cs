using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using ManageClub_PRN212.DAO;
using ManageClub_PRN212.Models;
using ManageClub_PRN212.ViewModels;
using ManageClub_PRN212.WPF.Member;
using ManageClub_PRN212.WPF.President;

namespace ManageClub_PRN212.WPF
{
    /// <summary>
    /// Interaction logic for AttendanceWPF.xaml
    /// </summary>
    public partial class AttendanceWPF : Window
    {
        private readonly EventDAO _eventDAO;
        private readonly AttendanceDAO _attendanceDAO;
        private readonly User _currentUser;
        private List<Event> _events;
        private List<AttendanceViewModel> _attendanceViewModels;
        private Event _selectedEvent;

        public AttendanceWPF (User currentUser)
        {
            InitializeComponent();
            _eventDAO = new EventDAO();
            _attendanceDAO = new AttendanceDAO();
            _currentUser = currentUser;

            // Set user information in footer
            CurrentUserText.Text = _currentUser.FullName;
            CurrentRoleText.Text = _currentUser.Role.RoleName;

            // Load events and set up UI based on user role
            LoadEvents();
            SetupUIByRole();
        }

        private void LoadEvents()
        {
            try
            {
                // If current user is Admin or Club President, load all events
                if (_currentUser.RoleId == 1 || _currentUser.RoleId == 2)
                {
                    _events = _eventDAO.GetAllEvents();
                    
                    // For Club President, check if there are any events for today
                    if (_currentUser.RoleId == 2)
                    {
                        DateTime today = DateTime.Today;
                        bool hasTodayEvents = _events.Any(e => e.EventDate.Date == today);
                    }
                }
                // If current user is Member, load only events they're participating in
                else if (_currentUser.RoleId == 3)
                {
                    _events = _eventDAO.GetEventsByUser(_currentUser.UserId);
                }
                
                EventComboBox.ItemsSource = _events;
                EventComboBox.DisplayMemberPath = "EventName";
                EventComboBox.SelectedValuePath = "EventId";

                if (_events.Count > 0)
                {
                    EventComboBox.SelectedIndex = 0;
                }
                else
                {
                    MessageBox.Show("No events available.");
                }

                // Load attendance history for the current user
                LoadAttendanceHistory();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading events: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void SetupUIByRole()
        {
            // Admin and Club President can manage attendance
            bool isAttendanceManager = _currentUser.RoleId == 1 || _currentUser.RoleId == 2;
            
            // Set visibility of check-in/check-out buttons based on role
            CheckInButton.Visibility = isAttendanceManager ? Visibility.Visible : Visibility.Collapsed;
            CheckOutButton.Visibility = isAttendanceManager ? Visibility.Visible : Visibility.Collapsed;
            
            // For members, hide the Event Attendance tab and show only Attendance History
            if (_currentUser.RoleId == 3) // Member
            {
                EventAttendanceTab.Visibility = Visibility.Collapsed;
                AttendanceHistoryTab.IsSelected = true;
            }
            
            // For Club Presidents, show a message about attendance policy
            if (_currentUser.RoleId == 2)
            {
                CheckInButton.IsEnabled = false;
                CheckOutButton.IsEnabled = false;
            }
        }

        private void LoadEventAttendance()
        {
            if (_selectedEvent == null)
            {
                return;
            }

            try
            {
                // Get all attendance records for the selected event
                var attendances = _attendanceDAO.GetAttendanceByEvent(_selectedEvent.EventId);
                
                // Get all participants for this event
                var participants = _eventDAO.GetUsersByEvent(_selectedEvent.EventId);
                
                _attendanceViewModels = new List<AttendanceViewModel>();
                
                // For Club Presidents, check if the event is for today
                bool isToday = true; // Default for Admin and other roles
                if (_currentUser.RoleId == 2) // Club President
                {
                    isToday = _selectedEvent.EventDate.Date == DateTime.Today;
                }
                
                // Create view models for existing attendance records
                foreach (var attendance in attendances)
                {
                    var viewModel = new AttendanceViewModel
                    {
                        AttendanceId = attendance.AttendanceId,
                        EventId = attendance.EventId,
                        UserId = attendance.UserId,
                        User = attendance.User,
                        CheckInTime = attendance.CheckInTime,
                        CheckOutTime = attendance.CheckOutTime
                    };
                    
                    // Set visibility for action buttons based on attendance status and event date
                    if (_currentUser.RoleId == 2 && !isToday)
                    {
                        // For Club Presidents, hide all action buttons if event is not today
                        viewModel.CheckInVisibility = Visibility.Collapsed;
                        viewModel.CheckOutVisibility = Visibility.Collapsed;
                        
                        // Add a note to the status if it's not today's event
                        if (attendance.CheckInTime == null)
                        {
                            viewModel.Status = "Not Checked In (Past/Future Event)";
                        }
                        else if (attendance.CheckOutTime == null)
                        {
                            viewModel.Status = "Checked In (Past/Future Event)";
                        }
                        else
                        {
                            viewModel.Status = "Completed (Past/Future Event)";
                        }
                    }
                    else
                    {
                        // Normal visibility rules for today's events or admin users
                        viewModel.CheckInVisibility = attendance.CheckInTime == null ? Visibility.Visible : Visibility.Collapsed;
                        viewModel.CheckOutVisibility = attendance.CheckInTime != null && attendance.CheckOutTime == null ? Visibility.Visible : Visibility.Collapsed;
                        
                        // Set status text
                        if (attendance.CheckInTime == null)
                        {
                            viewModel.Status = "Not Checked In";
                        }
                        else if (attendance.CheckOutTime == null)
                        {
                            viewModel.Status = "Checked In";
                        }
                        else
                        {
                            viewModel.Status = "Completed";
                        }
                    }
                    
                    _attendanceViewModels.Add(viewModel);
                }
                
                // Add participants who don't have attendance records yet
                foreach (var participant in participants)
                {
                    if (!_attendanceViewModels.Any(a => a.UserId == participant.UserId))
                    {
                        var viewModel = new AttendanceViewModel
                        {
                            EventId = _selectedEvent.EventId,
                            UserId = participant.UserId,
                            User = participant
                        };
                        
                        // Set visibility and status based on whether the event is today (for Club Presidents)
                        if (_currentUser.RoleId == 2 && !isToday)
                        {
                            viewModel.Status = "Not Checked In (Past/Future Event)";
                            viewModel.CheckInVisibility = Visibility.Collapsed;
                            viewModel.CheckOutVisibility = Visibility.Collapsed;
                        }
                        else
                        {
                            viewModel.Status = "Not Checked In";
                            viewModel.CheckInVisibility = Visibility.Visible;
                            viewModel.CheckOutVisibility = Visibility.Collapsed;
                        }
                        
                        _attendanceViewModels.Add(viewModel);
                    }
                }
                
                // Update the UI
                AttendanceDataGrid.ItemsSource = _attendanceViewModels;
                
                // Only admins and club presidents can see action buttons
                bool isAttendanceManager = _currentUser.RoleId == 1 || _currentUser.RoleId == 2;
                AttendanceDataGrid.Columns[5].Visibility = isAttendanceManager ? Visibility.Visible : Visibility.Collapsed;
                
                // For Club Presidents, update buttons based on whether event is today
                if (_currentUser.RoleId == 2)
                {
                    CheckInButton.IsEnabled = isToday;
                    CheckOutButton.IsEnabled = isToday;
                    
                    if (!isToday)
                    {
                        // Show message for past/future events
                        string eventDateStr = _selectedEvent.EventDate.ToString("dd/MM/yyyy");
                        string message = DateTime.Today > _selectedEvent.EventDate.Date 
                            ? $"This is a past event (dated {eventDateStr}). Attendance can only be managed for today's events."
                            : $"This is a future event (dated {eventDateStr}). Attendance can only be managed for today's events.";
                        
                        MessageBox.Show(message, "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading attendance data: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void LoadAttendanceHistory()
        {
            try
            {
                List<Attendance> attendanceHistory;
                
                // If admin or club president, show all attendance records
                if (_currentUser.RoleId == 1 || _currentUser.RoleId == 2)
                {
                    if (_selectedEvent != null)
                    {
                        // Show attendance for currently selected event
                        attendanceHistory = _attendanceDAO.GetAttendanceByEvent(_selectedEvent.EventId);
                    }
                    else
                    {
                        // No event selected yet, show empty list
                        attendanceHistory = new List<Attendance>();
                    }
                }
                else
                {
                    // For regular members, show only their attendance history
                    attendanceHistory = _attendanceDAO.GetAttendanceByUser(_currentUser.UserId);
                }
                
                HistoryDataGrid.ItemsSource = attendanceHistory;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading attendance history: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void EventComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (EventComboBox.SelectedItem is Event selectedEvent)
            {
                _selectedEvent = selectedEvent;
                LoadEventAttendance();
                
                // Also update the history tab if current user is admin or club president
                if (_currentUser.RoleId == 1 || _currentUser.RoleId == 2)
                {
                    LoadAttendanceHistory();
                }
                
                // For Club President, validate if the event is scheduled for today
                if (_currentUser.RoleId == 2)
                {
                    DateTime today = DateTime.Today;
                    if (_selectedEvent.EventDate.Date != today)
                    {
                        MessageBox.Show("This event is not scheduled for today. Attendance can only be managed for today's events.", 
                            "Information", MessageBoxButton.OK, MessageBoxImage.Warning);
                        CheckInButton.IsEnabled = false;
                        CheckOutButton.IsEnabled = false;
                    }
                }
            }
        }

        private void AttendanceDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // This method can be used to update UI based on selection
            // For example, enabling/disabling check-in/check-out buttons
            
            if (AttendanceDataGrid.SelectedItem is AttendanceViewModel selectedAttendance)
            {
                bool canCheckIn = selectedAttendance.CheckInTime == null;
                bool canCheckOut = selectedAttendance.CheckInTime != null && selectedAttendance.CheckOutTime == null;
                
                CheckInButton.IsEnabled = canCheckIn;
                CheckOutButton.IsEnabled = canCheckOut;
            }
            else
            {
                CheckInButton.IsEnabled = false;
                CheckOutButton.IsEnabled = false;
            }
        }

        private void CheckInButton_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedEvent == null)
            {
                MessageBox.Show("Please select an event first.", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            
            if (AttendanceDataGrid.SelectedItem is AttendanceViewModel selectedAttendance)
            {
                PerformCheckIn(selectedAttendance.EventId, selectedAttendance.UserId);
            }
            else
            {
                MessageBox.Show("Please select a user to check in.", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void CheckOutButton_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedEvent == null)
            {
                MessageBox.Show("Please select an event first.", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            
            if (AttendanceDataGrid.SelectedItem is AttendanceViewModel selectedAttendance)
            {
                PerformCheckOut(selectedAttendance.EventId, selectedAttendance.UserId);
            }
            else
            {
                MessageBox.Show("Please select a user to check out.", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void Row_CheckIn(object sender, RoutedEventArgs e)
        {
            if (((Button)sender).DataContext is AttendanceViewModel attendance)
            {
                // For Club Presidents, verify that the event is for today
                if (_currentUser.RoleId == 2)
                {
                    Event eventToCheck = _events.FirstOrDefault(ev => ev.EventId == attendance.EventId);
                    if (eventToCheck != null && eventToCheck.EventDate.Date != DateTime.Today)
                    {
                        MessageBox.Show($"Cannot check in for this event. Attendance can only be managed for today's events. " +
                            $"This event is scheduled for {eventToCheck.EventDate.ToString("dd/MM/yyyy")}", 
                            "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }
                }
                
                PerformCheckIn(attendance.EventId, attendance.UserId);
            }
        }

        private void Row_CheckOut(object sender, RoutedEventArgs e)
        {
            if (((Button)sender).DataContext is AttendanceViewModel attendance)
            {
                // For Club Presidents, verify that the event is for today
                if (_currentUser.RoleId == 2)
                {
                    Event eventToCheck = _events.FirstOrDefault(ev => ev.EventId == attendance.EventId);
                    if (eventToCheck != null && eventToCheck.EventDate.Date != DateTime.Today)
                    {
                        MessageBox.Show($"Cannot check out for this event. Attendance can only be managed for today's events. " +
                            $"This event is scheduled for {eventToCheck.EventDate.ToString("dd/MM/yyyy")}", 
                            "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }
                }
                
                PerformCheckOut(attendance.EventId, attendance.UserId);
            }
        }

        private void PerformCheckIn(int eventId, int userId)
        {
            try
            {
                // Get the event to check its date
                Event eventToCheck = _events.FirstOrDefault(e => e.EventId == eventId);
                
                // For Club President, validate if the event is scheduled for today
                if (_currentUser.RoleId == 2 && eventToCheck != null)
                {
                    DateTime today = DateTime.Today;
                    if (eventToCheck.EventDate.Date != today)
                    {
                        MessageBox.Show($"Cannot check in for this event. Attendance can only be managed for today's events. " +
                            $"This event is scheduled for {eventToCheck.EventDate.ToString("dd/MM/yyyy")}", 
                            "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }
                }
                
                bool result = _attendanceDAO.RecordCheckIn(eventId, userId);
                
                if (result)
                {
                    MessageBox.Show("Check-in recorded successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    // Reload attendance data to reflect changes
                    LoadEventAttendance();
                    LoadAttendanceHistory();
                }
                else
                {
                    MessageBox.Show("User already checked in or an error occurred.", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error during check-in: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void PerformCheckOut(int eventId, int userId)
        {
            try
            {
                // Get the event to check its date
                Event eventToCheck = _events.FirstOrDefault(e => e.EventId == eventId);
                
                // For Club President, validate if the event is scheduled for today
                if (_currentUser.RoleId == 2 && eventToCheck != null)
                {
                    DateTime today = DateTime.Today;
                    if (eventToCheck.EventDate.Date != today)
                    {
                        MessageBox.Show($"Cannot check out for this event. Attendance can only be managed for today's events. " +
                            $"This event is scheduled for {eventToCheck.EventDate.ToString("dd/MM/yyyy")}", 
                            "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }
                }
                
                bool result = _attendanceDAO.RecordCheckOut(eventId, userId);
                
                if (result)
                {
                    MessageBox.Show("Check-out recorded successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    // Reload attendance data to reflect changes
                    LoadEventAttendance();
                    LoadAttendanceHistory();
                }
                else
                {
                    MessageBox.Show("User needs to check in first or an error occurred.", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error during check-out: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
} 