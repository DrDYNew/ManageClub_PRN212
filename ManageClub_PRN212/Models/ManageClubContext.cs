using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace ManageClub_PRN212.Models;

public partial class ManageClubContext : DbContext
{
    public ManageClubContext()
    {
    }

    public ManageClubContext(DbContextOptions<ManageClubContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Attendance> Attendances { get; set; }

    public virtual DbSet<Club> Clubs { get; set; }

    public virtual DbSet<ClubFinance> ClubFinances { get; set; }

    public virtual DbSet<ClubMember> ClubMembers { get; set; }

    public virtual DbSet<Event> Events { get; set; }

    public virtual DbSet<EventFeedback> EventFeedbacks { get; set; }

    public virtual DbSet<EventParticipant> EventParticipants { get; set; }

    public virtual DbSet<Report> Reports { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=(local);Database=ManageClub;Uid=sa;Pwd=123456;Encrypt=False");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Attendance>(entity =>
        {
            entity.HasKey(e => e.AttendanceId).HasName("PK__Attendan__8B69263C0603277F");

            entity.ToTable("Attendance");

            entity.Property(e => e.AttendanceId).HasColumnName("AttendanceID");
            entity.Property(e => e.CheckInTime).HasColumnType("datetime");
            entity.Property(e => e.CheckOutTime).HasColumnType("datetime");
            entity.Property(e => e.EventId).HasColumnName("EventID");
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.Event).WithMany(p => p.Attendances)
                .HasForeignKey(d => d.EventId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Attendanc__Event__4E88ABD4");

            entity.HasOne(d => d.User).WithMany(p => p.Attendances)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Attendanc__UserI__4F7CD00D");
        });

        modelBuilder.Entity<Club>(entity =>
        {
            entity.HasKey(e => e.ClubId).HasName("PK__Clubs__D35058C7A298BE00");

            entity.Property(e => e.ClubId).HasColumnName("ClubID");
            entity.Property(e => e.ClubName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.ClubStatus).HasMaxLength(20);
            entity.Property(e => e.Description).HasColumnType("text");
            entity.Property(e => e.PresidentId).HasColumnName("PresidentID");
            entity.Property(e => e.TotalCost)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("Total_cost");

            entity.HasOne(d => d.President).WithMany(p => p.Clubs)
                .HasForeignKey(d => d.PresidentId)
                .HasConstraintName("FK__Clubs__President__534D60F1");
        });

        modelBuilder.Entity<ClubFinance>(entity =>
        {
            entity.HasKey(e => e.FinanceId).HasName("PK__ClubFina__7917A8FF7A343B14");

            entity.Property(e => e.FinanceId).HasColumnName("FinanceID");
            entity.Property(e => e.Amount).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.ClubId).HasColumnName("ClubID");
            entity.Property(e => e.Description).HasColumnType("text");
            entity.Property(e => e.TransactionDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.TransactionType).HasMaxLength(10);

            entity.HasOne(d => d.Club).WithMany(p => p.ClubFinances)
                .HasForeignKey(d => d.ClubId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ClubFinan__ClubI__5070F446");
        });

        modelBuilder.Entity<ClubMember>(entity =>
        {
            entity.HasKey(e => e.MembershipId).HasName("PK__ClubMemb__92A78599AA195D14");

            entity.Property(e => e.MembershipId).HasColumnName("MembershipID");
            entity.Property(e => e.ClubId).HasColumnName("ClubID");
            entity.Property(e => e.JoinDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.MemberStatus)
                .HasMaxLength(20)
                .HasColumnName("Member_Status");
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.Club).WithMany(p => p.ClubMembers)
                .HasForeignKey(d => d.ClubId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ClubMembe__ClubI__5165187F");

            entity.HasOne(d => d.User).WithMany(p => p.ClubMembers)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ClubMembe__UserI__52593CB8");
        });

        modelBuilder.Entity<Event>(entity =>
        {
            entity.HasKey(e => e.EventId).HasName("PK__Events__7944C8706B4A070C");

            entity.Property(e => e.EventId).HasColumnName("EventID");
            entity.Property(e => e.ClubId).HasColumnName("ClubID");
            entity.Property(e => e.Description).HasColumnType("text");
            entity.Property(e => e.EventDate).HasColumnType("datetime");
            entity.Property(e => e.EventName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.EventStatus)
                .HasMaxLength(20)
                .HasColumnName("Event_Status");
            entity.Property(e => e.Location).HasMaxLength(200);
            entity.Property(e => e.OrganizerId).HasColumnName("OrganizerID");

            entity.HasOne(d => d.Club).WithMany(p => p.Events)
                .HasForeignKey(d => d.ClubId)
                .HasConstraintName("FK__Events__ClubID__5812160E");

            entity.HasOne(d => d.Organizer).WithMany(p => p.Events)
                .HasForeignKey(d => d.OrganizerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Events__Organize__59063A47");
        });

        modelBuilder.Entity<EventFeedback>(entity =>
        {
            entity.HasKey(e => e.FeedbackId).HasName("PK__EventFee__6A4BEDF64546EE98");

            entity.ToTable("EventFeedback");

            entity.Property(e => e.FeedbackId).HasColumnName("FeedbackID");
            entity.Property(e => e.Comments).HasColumnType("text");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.EventId).HasColumnName("EventID");
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.Event).WithMany(p => p.EventFeedbacks)
                .HasForeignKey(d => d.EventId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__EventFeed__Event__5441852A");

            entity.HasOne(d => d.User).WithMany(p => p.EventFeedbacks)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__EventFeed__UserI__5535A963");
        });

        modelBuilder.Entity<EventParticipant>(entity =>
        {
            entity.HasKey(e => e.EventParticipantId).HasName("PK__EventPar__09F32B72ABB6C902");

            entity.Property(e => e.EventParticipantId).HasColumnName("EventParticipantID");
            entity.Property(e => e.EventId).HasColumnName("EventID");
            entity.Property(e => e.Reason).HasMaxLength(200);
            entity.Property(e => e.RegistrationDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Status).HasMaxLength(15);
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.Event).WithMany(p => p.EventParticipants)
                .HasForeignKey(d => d.EventId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__EventPart__Event__5629CD9C");

            entity.HasOne(d => d.User).WithMany(p => p.EventParticipants)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__EventPart__UserI__571DF1D5");
        });

        modelBuilder.Entity<Report>(entity =>
        {
            entity.HasKey(e => e.ReportId).HasName("PK__Reports__D5BD48E541811A6D");

            entity.Property(e => e.ReportId).HasColumnName("ReportID");
            entity.Property(e => e.ClubId).HasColumnName("ClubID");
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.EventSummary).HasColumnType("text");
            entity.Property(e => e.MemberChanges).HasColumnType("text");
            entity.Property(e => e.ParticipationStats).HasColumnType("text");
            entity.Property(e => e.Semester).HasMaxLength(20);

            entity.HasOne(d => d.Club).WithMany(p => p.Reports)
                .HasForeignKey(d => d.ClubId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Reports__ClubID__59FA5E80");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.RoleId).HasName("PK__Roles__8AFACE3A50426075");

            entity.HasIndex(e => e.RoleName, "UQ__Roles__8A2B6160A843368B").IsUnique();

            entity.Property(e => e.RoleId).HasColumnName("RoleID");
            entity.Property(e => e.RoleName).HasMaxLength(50);
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__Users__1788CCAC1C8931A9");

            entity.HasIndex(e => e.Email, "UQ__Users__A9D105343E88CE8C").IsUnique();

            entity.Property(e => e.UserId).HasColumnName("UserID");
            entity.Property(e => e.Address).HasColumnType("text");
            entity.Property(e => e.AvatarUrl).HasColumnName("AvatarURL");
            entity.Property(e => e.DateJoined)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.FullName).HasMaxLength(100);
            entity.Property(e => e.Password).HasMaxLength(255);
            entity.Property(e => e.PhoneNumber).HasMaxLength(15);
            entity.Property(e => e.RoleId).HasColumnName("RoleID");
            entity.Property(e => e.Status)
                .HasMaxLength(20)
                .IsUnicode(false);

            entity.HasOne(d => d.Role).WithMany(p => p.Users)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Users__RoleID__5AEE82B9");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
