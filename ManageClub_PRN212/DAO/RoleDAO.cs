using ManageClub_PRN212.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManageClub_PRN212.DAO
{
    class RoleDAO
    {
        ManageClubContext _context;

        public RoleDAO()
        {
            _context = new ManageClubContext();
        }
        
        public static List<Role> GetRoles()
        {
            ManageClubContext context = new ManageClubContext();
            return context.Roles.ToList();
        }

        // Lấy tất cả vai trò
        public List<Role> GetAllRoles()
        {
            try
            {
                return _context.Roles.ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetAllRoles: {ex.Message}");
                return new List<Role>();
            }
        }
    }
}
