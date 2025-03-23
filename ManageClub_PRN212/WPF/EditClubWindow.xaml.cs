using ManageClub_PRN212.DAO;
using ManageClub_PRN212.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ManageClub_PRN212.WPF
{
    /// <summary>
    /// Interaction logic for EditClubWindow.xaml
    /// </summary>
    public partial class EditClubWindow : Window
    {
        private string action;
        public EditClubWindow(string action)
        {
            InitializeComponent();
            this.action = action;
            EditWithAction();
        }

        void EditWithAction()
        {
            if (action == "add")
            {
                LoadComboBoxPresident();
                LoadComboBoxClubStatus();
            }
        }

        void LoadComboBoxPresident()
        {
            var users = UserDAO.GetUsers();
            this.cbPresident.ItemsSource = users;
            this.cbPresident.DisplayMemberPath = "UserIdWithName";
            this.cbPresident.SelectedValuePath = "UserId";
            this.cbPresident.SelectedIndex = 0;
        }

        void LoadComboBoxClubStatus()
        {
            List<string> status = new List<string>();
            status.Add("Active");
            status.Add("Inactive");
            this.cbStatus.ItemsSource = status;
            this.cbStatus.SelectedIndex = 0;
        }

        private void txtPresident_TextChanged(object sender, TextChangedEventArgs e)
        {
            string searchText = txtPresident.Text.ToLower();
            var filteredUser = UserDAO.GetUsers().Where(u => u.FullName.ToLower().Contains(searchText)).ToList();
            cbPresident.ItemsSource = filteredUser;
            cbPresident.SelectedIndex = 0;
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            string clubName = txtClubName.Text;
            if (clubName == null || clubName.Trim().Length < 1)
            {
                MessageBox.Show("Club Name is required.", "Validation", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if(ClubDAO.CheckExistClubName(clubName))
            {
                MessageBox.Show("This club name is existed.", "Validation", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            string description = txtDescription.Text;
            if (description == null || description.Trim().Length < 1)
            {
                MessageBox.Show("Description is required.", "Validation", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            DateTime? establishedDate = dpEstablishedDate.SelectedDate;
            if (establishedDate.HasValue)
            {
                if(establishedDate.Value > DateTime.Now)
                {
                    MessageBox.Show("Please choose a valid date.", "Validation", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
            }
            else
            {
                MessageBox.Show("Please choose a valid date.", "Validation", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            int userId = Int32.Parse(cbPresident.SelectedValue.ToString());
            string status = cbStatus.SelectedValue.ToString();
            MessageBoxResult res =  MessageBox.Show("Are you sure you want to add new club ?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if(res == MessageBoxResult.Yes)
            {
                Club newClub = new Club()
                {
                    ClubName = clubName,
                    Description = description,
                    EstablishedDate = establishedDate,
                    PresidentId = userId,
                    ClubStatus = status,
                    TotalCost = 0,
                };
                ClubDAO.AddNewClub(newClub);
                UserDAO.UpdateUserRole(userId, 2);
                MessageBox.Show("Add Club Successfully!", "Add Club", MessageBoxButton.OK);
                this.Close();
            }
        }
    }
}
