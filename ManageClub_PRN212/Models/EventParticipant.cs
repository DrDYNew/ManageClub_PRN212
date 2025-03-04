using System;
using System.Collections.Generic;

namespace ManageClub_PRN212.Models;

public partial class EventParticipant
{
    public int EventParticipantId { get; set; }

    public int EventId { get; set; }

    public int UserId { get; set; }

    public DateTime? RegistrationDate { get; set; }

    public string Status { get; set; } = null!;

    public virtual Event Event { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
