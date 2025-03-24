using System;
using System.Windows;
using ManageClub_PRN212.Models;

namespace ManageClub_PRN212.ViewModels
{
    public class FeedbackViewModel
    {
        public int FeedbackId { get; set; }
        public int EventId { get; set; }
        public int UserId { get; set; }
        public int Rating { get; set; }
        public string Comments { get; set; }
        public User User { get; set; }
        public Event Event { get; set; }
        
        // UI-specific properties
        public string UserFullName => User?.FullName;
        public DateTime? UserJoinDate => User?.DateJoined;
        public string FormattedDate => User?.DateJoined?.ToString("dd/MM/yyyy");
        
        // Delete button visibility based on user role
        public Visibility DeleteButtonVisibility { get; set; }
        
        // Star rating display (for UI)
        public string Stars => new string('★', Rating) + new string('☆', 5 - Rating);
        
        // Rating style colors (for UI)
        public string RatingColor
        {
            get
            {
                return Rating switch
                {
                    5 => "#4CAF50", // Excellent (Green)
                    4 => "#8BC34A", // Very Good (Light Green)
                    3 => "#FFC107", // Good (Yellow)
                    2 => "#FF9800", // Fair (Orange)
                    _ => "#F44336", // Poor (Red)
                };
            }
        }
    }
} 