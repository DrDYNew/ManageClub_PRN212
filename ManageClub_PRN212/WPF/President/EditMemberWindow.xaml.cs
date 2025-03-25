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
    /// Interaction logic for EditMemberWindow.xaml
    /// </summary>
    public partial class EditMemberWindow : Window
    {

        private string action;
        private int memberId;
        private int clubId;
        public EditMemberWindow(string action, int clubId)
        {
            InitializeComponent();
            this.action = action;
            this.clubId = clubId;
            EditWithAction();
        }

        public EditMemberWindow(string action, int memberId, int clubId)
        {
            InitializeComponent();
            this.action = action;
            this.memberId = memberId;
            this.clubId = clubId;
            EditWithAction();
        }

        void EditWithAction()
        {
            if (action == "add")
            {
                LoadComboBoxMember();
                LoadComboBoxMemberStatus();
                this.btnSave.Visibility = Visibility.Collapsed;
            }
            if (action == "edit")
            {
                var member = UserDAO.GetUserById(memberId);
                //this.cbMember.IsReadOnly = true;
                this.cbMember.Visibility = Visibility.Collapsed;
                this.txtMember.Text = member.UserIdWithName;
                this.txtMember.IsReadOnly = true;
                LoadComboBoxMemberStatus();
                this.cbStatus.SelectedValue = member.Status;
                this.tbEditMember.Text = "Edit Member";
                dpJoinDate.SelectedDate = member.DateJoined;
                this.btnAdd.Visibility = Visibility.Collapsed;
            }
        }

        void LoadComboBoxMember()
        {
            var members = UserDAO.GetUserOutsideClubExceptRoleId(clubId, 1);
            this.cbMember.ItemsSource = members;
            this.cbMember.DisplayMemberPath = "UserIdWithName";
            this.cbMember.SelectedValuePath = "UserId";
            this.cbMember.SelectedIndex = 0;
        }

        void LoadComboBoxMemberStatus()
        {
            List<string> status = new List<string>();
            status.Add("Active");
            status.Add("Inactive");
            this.cbStatus.ItemsSource = status;
            this.cbStatus.SelectedIndex = 0;
        }

        private void txtMember_TextChanged(object sender, TextChangedEventArgs e)
        {
            string searchText = txtMember.Text.ToLower().Trim();
            var filteredUser = UserDAO.GetUserOutsideClubExceptRoleId(clubId, 1).Where(u => u.FullName.ToLower().Contains(searchText)).ToList();
            cbMember.ItemsSource = filteredUser;
            cbMember.SelectedIndex = 0;
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            int userId = Int32.Parse(cbMember.SelectedValue.ToString());
            DateTime? joinDate = dpJoinDate.SelectedDate;
            if (joinDate.HasValue)
            {
                if (joinDate.Value > DateTime.Now)
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
            string status = cbStatus.SelectedValue.ToString();
            MessageBoxResult res = MessageBox.Show("Are you sure you want to add new member ?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if(res == MessageBoxResult.Yes)
            {
                ClubMember clubMember = new ClubMember()
                {
                    ClubId = clubId,
                    UserId = userId,
                    JoinDate = joinDate,
                    MemberStatus = status
                };
                ClubMemberDAO.AddClubMember(clubMember);
            }
            MessageBox.Show("Add Member Successfully!", "Add Member", MessageBoxButton.OK);
            this.Close();
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {

            DateTime? joinDate = dpJoinDate.SelectedDate;
            if (joinDate.HasValue)
            {
                if (joinDate.Value > DateTime.Now)
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
            string status = cbStatus.SelectedValue.ToString();
            MessageBoxResult res = MessageBox.Show("Are you sure you want to save the edit ?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if(res == MessageBoxResult.Yes)
            {
                ClubMember clubMember = ClubMemberDAO.GetClubMemberByClubIdAndMemberId(clubId, memberId);
                clubMember.JoinDate = joinDate;
                clubMember.MemberStatus = status;
                ClubMemberDAO.UpdateClubMember(clubMember);
                MessageBox.Show("Update Member Successfully!", "Update Member", MessageBoxButton.OK);
                this.Close();
            }
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        
    }
}
