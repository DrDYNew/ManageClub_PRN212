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
    }
}
