using ManageClub_PRN212.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManageClub_PRN212.DAO
{
    public class EventDAO
    {
        private readonly ManageClubContext _context;

        public EventDAO()
        {
            _context = new ManageClubContext();
        }

        public List<Event> GetEventsByUser(int userId)
        {
            try
            {
                var events = _context.Events
                    .Include(e => e.Organizer)
                    .Include(e => e.Club)
                    .Where(e => e.OrganizerId == userId || (e.Club != null && e.Club.PresidentId == userId))
                    .ToList();
                return events;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetEventsByUser: {ex.Message}");
                return new List<Event>();
            }
        }

        public List<Club> GetClubsManagedByUser(int userId)
        {
            try
            {
                var clubs = _context.Clubs
                    .Where(c => c.PresidentId == userId)
                    .ToList();
                return clubs;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetClubsManagedByUser: {ex.Message}");
                return new List<Club>();
            }
        }

        public User GetUserById(int userId)
        {
            try
            {
                var user = _context.Users
                    .FirstOrDefault(u => u.UserId == userId);
                return user;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetUserById: {ex.Message}");
                return null;
            }
        }

        public void AddEvent(Event newEvent)
        {
            try
            {
                _context.Events.Add(newEvent);
                _context.SaveChanges();
                Console.WriteLine($"Added event: {newEvent.EventName}, EventDate: {newEvent.EventDate.ToString("dd/MM/yyyy")}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in AddEvent: {ex.Message}");
            }
        }

        public void UpdateEvent(Event updatedEvent)
        {
            try
            {
                _context.Entry(updatedEvent).State = EntityState.Modified;
                _context.SaveChanges();
                Console.WriteLine($"Updated event: {updatedEvent.EventName}, EventDate: {updatedEvent.EventDate.ToString("dd/MM/yyyy")}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in UpdateEvent: {ex.Message}");
            }
        }

        public void DeleteEvent(int eventId)
        {
            try
            {
                var eventToDelete = _context.Events.Find(eventId);
                if (eventToDelete != null)
                {
                    _context.Events.Remove(eventToDelete);
                    _context.SaveChanges();
                    Console.WriteLine($"Deleted event: {eventToDelete.EventName}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in DeleteEvent: {ex.Message}");
            }
        }

        public List<Event> GetAllEvents()
        {
            try
            {
                return _context.Events
                    .Include(e => e.Organizer)
                    .Include(e => e.Club)
                    .ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetAllEvents: {ex.Message}");
                return new List<Event>();
            }
        }

        public int GetParticipantCount(int eventId)
        {
            try
            {
                return _context.EventParticipants
                    .Count(ep => ep.EventId == eventId && ep.Status == "Registered");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetParticipantCount: {ex.Message}");
                return 0;
            }
        }

        public bool IsUserRegistered(int eventId, int userId)
        {
            try
            {
                return _context.EventParticipants
                    .Any(ep => ep.EventId == eventId && ep.UserId == userId && ep.Status == "Registered");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in IsUserRegistered: {ex.Message}");
                return false;
            }
        }

        public void AddEventParticipant(EventParticipant participant)
        {
            try
            {
                _context.EventParticipants.Add(participant);
                _context.SaveChanges();
                Console.WriteLine($"Added participant: EventId={participant.EventId}, UserId={participant.UserId}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in AddEventParticipant: {ex.Message}");
            }
        }
        public List<Event> GetEventsForMember(int userId)
        {
            try
            {
                // Lấy danh sách các ClubId mà user đã tham gia
                var joinedClubIds = _context.ClubMembers
                    .Where(cm => cm.UserId == userId && cm.MemberStatus == "Active") // Giả sử "Active" là trạng thái tham gia
                    .Select(cm => cm.ClubId)
                    .ToList();

                // Lấy các sự kiện từ những câu lạc bộ mà user đã tham gia
                return _context.Events
                    .Include(e => e.Organizer)
                    .Include(e => e.Club)
                    .Where(e => joinedClubIds.Contains(e.ClubId.Value))
                    .ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetEventsForMember: {ex.Message}");
                return new List<Event>();
            }
        }
        public List<EventParticipant> GetAllEventParticipants()
        {
            try
            {
                return _context.EventParticipants
                    .Include(ep => ep.Event)
                    .Include(ep => ep.User)
                    .ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetAllEventParticipants: {ex.Message}");
                return new List<EventParticipant>();
            }
        }

        public void UpdateEventParticipant(EventParticipant participant)
        {
            try
            {
                _context.Entry(participant).State = EntityState.Modified;
                _context.SaveChanges();
                Console.WriteLine($"Updated participant: EventId={participant.EventId}, UserId={participant.UserId}, Status={participant.Status}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in UpdateEventParticipant: {ex.Message}");
            }
        }

        public List<User> GetUsersByEvent(int eventId)
        {
            try
            {
                // Get all users who are participants in the event
                var users = _context.EventParticipants
                    .Where(ep => ep.EventId == eventId)
                    .Include(ep => ep.User)
                    .Select(ep => ep.User)
                    .ToList();

                return users;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetUsersByEvent: {ex.Message}");
                return new List<User>();
            }
        }
    }
}
