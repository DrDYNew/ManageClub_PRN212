using System;
using System.Collections.Generic;

namespace ManageClub_PRN212.Models;

public partial class ClubFinance
{
    public int FinanceId { get; set; }

    public int ClubId { get; set; }

    public string? TransactionType { get; set; }

    public double Price { get; set; }

    public string? Description { get; set; }

    public DateTime? TransactionDate { get; set; }

    public virtual Club Club { get; set; } = null!;
}
