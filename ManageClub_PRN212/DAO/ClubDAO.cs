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
            context.Add(club);
            context.SaveChanges();
        }
    }
}
