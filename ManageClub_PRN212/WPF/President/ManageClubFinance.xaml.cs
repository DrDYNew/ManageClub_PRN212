using ManageClub_PRN212.DAO;
using ManageClub_PRN212.Models;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using static ManageClub_PRN212.Models.User;

namespace ManageClub_PRN212.WPF.President
{
    public partial class ManageClubFinance : Window
    {
        private readonly ClubFinanceDAO _clubFinanceDAO;
        private readonly User _currentUser;
        private Button _selectedButton;
        public ObservableCollection<ClubFinance> ClubFinances { get; set; }
        private ObservableCollection<ClubFinance> _allClubFinances;

        public ManageClubFinance(User currentUser)
        {
            InitializeComponent();
            _clubFinanceDAO = new ClubFinanceDAO();
            _currentUser = currentUser ?? throw new ArgumentNullException(nameof(currentUser));

            // Khởi tạo ClubFinances và _allClubFinances
            ClubFinances = new ObservableCollection<ClubFinance>();
            _allClubFinances = new ObservableCollection<ClubFinance>();

            LoadClubs();
            LoadClubFinances();
            LoadFilterClubs();

            if (cmbFilterClub.SelectedItem == null)
            {
                cmbFilterClub.SelectedIndex = 0; 
            }

            dgClubFinance.ItemsSource = ClubFinances;
            _selectedButton = BtnClubFinance;
            _selectedButton.Style = (Style)FindResource("SelectedSidebarButtonStyle");
        }

        private void LoadClubs()
        {
            var clubs = _clubFinanceDAO.GetClubsManagedByUser(_currentUser.UserId);
            cmbClubs.ItemsSource = clubs;
            cmbClubs.DisplayMemberPath = "ClubName";
            cmbClubs.SelectedValuePath = "ClubId";
            cmbClubs.SelectedIndex = 0;
        }

        private void LoadFilterClubs()
        {
            var clubs = _clubFinanceDAO.GetClubsManagedByUser(_currentUser.UserId);
            foreach (var club in clubs)
            {
                cmbFilterClub.Items.Add(new ComboBoxItem { Content = club.ClubName, Tag = club.ClubId });
            }
        }

        private void LoadClubFinances()
        {
            try
            {
                var managedClubs = _clubFinanceDAO.GetClubsManagedByUser(_currentUser.UserId);
                var clubIds = managedClubs.Select(c => c.ClubId).ToList();
                _allClubFinances.Clear(); 
                var finances = _clubFinanceDAO.GetAllClubFinances().Where(cf => clubIds.Contains(cf.ClubId)).ToList();
                foreach (var finance in finances)
                {
                    _allClubFinances.Add(finance);
                }

                ClubFinances.Clear();
                foreach (var item in _allClubFinances)
                {
                    ClubFinances.Add(item);
                }

                dgClubFinance.ItemsSource = ClubFinances;

                if (!ClubFinances.Any())
                {
                    MessageBox.Show("No finance records found for clubs you manage.", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading club finances: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ApplyFilters()
        {
            if (_allClubFinances == null || ClubFinances == null)
            {
                return;
            }

            var filteredList = _allClubFinances.AsEnumerable();

            // Lọc theo Club Name
            if (cmbFilterClub.SelectedItem is ComboBoxItem selectedClub && selectedClub.Content?.ToString() != "All Clubs")
            {
                int clubId = (int)selectedClub.Tag;
                filteredList = filteredList.Where(cf => cf.ClubId == clubId);
            }

            ClubFinances.Clear();
            foreach (var item in filteredList)
            {
                ClubFinances.Add(item);
            }
        }

        private void FilterClub_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ApplyFilters();
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (cmbClubs.SelectedValue == null || string.IsNullOrWhiteSpace(txtTransactionType.Text) || string.IsNullOrWhiteSpace(txtPrice.Text))
                {
                    MessageBox.Show("Please fill in all required fields.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                var newFinance = new ClubFinance
                {
                    ClubId = (int)cmbClubs.SelectedValue,
                    TransactionType = txtTransactionType.Text,
                    Price = double.Parse(txtPrice.Text),
                    Description = txtDescription.Text,
                    TransactionDate = DateTime.Now
                };
                _clubFinanceDAO.AddClubFinance(newFinance);
                LoadClubFinances();
                ApplyFilters();
                ClearInputs();
                MessageBox.Show("Finance record added successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error adding finance record: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            if (dgClubFinance.SelectedItem is ClubFinance selectedFinance)
            {
                try
                {
                    selectedFinance.ClubId = (int)cmbClubs.SelectedValue;
                    selectedFinance.TransactionType = txtTransactionType.Text;
                    selectedFinance.Price = double.Parse(txtPrice.Text);
                    selectedFinance.Description = txtDescription.Text;
                    _clubFinanceDAO.UpdateClubFinance(selectedFinance);
                    LoadClubFinances();
                    ApplyFilters();
                    ClearInputs();
                    MessageBox.Show("Finance record updated successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error updating finance record: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("Please select a finance record to update.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (dgClubFinance.SelectedItem is ClubFinance selectedFinance)
            {
                var result = MessageBox.Show("Are you sure you want to delete this finance record?", "Confirm Delete", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    try
                    {
                        _clubFinanceDAO.DeleteClubFinance(selectedFinance.FinanceId);
                        LoadClubFinances();
                        ApplyFilters();
                        ClearInputs();
                        MessageBox.Show("Finance record deleted successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error deleting finance record: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("Please select a finance record to delete.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void BtnClear_Click(object sender, RoutedEventArgs e)
        {
            ClearInputs();
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void DgClubFinance_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dgClubFinance.SelectedItem is ClubFinance selectedFinance)
            {
                cmbClubs.SelectedValue = selectedFinance.ClubId;
                txtTransactionType.Text = selectedFinance.TransactionType;
                txtPrice.Text = selectedFinance.Price.ToString();
                txtDescription.Text = selectedFinance.Description;
            }
        }

        private void ClearInputs()
        {
            cmbClubs.SelectedIndex = 0;
            txtTransactionType.Text = string.Empty;
            txtPrice.Text = string.Empty;
            txtDescription.Text = string.Empty;
            dgClubFinance.SelectedItem = null;
        }

        // Sidebar Navigation
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

        private void BtnManageClubFinance_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedButton != null)
                _selectedButton.Style = (Style)FindResource("SidebarButtonStyle");
            _selectedButton = (Button)sender;
            _selectedButton.Style = (Style)FindResource("SelectedSidebarButtonStyle");
        }

        private void BtnLogout_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("Are you sure you want to logout?", "Logout", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                SessionDataUser.users.Clear();
                new Login().Show();
                Close();
            }
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