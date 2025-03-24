using System;
using System.Collections.Generic;

namespace ManageClub_PRN212.Models;

public partial class Club
{
    public int ClubId { get; set; }

    public string ClubName { get; set; } = null!;

    public string? Description { get; set; }

    public DateOnly? EstablishedDate { get; set; }

    public int? PresidentId { get; set; }

    public string ClubStatus { get; set; } = null!;

    public decimal? TotalCost { get; set; }

    public virtual ICollection<ClubFinance> ClubFinances { get; set; } = new List<ClubFinance>();

    public virtual ICollection<ClubMember> ClubMembers { get; set; } = new List<ClubMember>();

    public virtual ICollection<Event> Events { get; set; } = new List<Event>();

    public virtual User? President { get; set; }

    public virtual ICollection<Report> Reports { get; set; } = new List<Report>();
}
