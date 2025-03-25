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

namespace ManageClub_PRN212.WPF.Member
{
    /// <summary>
    /// Interaction logic for MyClub.xaml
    /// </summary>
    public partial class MyClub : Window
    {
        private User currentUser;
        private Button button;
        private int clubId;
        public MyClub(User user)
        {
            InitializeComponent();
            currentUser = user;
            button = BtnMyClubs;
            button.Style = (Style)FindResource("SelectedNavbarButtonStyle");
            LoadComboBoxClub();
        }

        private void BtnMyClubs_Click(object sender, RoutedEventArgs e) { }

        private void BtnJoinEvents_Click(object sender, RoutedEventArgs e)
        {
            new UserEventWPF(currentUser).Show();
            this.Close();
        }

        private void BtnJoinClubs_Click(object sender, RoutedEventArgs e)
        {
            new JoinClubWPF(currentUser).Show();
            this.Close();
        }

        private void BtnMyProfile_Click(object sender, RoutedEventArgs e)
        {
            new ProfileWindow().ShowDialog();
        }

        private void BtnLogout_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("Are you sure you want to logout?", "Logout", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                SessionDataUser.users.Clear();
                new Login().Show();
                this.Close();
            }
        }

        private void LoadComboBoxClub()
        {
            var clubs = ClubDAO.GetClubsOfMember(currentUser.UserId);
            if (clubs.Count == 0)
            {
                btnLeaveClub.Visibility = Visibility.Collapsed;
            }
            this.cbClubSelection.ItemsSource = clubs;
            this.cbClubSelection.DisplayMemberPath = "ClubName";
            this.cbClubSelection.SelectedValuePath = "ClubId";
            this.cbClubSelection.SelectedIndex = 0;
        }

        private void cbClubSelection_Changed(object sender, SelectionChangedEventArgs e)
        {
            if (cbClubSelection.SelectedValue != null)
            {
                int clubId = (int)cbClubSelection.SelectedValue;
                LoadDataGridClubMember(clubId);
            }
        }


        private void LoadDataGridClubMember(int clubId)
        {
            var members = ClubMemberDAO.GetClubMembersByClubId(clubId);
            this.dgClubMembers.ItemsSource = members;
        }

        private void btnLeaveClub_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("Are you sure you want to leave this club?", "Leave Club", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                if (cbClubSelection.SelectedValue != null)
                {
                    int clubId = (int)cbClubSelection.SelectedValue;
                    ClubMember clubMember = ClubMemberDAO.GetClubMemberByClubIdAndMemberId(clubId, currentUser.UserId);
                    if (clubMember != null)
                    {
                        clubMember.MemberStatus = "Left";
                        ClubMemberDAO.UpdateClubMember(clubMember);
                        MessageBox.Show("Leave Club Successfully!", "Leave Club", MessageBoxButton.OK);
                        LoadComboBoxClub();
                        if (cbClubSelection.SelectedValue != null)
                        {
                            LoadDataGridClubMember((int)cbClubSelection.SelectedValue);
                        }
                        else
                        {
                            LoadDataGridClubMember(-1);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Error: Club member not found.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                else
                {
                    MessageBox.Show("Error: No club selected.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
        }


        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void BtnAttendance_Click(object sender, RoutedEventArgs e)
        {
            new AttendanceWPF(SessionDataUser.users[0]).ShowDialog();
        }
    }
}
