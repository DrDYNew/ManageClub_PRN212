using ManageClub_PRN212.DAO;
using ManageClub_PRN212.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Animation;
using ManageClub_PRN212.ViewModels;
using static ManageClub_PRN212.Models.User;

namespace ManageClub_PRN212.WPF.President
{
    public partial class ListMemberJoinClub : Window
    {
        private readonly ClubDAO _clubDAO;
        private readonly ClubMemberDAO _memberDAO;
        private readonly User _currentUser;
        private ObservableCollection<ClubMemberViewModel> _members;
        private List<Club> _managedClubs;
        private Button _selectedButton;
        private bool _isWindowClosing; // Flag to track window closure

        public double SidebarWidth
        {
            get => (double)GetValue(SidebarWidthProperty);
            set => SetValue(SidebarWidthProperty, value);
        }
        public static readonly DependencyProperty SidebarWidthProperty =
            DependencyProperty.Register("SidebarWidth", typeof(double), typeof(ListMemberJoinClub), new PropertyMetadata(250.0));

        public bool IsSidebarCollapsed
        {
            get => (bool)GetValue(IsSidebarCollapsedProperty);
            set => SetValue(IsSidebarCollapsedProperty, value);
        }
        public static readonly DependencyProperty IsSidebarCollapsedProperty =
            DependencyProperty.Register("IsSidebarCollapsed", typeof(bool), typeof(ListMemberJoinClub), new PropertyMetadata(false));

        public ListMemberJoinClub(User currentUser)
        {
            _clubDAO = new ClubDAO();
            _memberDAO = new ClubMemberDAO();
            try
            {
                InitializeComponent();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error initializing UI: {ex.Message}\nStack Trace: {ex.StackTrace}", "Initialization Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Ensure UI elements are initialized
            if (dgMembers == null || txtMemberFilter == null)
            {
                MessageBox.Show("One or more UI elements failed to initialize. Please check the XAML.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            _clubDAO = new ClubDAO();
            _currentUser = currentUser ?? throw new ArgumentNullException(nameof(currentUser));
            _isWindowClosing = false;

            SidebarWidth = 250;
            IsSidebarCollapsed = false;

            _selectedButton = BtnMemberJoinClub;
            _selectedButton.Style = (Style)FindResource("SelectedSidebarButtonStyle");

            LoadClubsAndMembers();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("Window loaded");
            System.Diagnostics.Debug.WriteLine($"txtMemberFilter.IsEnabled: {txtMemberFilter.IsEnabled}");
        }

        private void UnregisterEventHandlers()
        {
            _isWindowClosing = true; // Set the flag to prevent event handlers from executing

            // Unregister event handlers to prevent them from firing after the window is closed
            if (txtMemberFilter != null)
                txtMemberFilter.TextChanged -= TxtMemberFilter_TextChanged;

            // Disable UI elements to prevent further interactions
            if (txtMemberFilter != null)
                txtMemberFilter.IsEnabled = false;
            if (dgMembers != null)
                dgMembers.IsEnabled = false;
            if (btnClearFilter != null)
                btnClearFilter.IsEnabled = false;
            if (btnClose != null)
                btnClose.IsEnabled = false;
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

            UnregisterEventHandlers(); // Unregister handlers before closing
            new EventWPF(_currentUser).Show();
            Close();
        }

        private void BtnParticipants_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedButton != null)
                _selectedButton.Style = (Style)FindResource("SidebarButtonStyle");
            _selectedButton = (Button)sender;
            _selectedButton.Style = (Style)FindResource("SelectedSidebarButtonStyle");

            UnregisterEventHandlers(); // Unregister handlers before closing
            new EventParticipantsWPF(_currentUser).Show();
            Close();
        }

        private void BtnJoinListEvent_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedButton != null)
                _selectedButton.Style = (Style)FindResource("SidebarButtonStyle");
            _selectedButton = (Button)sender;
            _selectedButton.Style = (Style)FindResource("SelectedSidebarButtonStyle");

            UnregisterEventHandlers(); // Unregister handlers before closing
            new ListJoinEvent(_currentUser).Show();
            Close();
        }

