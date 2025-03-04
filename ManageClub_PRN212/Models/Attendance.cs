using System;
using System.Collections.Generic;

namespace ManageClub_PRN212.Models;

public partial class Attendance
{
    public int AttendanceId { get; set; }

    public int EventId { get; set; }

    public int UserId { get; set; }

    public DateTime? CheckInTime { get; set; }

    public DateTime? CheckOutTime { get; set; }

    public virtual Event Event { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
