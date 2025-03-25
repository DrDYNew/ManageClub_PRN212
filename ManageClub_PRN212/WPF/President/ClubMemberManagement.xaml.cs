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

namespace ManageClub_PRN212.WPF.President
{
    /// <summary>
    /// Interaction logic for ClubMemberManagement.xaml
    /// </summary>
    public partial class ClubMemberManagement : Window
    {
        private User user;
        private int clubId;
        public ClubMemberManagement(User user)
        {
            this.user = user;
            InitializeComponent();
            LoadComboBoxClub();
            var clubs = ClubDAO.GetClubsById(user.UserId);
            clubId = clubs[0].ClubId;
            LoadDataGridClubMember(clubs[0].ClubId);
        }

        void LoadComboBoxClub()
        {
            var clubs = ClubDAO.GetClubsById(user.UserId);
            this.cbClubs.ItemsSource = clubs;
            this.cbClubs.DisplayMemberPath = "ClubName";
            this.cbClubs.SelectedValuePath = "ClubId";
            this.cbClubs.SelectedIndex = 0;
        }

        void LoadDataGridClubMember(int clubId)
        {
            var members = ClubMemberDAO.GetClubMembersByClubId(clubId);
            this.dgMembers.ItemsSource = members;
        }


        private void cbClubs_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int id = Int32.Parse(cbClubs.SelectedValue.ToString());
            clubId = id;
            LoadDataGridClubMember(clubId);
        }

        private void txtAddMember_Click(object sender, RoutedEventArgs e)
        {
            EditMemberWindow editMemberWindow = new EditMemberWindow("add", clubId);
            editMemberWindow.Closed += (sender, e) =>
            {
                LoadDataGridClubMember(clubId);
            };
            editMemberWindow.ShowDialog();
        }

        private void btnEditClub_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            int memberId = Int32.Parse(button.Tag.ToString());
            EditMemberWindow editMemberWindow = new EditMemberWindow("edit", memberId, clubId);
            editMemberWindow.Closed += (sender, e) =>
            {
                LoadDataGridClubMember(clubId);
            };
            editMemberWindow.ShowDialog();
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void BtnClubMemberManagement_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnEventManagement_Click(object sender, RoutedEventArgs e)
        {
            new EventWPF(user).Show();
            this.Close();
        }

        private void BtnParticipants_Click(object sender, RoutedEventArgs e)
        {
            new EventParticipantsWPF(user).Show();
            this.Close();
        }

        private void BtnJoinListEvent_Click(object sender, RoutedEventArgs e)
        {
            new ListJoinEvent(user).Show();
            this.Close();
        }

        private void BtnMemberJoinClub_Click(object sender, RoutedEventArgs e)
        {
            new ListMemberJoinClub(user).Show();
            this.Close();
        }

        private void BtnClubFinance_Click(object sender, RoutedEventArgs e)
        {
            new ManageClubFinance(user).Show();
            this.Close();
        }

        private void BtnLogout_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("Are you sure you want to logout?", "Logout", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                SessionDataUser.users.Clear();
                Login loginWindown = new Login();
                loginWindown.Show();
                this.Close();
            }
        }

        private void BtnAttendance_Click(object sender, RoutedEventArgs e)
        {
            new AttendanceWPF(SessionDataUser.users[0]).ShowDialog();
        }
    }
}
