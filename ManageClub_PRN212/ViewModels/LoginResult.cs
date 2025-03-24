using ManageClub_PRN212.Models;

namespace ManageClub_PRN212.ViewModels
{
    public class LoginResult
    {
        public User User { get; set; }
        public string ErrorMessage { get; set; }
        public bool IsSuccess => User != null;
    }
}