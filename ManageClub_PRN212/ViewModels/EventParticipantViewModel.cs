using ManageClub_PRN212.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManageClub_PRN212.ViewModels
{
    class EventParticipantViewModel
    {
        public int Index { get; set; }
        public int EventParticipantId { get; set; }
        public User User { get; set; }
        public Event Event { get; set; }
        public DateTime? RegistrationDate { get; set; }
    }
}
