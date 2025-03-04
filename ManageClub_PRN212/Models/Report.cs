using System;
using System.Collections.Generic;

namespace ManageClub_PRN212.Models;

public partial class Report
{
    public int ReportId { get; set; }

    public int ClubId { get; set; }

    public string Semester { get; set; } = null!;

    public string? MemberChanges { get; set; }

    public string? EventSummary { get; set; }

    public string? ParticipationStats { get; set; }

    public DateTime? CreatedDate { get; set; }

    public virtual Club Club { get; set; } = null!;
}
