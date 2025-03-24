using System;
using System.Collections.Generic;

namespace ManageClub_PRN212.Models;

public partial class User
{
    public int UserId { get; set; }

    public string FullName { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;

    public int RoleId { get; set; }

    public DateOnly? DateOfBirth { get; set; }

    public string? PhoneNumber { get; set; }

    public string? Address { get; set; }

    public DateTime? DateJoined { get; set; }

    public string Status { get; set; } = null!;

    public virtual ICollection<Attendance> Attendances { get; set; } = new List<Attendance>();

    public virtual ICollection<ClubMember> ClubMembers { get; set; } = new List<ClubMember>();

    public virtual ICollection<Club> Clubs { get; set; } = new List<Club>();

    public virtual ICollection<EventFeedback> EventFeedbacks { get; set; } = new List<EventFeedback>();

    public virtual ICollection<EventParticipant> EventParticipants { get; set; } = new List<EventParticipant>();

    public virtual ICollection<Event> Events { get; set; } = new List<Event>();

    public virtual Role Role { get; set; } = null!;

    public static class SessionDataUser
    {
        public static List<User> users { get; set; } = new List<User>();
    }

    public string UserIdWithName { get => $"{UserId} - {FullName}"; }
}
