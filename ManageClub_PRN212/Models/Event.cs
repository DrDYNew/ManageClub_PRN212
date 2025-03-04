using System;
using System.Collections.Generic;

namespace ManageClub_PRN212.Models;

public partial class Event
{
    public int EventId { get; set; }

    public string EventName { get; set; } = null!;

    public string? Description { get; set; }

    public DateTime EventDate { get; set; }

    public string Location { get; set; } = null!;

    public int OrganizerId { get; set; }

    public int? ClubId { get; set; }

    public int? MaxParticipants { get; set; }

    public virtual ICollection<Attendance> Attendances { get; set; } = new List<Attendance>();

    public virtual Club? Club { get; set; }

    public virtual ICollection<EventFeedback> EventFeedbacks { get; set; } = new List<EventFeedback>();

    public virtual ICollection<EventParticipant> EventParticipants { get; set; } = new List<EventParticipant>();

    public virtual User Organizer { get; set; } = null!;
}
