using System;
using System.Windows;
using ManageClub_PRN212.Models;

namespace ManageClub_PRN212.ViewModels
{
    public class AttendanceViewModel
    {
        public int AttendanceId { get; set; }
        public int EventId { get; set; }
        public int UserId { get; set; }
        public DateTime? CheckInTime { get; set; }
        public DateTime? CheckOutTime { get; set; }
        public string Status { get; set; }
        public User User { get; set; }
        public Event Event { get; set; }
        
        // UI-specific properties for button visibility
        public Visibility CheckInVisibility { get; set; }
        public Visibility CheckOutVisibility { get; set; }
    }
} 