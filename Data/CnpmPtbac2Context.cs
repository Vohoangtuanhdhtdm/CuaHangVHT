using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace CuaHangVHT.Data;

public partial class CnpmPtbac2Context : DbContext
{
    public CnpmPtbac2Context()
    {
    }

    public CnpmPtbac2Context(DbContextOptions<CnpmPtbac2Context> options)
        : base(options)
    {
    }

    public virtual DbSet<LichSuKetQua> LichSuKetQuas { get; set; }

    public virtual DbSet<LichSuNguoiDung> LichSuNguoiDungs { get; set; }

    public virtual DbSet<NguoiDung> NguoiDungs { get; set; }

    public virtual DbSet<Ptbac2> Ptbac2s { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=MSI\\SQLEXPRESS;Initial Catalog=CNPM_PTBAC2;Integrated Security=True;Trust Server Certificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<LichSuKetQua>(entity =>
        {
            entity.HasKey(e => e.IdlichSu).HasName("PK__LichSuKe__BE9CDA92E9FAF987");

            entity.ToTable("LichSuKetQua");

            entity.Property(e => e.DateCalculated)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.IdPtbac2).HasColumnName("IdPTBac2");
            entity.Property(e => e.Root1).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Root2).HasColumnType("decimal(18, 2)");

            entity.HasOne(d => d.IdPtbac2Navigation).WithMany(p => p.LichSuKetQuas)
                .HasForeignKey(d => d.IdPtbac2)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__LichSuKet__IdPTB__628FA481");
        });

        modelBuilder.Entity<LichSuNguoiDung>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__LichSuNg__3214EC07E8195B87");

            entity.ToTable("LichSuNguoiDung");

            entity.Property(e => e.DateUsed)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.IdNguoiDungNavigation).WithMany(p => p.LichSuNguoiDungs)
                .HasForeignKey(d => d.IdNguoiDung)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__LichSuNgu__IdNgu__66603565");

            entity.HasOne(d => d.IdlichSuNavigation).WithMany(p => p.LichSuNguoiDungs)
                .HasForeignKey(d => d.IdlichSu)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__LichSuNgu__Idlic__6754599E");
        });

        modelBuilder.Entity<NguoiDung>(entity =>
        {
            entity.HasKey(e => e.IdNguoiDung).HasName("PK__NguoiDun__FEE82D40FDA221B7");

            entity.ToTable("NguoiDung");

            entity.HasIndex(e => e.UserName, "UQ__NguoiDun__C9F284562E62B2DF").IsUnique();

            entity.Property(e => e.DateCreated)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Email).HasMaxLength(255);
            entity.Property(e => e.PasswordHash).HasMaxLength(256);
            entity.Property(e => e.UserName).HasMaxLength(100);
        });

        modelBuilder.Entity<Ptbac2>(entity =>
        {
            entity.HasKey(e => e.IdPtbac2).HasName("PK__PTBac2__A0742A780F2C181A");

            entity.ToTable("PTBac2");

            entity.Property(e => e.IdPtbac2).HasColumnName("IdPTBac2");
            entity.Property(e => e.A).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.B).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.C).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.DateCreated)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
