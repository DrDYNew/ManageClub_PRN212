using ManageClub_PRN212.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManageClub_PRN212.DAO
{
    internal class ClubDAO
    {

        private readonly ManageClubContext _context;

        public ClubDAO()
        {
            _context = new ManageClubContext();
        }

        public static List<Club> GetClubs()
        {
            ManageClubContext context = new ManageClubContext();
            return context.Clubs.Include(c => c.President).ToList();
        }

        public static bool CheckExistClubName(string name)
        {
            ManageClubContext context = new ManageClubContext();
            return context.Clubs.Where(cl => cl.ClubName.ToLower() == name.ToLower()).FirstOrDefault() != null;
        }

        public static void AddNewClub(Club club)
        {
            ManageClubContext context = new ManageClubContext();
            context.Clubs.Add(club);
            context.SaveChanges();
        }

        public static Club GetClubById(int id)
        {
            ManageClubContext context = new ManageClubContext();
            return context.Clubs.Where(cl => cl.ClubId == id).FirstOrDefault();
        }

        public static void UpdateClub(Club club)
        {
            ManageClubContext context = new ManageClubContext();
            context.Clubs.Update(club);
            context.SaveChanges();
        }

        public static bool CheckExistClubNameUpdate(string name, int id)
        {
            ManageClubContext context = new ManageClubContext();
            return context.Clubs.Where(cl => cl.ClubName.ToLower() == name.ToLower() && cl.ClubId != id).FirstOrDefault() != null;
        }

        public static void RemoveClub(Club club)
        {
            ManageClubContext manageClubContext = new ManageClubContext();
            manageClubContext.Clubs.Remove(club);
            manageClubContext.SaveChanges();
        }

        public static List<Club> GetClubsById(int id)
        {
            ManageClubContext manageClubContext= new ManageClubContext();
            return manageClubContext.Clubs.Where(cl => cl.PresidentId == id).ToList();
        }

        public List<Club> GetActiveClubs()
        {
            try
            {
                return _context.Clubs
                    .Include(c => c.President)
                    .Where(c => c.ClubStatus == "Active")
                    .ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetActiveClubs: {ex.Message}");
                return new List<Club>();
            }
        }

        public List<Club> GetClubsByPresident(int userId)
        {
            try
            {
                return _context.Clubs
                    .Include("President") // Bao gồm thông tin President (User)
                    .Where(c => c.PresidentId == userId && c.ClubStatus == "Active")
                    .ToList();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving clubs for president {userId}: {ex.Message}", ex);
            }
        }

    }
}
