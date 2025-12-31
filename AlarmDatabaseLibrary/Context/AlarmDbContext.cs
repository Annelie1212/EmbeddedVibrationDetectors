using System;
using System.Collections.Generic;
using AlarmDatabaseLibrary.Models;
using Microsoft.EntityFrameworkCore;

namespace AlarmDatabaseLibrary.Context;

public partial class AlarmDbContext : DbContext
{
    public AlarmDbContext()
    {
    }

    public AlarmDbContext(DbContextOptions<AlarmDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<VibrationDetectorStatusLog> VibrationDetectorStatusLogs { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    { }
//#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
//    => optionsBuilder.UseSqlServer("Server=ASUSAG24;Database=AlarmDatabase;Trusted_Connection=True;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<VibrationDetectorStatusLog>(entity =>
        {
            entity.ToTable("VibrationDetectorStatusLog");
            entity.HasKey(e => e.VibrationDetectorStatusLogId);

            entity.Property(e => e.VibrationDetectorStatusLogId)
                  .ValueGeneratedOnAdd(); 

            entity.Property(e => e.DeviceAction)
                .HasMaxLength(15)
                .IsFixedLength();
            entity.Property(e => e.DeviceName)
                .HasMaxLength(25)
                .IsFixedLength();
            entity.Property(e => e.Location)
                .HasMaxLength(20)
                .IsFixedLength();
            entity.Property(e => e.LogMessage)
                .HasMaxLength(100)
                .IsFixedLength();

        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
