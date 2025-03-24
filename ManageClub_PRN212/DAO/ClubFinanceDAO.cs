using ManageClub_PRN212.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManageClub_PRN212.DAO
{
    public class ClubFinanceDAO
    {
        private readonly ManageClubContext _context;

        public ClubFinanceDAO()
        {
            _context = new ManageClubContext();
        }

        public List<Club> GetClubsManagedByUser(int userId)
        {
            try
            {
                return _context.Clubs
                    .Where(c => c.PresidentId == userId)
                    .ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetClubsManagedByUser: {ex.Message}");
                return new List<Club>();
            }
        }

        public List<ClubFinance> GetAllClubFinances()
        {
            try
            {
                return _context.ClubFinances
                    .Include(cf => cf.Club)
                    .ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetAllClubFinances: {ex.Message}");
                return new List<ClubFinance>();
            }
        }

        public void AddClubFinance(ClubFinance newFinance)
        {
            try
            {
                _context.ClubFinances.Add(newFinance);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in AddClubFinance: {ex.Message}");
            }
        }

        public void UpdateClubFinance(ClubFinance updatedFinance)
        {
            try
            {
                _context.Entry(updatedFinance).State = EntityState.Modified;
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in UpdateClubFinance: {ex.Message}");
            }
        }

        public void DeleteClubFinance(int financeId)
        {
            try
            {
                var financeToDelete = _context.ClubFinances.Find(financeId);
                if (financeToDelete != null)
                {
                    _context.ClubFinances.Remove(financeToDelete);
                    _context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in DeleteClubFinance: {ex.Message}");
            }
        }
    }
}
