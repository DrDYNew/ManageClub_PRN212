using ManageClub_PRN212.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManageClub_PRN212.ViewModels
{
    class ClubMemberViewModel
    {
        public int Index { get; set; }
        public int ClubMemberId { get; set; }
        public User Member { get; set; }
        public Club Club { get; set; }
        public DateTime JoinDate { get; set; }
        public string MemberStatus { get; set; }
        public bool ShowActionButtons { get; set; }
    }
}
