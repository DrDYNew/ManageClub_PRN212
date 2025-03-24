using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManageClub_PRN212.ViewModels
{
    public class ReportResult // Thêm "public" để lớp có thể truy cập từ các file khác
    {
        public string ClubName { get; set; }
        public int NewMembers { get; set; }
        public int EventCount { get; set; }
        public int TotalParticipants { get; set; }
        public decimal TotalCost { get; set; }
        public int PositiveFeedbackCount { get; set; } // Số lượng feedback có Rating >= 4
        public int NegativeFeedbackCount { get; set; } // Số lượng feedback có Rating <= 3
    }
}