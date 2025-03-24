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
        private readonly ManageClubContext _context;

        public UserDAO()
        {
            _context = new ManageClubContext();
        }

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

        public static void RegisterUser(User user)
        {
            ManageClubContext context = new ManageClubContext();
            try
            {
                // Check if email already exists
                if (context.Users.Any(u => u.Email == user.Email))
                {
                    throw new Exception("Email is already registered!");
                }

                context.Users.Add(user);
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error during registration: {ex.Message}");
            }
        }

        public static List<User> GetUserWithRole()
        {
            ManageClubContext context = new ManageClubContext();
            return context.Users.Include(x => x.Role).ToList();
        }

        public static void UpdateUser(User user)
        {
            ManageClubContext manageClubContext= new ManageClubContext();
            manageClubContext.Users.Update(user);
            manageClubContext.SaveChanges();
        }

        public static List<User> GetUserOutsideClubExceptRoleId(int clubId, int roleId)
        {
            using (ManageClubContext manageClubContext = new ManageClubContext())
            {
                var usersOutsideClub = (from user in manageClubContext.Users
                                        where !(from cm in manageClubContext.ClubMembers
                                                where cm.ClubId == clubId
                                                select cm.UserId)
                                                .Contains(user.UserId)
                                        && user.RoleId != roleId
                                        select user).ToList();

                return usersOutsideClub;
            }
        }


        public static List<User> GetUserExceptRoleId(int id)
        {
            ManageClubContext manageClubContext= new ManageClubContext();
            return manageClubContext.Users.Where(u => u.RoleId != id).ToList();
        }

        public void AddUser(User user)
        {
            try
            {
                _context.Users.Add(user);
                _context.SaveChanges();
                Console.WriteLine($"Added user: {user.Email}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in AddUser: {ex.Message}");
                throw; // Ném ngoại lệ để xử lý ở tầng gọi
            }
        }

        public User GetUserWithRoleById(int userId)
        {
            try
            {
                return _context.Users
                    .Include(u => u.Role)
                    .FirstOrDefault(u => u.UserId == userId);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetUserById: {ex.Message}");
                return null;
            }
        }

        public List<User> GetAllUsers()
        {
            try
            {
                return _context.Users
                    .Include(u => u.Role) // Bao gồm thông tin Role để lấy RoleName
                    .ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetAllUsers: {ex.Message}");
                return new List<User>();
            }
        }
    }
}
