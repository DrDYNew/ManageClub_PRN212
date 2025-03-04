using System;
using System.Collections.Generic;

namespace ManageClub_PRN212.Models;

public partial class ClubMember
{
    public int MembershipId { get; set; }

    public int ClubId { get; set; }

    public int UserId { get; set; }

    public DateTime? JoinDate { get; set; }

    public string? Position { get; set; }

    public string? Status { get; set; }

    public virtual Club Club { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
