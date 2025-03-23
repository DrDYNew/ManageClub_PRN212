using ManageClub_PRN212.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManageClub_PRN212.DAO
{
    internal class UserDAO
    {
        public static User GetUserByLogIn(string email, string password)
        {
            ManageClubContext manageClubContext = new ManageClubContext();
            return manageClubContext.Users.Include(x => x.Role).Where(x => x.Email == email && x.Password == password).FirstOrDefault();
        }

        public static List<User> GetUsers()
        {
            ManageClubContext manageClubContext = new ManageClubContext();
            return manageClubContext.Users.ToList();
        }

        public static void UpdateUserRole(int userId, int newRoleId)
        {
            ManageClubContext manageClubContext = new ManageClubContext();
            User user = GetUserById(userId);
            if (user != null)
            {
                user.RoleId = newRoleId;
            }
            manageClubContext.Update(user);
            manageClubContext.SaveChanges();
        }

        public static User GetUserById(int id)
        {
            ManageClubContext manageClubContext = new ManageClubContext();
            return manageClubContext.Users.Where(u => u.UserId == id).FirstOrDefault();
        }

        public static User GetUserByEmail(string email)
        {
            ManageClubContext manageClubContext= new ManageClubContext();
            return manageClubContext.Users.FirstOrDefault(x => x.Email == email);
        }

        public static void UpdateForgotPassword(User user, string forgotPassword)
        {
            ManageClubContext manageClubContext = new ManageClubContext();
            user.Password = forgotPassword;
            manageClubContext.Update(user);
            manageClubContext.SaveChanges();
        }
    }
}
