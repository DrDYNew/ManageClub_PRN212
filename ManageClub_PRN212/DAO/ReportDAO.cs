using ManageClub_PRN212.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManageClub_PRN212.DAO
{
    public class ReportDAO
    {
        private readonly ManageClubContext _context;

        public ReportDAO()
        {
            _context = new ManageClubContext();
        }

        // Lấy danh sách tất cả các club
        public List<Club> GetAllClubs()
        {
            try
            {
                return _context.Clubs.ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetAllClubs: {ex.Message}");
                return new List<Club>();
            }
        }

        // Lấy số thành viên mới tham gia theo tháng
        public int GetNewMembersByMonth(int clubId, int month, int year)
        {
            try
            {
                return _context.ClubMembers
                    .Count(cm => cm.ClubId == clubId && cm.JoinDate.HasValue &&
                                 cm.JoinDate.Value.Month == month && cm.JoinDate.Value.Year == year);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetNewMembersByMonth: {ex.Message}");
                return 0;
            }
        }

        // Lấy số sự kiện được tổ chức theo tháng
        public int GetEventCountByMonth(int clubId, int month, int year)
        {
            try
            {
                return _context.Events
                    .Count(e => e.ClubId == clubId && e.EventDate.Month == month && e.EventDate.Year == year);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetEventCountByMonth: {ex.Message}");
                return 0;
            }
        }

        // Lấy tổng số người tham gia sự kiện theo tháng
        public int GetTotalParticipantsByMonth(int clubId, int month, int year)
        {
            try
            {
                return _context.EventParticipants
                    .Where(ep => ep.Event.ClubId == clubId && ep.Event.EventDate.Month == month && ep.Event.EventDate.Year == year)
                    .Count();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetTotalParticipantsByMonth: {ex.Message}");
                return 0;
            }
        }

        // Lấy tổng chi phí của club theo tháng
        public decimal GetTotalCostByMonth(int clubId, int month, int year)
        {
            try
            {
                var club = _context.Clubs
                    .FirstOrDefault(c => c.ClubId == clubId);

                if (club == null || club.TotalCost == null)
                {
                    return 0;
                }

                return club.TotalCost.Value; // Trả về TotalCost thực tế
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetTotalCostByMonth: {ex.Message}");
                return 0;
            }
        }

        public int GetPositiveFeedbackCountByMonth(int clubId, int month, int year)
        {
            try
            {
                return _context.EventFeedbacks
                    .Where(f => f.Event.ClubId == clubId &&
                                f.CreatedAt.HasValue &&
                                f.CreatedAt.Value.Month == month &&
                                f.CreatedAt.Value.Year == year &&
                                f.Rating.HasValue &&
                                f.Rating >= 4)
                    .Count();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetPositiveFeedbackCountByMonth: {ex.Message}");
                return 0;
            }
        }

        public int GetNegativeFeedbackCountByMonth(int clubId, int month, int year)
        {
            try
            {
                return _context.EventFeedbacks
                    .Where(f => f.Event.ClubId == clubId &&
                                f.CreatedAt.HasValue &&
                                f.CreatedAt.Value.Month == month &&
                                f.CreatedAt.Value.Year == year &&
                                f.Rating.HasValue &&
                                f.Rating <= 3)
                    .Count();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetNegativeFeedbackCountByMonth: {ex.Message}");
                return 0;
            }
        }
    }
}
