using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using ManageClub_PRN212.Models;

namespace ManageClub_PRN212.DAO
{
    public class AttendanceDAO
    {
        private readonly ManageClubContext _context;

        public AttendanceDAO()
        {
            _context = new ManageClubContext();
        }

        // Record check-in for an event participant
        public bool RecordCheckIn(int eventId, int userId)
        {
            try
            {
                var existing = _context.Attendances
                    .FirstOrDefault(a => a.EventId == eventId && a.UserId == userId);

                if (existing != null)
                {
                    // Update existing record if it exists but doesn't have check-in time
                    if (existing.CheckInTime == null)
                    {
                        existing.CheckInTime = DateTime.Now;
                        _context.SaveChanges();
                        return true;
                    }
                    return false; // Already checked in
                }
                else
                {
                    // Create new attendance record
                    var attendance = new Attendance
                    {
                        EventId = eventId,
                        UserId = userId,
                        CheckInTime = DateTime.Now
                    };
                    _context.Attendances.Add(attendance);
                    _context.SaveChanges();
                    return true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in RecordCheckIn: {ex.Message}");
                return false;
            }
        }

        // Record check-out for an event participant
        public bool RecordCheckOut(int eventId, int userId)
        {
            try
            {
                var existing = _context.Attendances
                    .FirstOrDefault(a => a.EventId == eventId && a.UserId == userId);

                if (existing != null && existing.CheckInTime != null)
                {
                    existing.CheckOutTime = DateTime.Now;
                    _context.SaveChanges();
                    return true;
                }
                return false; // Not checked in yet
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in RecordCheckOut: {ex.Message}");
                return false;
            }
        }

        // Get attendance history for a specific user
        public List<Attendance> GetAttendanceByUser(int userId)
        {
            try
            {
                return _context.Attendances
                    .Include(a => a.Event)
                    .Include(a => a.User)
                    .Where(a => a.UserId == userId)
                    .OrderByDescending(a => a.Event.EventDate)
                    .ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetAttendanceByUser: {ex.Message}");
                return new List<Attendance>();
            }
        }

        // Get attendance for a specific event
        public List<Attendance> GetAttendanceByEvent(int eventId)
        {
            try
            {
                return _context.Attendances
                    .Include(a => a.Event)
                    .Include(a => a.User)
                    .Where(a => a.EventId == eventId)
                    .OrderBy(a => a.User.FullName)
                    .ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetAttendanceByEvent: {ex.Message}");
                return new List<Attendance>();
            }
        }

        // Get specific attendance record
        public Attendance GetAttendance(int eventId, int userId)
        {
            try
            {
                return _context.Attendances
                    .Include(a => a.Event)
                    .Include(a => a.User)
                    .FirstOrDefault(a => a.EventId == eventId && a.UserId == userId);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetAttendance: {ex.Message}");
                return null;
            }
        }
    }
} 