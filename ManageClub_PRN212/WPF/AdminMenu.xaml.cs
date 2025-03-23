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

namespace ManageClub_PRN212.WPF
{
    /// <summary>
    /// Interaction logic for AdminMenu.xaml
    /// </summary>
    public partial class AdminMenu : Window
    {
        ManageClubContext _context;

        public AdminMenu()
        {
            InitializeComponent();
            _context = new ManageClubContext();
            LoadName();

        }

        private void LoadName()
        {
            if (SessionDataUser.users == null || SessionDataUser.users.Count == 0)
            {
                MessageBox.Show("User session expired. Please log in again.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                this.Close();
                return;
            }

            User userSession = SessionDataUser.users[0];

            User user = _context.Users.FirstOrDefault(x => x.UserId == userSession.UserId);

            this.lbName.Content = "Welcome " + user.FullName;
        }


    }
}
