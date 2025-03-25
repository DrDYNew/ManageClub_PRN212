using ManageClub_PRN212.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManageClub_PRN212.DAO
{
    class ClubMemberDAO
    {
        private readonly ManageClubContext _context;

        public ClubMemberDAO()
        {
            _context = new ManageClubContext();
        }

        public static List<ClubMember> GetClubMembersByClubId(int id)
        {
            ManageClubContext context = new ManageClubContext();
            return context.ClubMembers.Include(cl => cl.Club).Include(cl => cl.User).Where(cl => cl.ClubId == id && cl.MemberStatus != "Left").ToList();
        }

        public static void AddClubMember(ClubMember clubMember)
        {
            ManageClubContext context = new ManageClubContext();
            context.ClubMembers.Add(clubMember);
            context.SaveChanges();
        }

        public static ClubMember GetClubMemberByClubIdAndMemberId(int clubId, int memberId)
        {
            using (ManageClubContext context = new ManageClubContext())
            {
                return context.ClubMembers.FirstOrDefault(cl => cl.ClubId == clubId && cl.UserId == memberId);
            }
        }

        public static void UpdateClubMember(ClubMember clubMember)
        {
            ManageClubContext context = new ManageClubContext();
            context.ClubMembers.Update(clubMember);
            context.SaveChanges();
        }

        public bool IsUserMember(int clubId, int userId)
        {
            try
            {
                return _context.ClubMembers
                    .Any(cm => cm.ClubId == clubId && cm.UserId == userId && (cm.MemberStatus == "Active" || cm.MemberStatus == "Pending"));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in IsUserMember: {ex.Message}");
                return false;
            }
        }

        public void UpdateClubMemberStatus(int clubMemberId, string status)
        {
            try
            {
                var clubMember = _context.ClubMembers
                    .FirstOrDefault(cm => cm.MembershipId == clubMemberId);
                if (clubMember == null)
                {
                    throw new Exception($"Club member with ID {clubMemberId} not found.");
                }

                clubMember.MemberStatus = status;
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error updating club member status: {ex.Message}", ex);
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

        // Trong ClubDAO.cs
        public List<ClubMember> GetAllClubMembers()
        {
            try
            {
                return _context.ClubMembers
                    .Include("User")
                    .Include("Club")
                    .ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("Error retrieving all club members: " + ex.Message, ex);
            }
        }

        public void AddClubMember1(ClubMember clubMember)
        {
            try
            {
                _context.ClubMembers.Add(clubMember);
                _context.SaveChanges();
                Console.WriteLine($"Added member: ClubId={clubMember.ClubId}, UserId={clubMember.UserId}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in AddClubMember: {ex.Message}");
            }
        }


    }
}
