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
    /// Interaction logic for ClubManagement.xaml
    /// </summary>
    public partial class ClubManagement : Window
    {
        ManageClubContext _context;
        User currentUser;
        public ClubManagement(User user)
        {
            InitializeComponent();
            _context = new ManageClubContext();
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
    }
}
