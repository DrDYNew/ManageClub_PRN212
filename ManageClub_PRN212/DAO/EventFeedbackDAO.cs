using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using ManageClub_PRN212.Models;

namespace ManageClub_PRN212.DAO
{
    public class EventFeedbackDAO
    {
        private readonly ManageClubContext _context;

        public EventFeedbackDAO()
        {
            _context = new ManageClubContext();
        }

        // Add new feedback for an event
        public bool AddFeedback(EventFeedback feedback)
        {
            try
            {
                // Check if this user has already submitted feedback for this event
                if (HasUserSubmittedFeedback(feedback.EventId, feedback.UserId))
                {
                    return false; // User already submitted feedback
                }

                _context.EventFeedbacks.Add(feedback);
                _context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in AddFeedback: {ex.Message}");
                return false;
            }
        }

        // Get all feedback for a specific event
        public List<EventFeedback> GetFeedbackByEvent(int eventId)
        {
            try
            {
                return _context.EventFeedbacks
                    .Include(f => f.Event)
                    .Include(f => f.User)
                    .Where(f => f.EventId == eventId)
                    .OrderByDescending(f => f.FeedbackId) // Show newest first
                    .ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetFeedbackByEvent: {ex.Message}");
                return new List<EventFeedback>();
            }
        }

        // Delete feedback by ID
        public bool DeleteFeedback(int feedbackId)
        {
            try
            {
                var feedback = _context.EventFeedbacks.Find(feedbackId);
                if (feedback != null)
                {
                    _context.EventFeedbacks.Remove(feedback);
                    _context.SaveChanges();
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in DeleteFeedback: {ex.Message}");
                return false;
            }
        }

        // Check if a user has already submitted feedback for an event
        public bool HasUserSubmittedFeedback(int eventId, int userId)
        {
            try
            {
                return _context.EventFeedbacks.Any(f => f.EventId == eventId && f.UserId == userId);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in HasUserSubmittedFeedback: {ex.Message}");
                return false;
            }
        }

        // Get average rating for an event
        public double GetAverageRating(int eventId)
        {
            try
            {
                var feedbacks = _context.EventFeedbacks.Where(f => f.EventId == eventId);
                if (!feedbacks.Any())
                {
                    return 0;
                }
                return feedbacks.Average(f => f.Rating) ?? 0.0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetAverageRating: {ex.Message}");
                return 0;
            }
        }

        // Check if user has participated in the event (to be eligible to submit feedback)
        public bool HasUserParticipatedInEvent(int eventId, int userId)
        {
            try
            {
                return _context.EventParticipants
                    .Any(ep => ep.EventId == eventId && ep.UserId == userId && ep.Status == "Registered");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in HasUserParticipatedInEvent: {ex.Message}");
                return false;
            }
        }
    }
} 