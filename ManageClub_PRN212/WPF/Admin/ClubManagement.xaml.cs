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
using static ManageClub_PRN212.Models.User;

namespace ManageClub_PRN212.WPF.Admin
{
    /// <summary>
    /// Interaction logic for ClubManagement.xaml
    /// </summary>
    public partial class ClubManagement : Window
    {
        User currentUser;
        public ClubManagement(User user)
        {
            InitializeComponent();
            currentUser = user;
            LoadDataGridClub();
        }

        void LoadDataGridClub()
        {
            var clubs = ClubDAO.GetClubs();
            this.dgClubs.ItemsSource = clubs;
        }

        private void btnAddClub_Click(object sender, RoutedEventArgs e)
        {
            EditClubWindow editClubWindow = new EditClubWindow("add");
            editClubWindow.Closed += (sender, e) =>
            {
                LoadDataGridClub();
            };
            editClubWindow.ShowDialog();
        }

        private void BtnAccountManagement_Click(object sender, RoutedEventArgs e)
        {
            AccountWindow accountWindow = new AccountWindow();
            accountWindow.Show();
            this.Close();
        }

        private void BtnLogout_Click(object sender, RoutedEventArgs e)
        {
            SessionDataUser.users.Clear();
            Login loginWindown = new Login();
            loginWindown.Show();
            this.Close();
        }

        private void btnEditClub_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            var clubId = Int32.Parse(btn.Tag.ToString());
            EditClubWindow editClubWindow = new EditClubWindow("edit", clubId);
            editClubWindow.Closed += (sender, e) =>
            {
                LoadDataGridClub();
            };
            editClubWindow.ShowDialog();
        }

        private void BtnRemoveClub_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            var clubId = Int32.Parse(btn.Tag.ToString());
            MessageBoxResult res = MessageBox.Show("Are you sure you want to remove this club ?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (res == MessageBoxResult.Yes)
            {
                ClubDAO.RemoveClub(ClubDAO.GetClubById(clubId));
                MessageBox.Show("Remove Club Successfully!", "Remove Club", MessageBoxButton.OK);
                LoadDataGridClub();
            }
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void BtnMyProfile_Click(object sender, RoutedEventArgs e)
        {
            ProfileWindow profileWindow = new ProfileWindow();
            profileWindow.ShowDialog();
        }
    }
}