        private void BtnMembers_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedButton != null)
                _selectedButton.Style = (Style)FindResource("SidebarButtonStyle");
            _selectedButton = (Button)sender;
            _selectedButton.Style = (Style)FindResource("SelectedSidebarButtonStyle");
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
                UnregisterEventHandlers(); // Unregister handlers before closing
                new Login().Show();
                Close();
            }
        }

        private void LoadClubsAndMembers()
        {
            try
            {
                _managedClubs = _clubDAO.GetClubsByPresident(_currentUser.UserId);

                if (_managedClubs == null || !_managedClubs.Any())
                {
                    MessageBox.Show("You are not managing any clubs.", "No Clubs", MessageBoxButton.OK, MessageBoxImage.Information);
                    dgMembers.ItemsSource = new ObservableCollection<ClubMemberViewModel>();
                    return;
                }

                // Log the number of clubs loaded
                System.Diagnostics.Debug.WriteLine($"Loaded {_managedClubs.Count} clubs for user {_currentUser.UserId}");

                // Load members for all clubs
                LoadMembersForAllClubs();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading clubs: {ex.Message}\nStack Trace: {ex.StackTrace}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void LoadMembersForAllClubs()
        {
            try
            {
                var allClubMembers = _memberDAO.GetAllClubMembers();
                if (allClubMembers == null || !allClubMembers.Any())
                {
                    MessageBox.Show("No club members found.", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                    _members = new ObservableCollection<ClubMemberViewModel>();
                    dgMembers.ItemsSource = _members;
                    return;
                }

                var members = allClubMembers
                    .Where(cm => cm != null && _managedClubs.Any(c => c.ClubId == cm.ClubId))
                    .OrderBy(cm => (cm.MemberStatus ?? "Unknown") != "Pending")
                    .Select((cm, index) => new ClubMemberViewModel
                    {
                        Index = index + 1,
                        ClubMemberId = cm.MembershipId,
                        Member = cm.User,
                        Club = cm.Club,
                        JoinDate = cm.JoinDate ?? DateTime.Now,
                        MemberStatus = cm.MemberStatus ?? "Unknown",
                        ShowActionButtons = (cm.MemberStatus ?? "Unknown") == "Pending"
                    })
                    .ToList();

                _members = new ObservableCollection<ClubMemberViewModel>(members);
                dgMembers.ItemsSource = _members;
                FilterMembers(); // Apply initial filters if any
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading members: {ex.Message}\nStack Trace: {ex.StackTrace}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void MemberName_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (_isWindowClosing) return; // Prevent execution if window is closing

            if (sender is TextBlock textBlock && textBlock.DataContext is ClubMemberViewModel member)
            {
                var user = member.Member;
                if (user == null)
                {
                    MessageBox.Show("Member information is not available.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                var userDetails = new Window
                {
                    Title = $"Member Details - {user.FullName ?? "Unknown"}",
                    Width = 300,
                    Height = 250,
                    WindowStartupLocation = WindowStartupLocation.CenterScreen,
                    Content = new StackPanel
                    {
                        Margin = new Thickness(10),
                        Children =
                        {
                            new TextBlock { Text = $"User ID: {user.UserId}" },
                            new TextBlock { Text = $"Full Name: {user.FullName ?? "N/A"}" },
                            new TextBlock { Text = $"Email: {user.Email ?? "N/A"}" },
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

        private void BtnAccept_Click(object sender, RoutedEventArgs e)
        {
            if (_isWindowClosing) return; // Prevent execution if window is closing

            if (sender is Button button && button.Tag is ClubMemberViewModel member)
            {
                try
                {
                    _memberDAO.UpdateClubMemberStatus(member.ClubMemberId, "Active");
                    SendStatusEmail(member.Member, member.Club, "Active");

                    member.MemberStatus = "Active";
                    member.ShowActionButtons = false;
                    FilterMembers(); // Refresh the list
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error accepting member: {ex.Message}\nStack Trace: {ex.StackTrace}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            if (_isWindowClosing) return; // Prevent execution if window is closing

            if (sender is Button button && button.Tag is ClubMemberViewModel member)
            {
                try
                {
                    _memberDAO.UpdateClubMemberStatus(member.ClubMemberId, "Inactive");
                    SendStatusEmail(member.Member, member.Club, "Inactive");

                    member.MemberStatus = "Inactive";
                    member.ShowActionButtons = false;
                    FilterMembers(); // Refresh the list
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error canceling member: {ex.Message}\nStack Trace: {ex.StackTrace}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void SendStatusEmail(User member, Club club, string status)
        {
            try
            {
                if (member == null || club == null || string.IsNullOrEmpty(member.Email))
                {
                    MessageBox.Show("Cannot send email: Member or club information is missing.", "Email Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                var fromAddress = new MailAddress("dungbd07@gmail.com", "Club Management");
                var toAddress = new MailAddress(member.Email);
                const string fromPassword = "qxuu rbkd khoy ubwu";
                string subject = $"Club Membership Status Update - {club.ClubName ?? "Unknown Club"}";
                string body = $"Dear {member.FullName ?? "Member"},\n\n" +
                              $"Your request to join the club '{club.ClubName ?? "Unknown Club"}' has been {status.ToLower()}.\n" +
                              (status == "Active" ? "You are now an active member of the club!" : "Your membership request has been canceled.") + "\n\n" +
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
                MessageBox.Show($"Failed to send email: {ex.Message}\nStack Trace: {ex.StackTrace}", "Email Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void TxtMemberFilter_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (_isWindowClosing) return; // Prevent execution if window is closing
            System.Diagnostics.Debug.WriteLine("Member name filter changed: " + txtMemberFilter.Text);
            FilterMembers();
        }

        private void FilterMembers()
        {
            try
            {
                System.Diagnostics.Debug.WriteLine("Entering FilterMembers");

                if (!IsLoaded || dgMembers == null)
                {
                    System.Diagnostics.Debug.WriteLine("Window not loaded or dgMembers is null, exiting FilterMembers");
                    return;
                }

                if (_members == null || !_members.Any())
                {
                    System.Diagnostics.Debug.WriteLine("No members to filter, setting empty ItemsSource");
                    dgMembers.ItemsSource = new ObservableCollection<ClubMemberViewModel>();
                    return;
                }

                var filteredMembers = _members.ToList();

                // Filter by Member Name
                if (!string.IsNullOrWhiteSpace(txtMemberFilter.Text))
                {
                    filteredMembers = filteredMembers
                        .Where(m => m != null && m.Member != null && m.Member.FullName != null && m.Member.FullName.ToLower().Contains(txtMemberFilter.Text.ToLower()))
                        .ToList();
                    System.Diagnostics.Debug.WriteLine($"After name filter: {filteredMembers.Count} members");
                }

                // Sort: Pending first, and update Index
                filteredMembers = filteredMembers
                    .OrderBy(m => m != null && (m.MemberStatus ?? "Unknown") != "Pending")
                    .Select((m, index) => new ClubMemberViewModel
                    {
                        Index = index + 1, // Update Index after filtering
                        ClubMemberId = m.ClubMemberId,
                        Member = m.Member,
                        Club = m.Club,
                        JoinDate = m.JoinDate,
                        MemberStatus = m.MemberStatus,
                        ShowActionButtons = m.ShowActionButtons
                    })
                    .ToList();

                // Update DataGrid
                dgMembers.ItemsSource = new ObservableCollection<ClubMemberViewModel>(filteredMembers);
                System.Diagnostics.Debug.WriteLine($"DataGrid updated with {filteredMembers.Count} members");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error in FilterMembers: {ex.Message}\nStack Trace: {ex.StackTrace}");
                MessageBox.Show($"Error filtering members: {ex.Message}\nStack Trace: {ex.StackTrace}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnClearFilter_Click(object sender, RoutedEventArgs e)
        {
            if (_isWindowClosing) return; // Prevent execution if window is closing

            txtMemberFilter.Text = "";
            System.Diagnostics.Debug.WriteLine("Filter cleared");
            FilterMembers();
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
            UnregisterEventHandlers(); // Unregister handlers before closing
            Close();
        }

        private void BtnClubMemberManagement_Click(object sender, RoutedEventArgs e)
        {
            new ClubMemberManagement(_currentUser).Show();
            this.Close();
        }

        private void BtnListMemberJoinClub_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnAttendance_Click(object sender, RoutedEventArgs e)
        {
            new AttendanceWPF(SessionDataUser.users[0]).ShowDialog();
        }
    }
}