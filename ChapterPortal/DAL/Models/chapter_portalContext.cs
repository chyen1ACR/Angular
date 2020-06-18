using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace ChapterPortal.DAL.Models
{
    public partial class chapter_portalContext : DbContext
    {
        public chapter_portalContext()
        {
        }

        public chapter_portalContext(DbContextOptions<chapter_portalContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Announcement> Announcement { get; set; }
        public virtual DbSet<ChapterLookUp> ChapterLookUp { get; set; }
        public virtual DbSet<ChapterOfficerDetails> ChapterOfficerDetails { get; set; }
        public virtual DbSet<ChildFolders> ChildFolders { get; set; }
        public virtual DbSet<Files> Files { get; set; }
        public virtual DbSet<Folders> Folders { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
//#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Data Source=.;Initial Catalog=chapter_portal;Persist Security Info=True;User ID=sa;Password=welcome1!");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Announcement>(entity =>
            {
                entity.Property(e => e.ChpaterId).HasMaxLength(20);

                entity.Property(e => e.EndDate)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.ModifiedDate)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.StartDate)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<ChapterLookUp>(entity =>
            {
                entity.Property(e => e.ChapterId)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.Property(e => e.ChapterName).IsRequired();
            });

            modelBuilder.Entity<ChapterOfficerDetails>(entity =>
            {
                entity.Property(e => e.CustomerId)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.Property(e => e.FullName).IsRequired();

                entity.Property(e => e.PositionBeginDate).HasMaxLength(100);

                entity.Property(e => e.PositionEndDate).HasMaxLength(100);

                entity.Property(e => e.PrimaryEmail)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.PrimaryPhone).HasMaxLength(20);
            });

            modelBuilder.Entity<ChildFolders>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.ChildFolderId).HasColumnName("ChildFolderID");

                entity.Property(e => e.FolderId).HasColumnName("FolderID");

                entity.HasOne(d => d.ChildFolder)
                    .WithMany(p => p.ChildFoldersChildFolder)
                    .HasForeignKey(d => d.ChildFolderId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ChildFolders_ChildFolder");

                entity.HasOne(d => d.Folder)
                    .WithMany(p => p.ChildFoldersFolder)
                    .HasForeignKey(d => d.FolderId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ChildFolders_Folders");
            });

            modelBuilder.Entity<Files>(entity =>
            {
                entity.HasKey(e => e.FileId);

                entity.Property(e => e.FileId).HasColumnName("FileID");

                entity.Property(e => e.CreatedBy)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.FilePath)
                    .HasMaxLength(5000)
                    .IsUnicode(false);

                entity.Property(e => e.FolderId).HasColumnName("FolderID");

                entity.Property(e => e.ModifiedBy)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.HasOne(d => d.Folder)
                    .WithMany(p => p.Files)
                    .HasForeignKey(d => d.FolderId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Files_Folders");
            });

            modelBuilder.Entity<Folders>(entity =>
            {
                entity.HasKey(e => e.FolderId);

                entity.Property(e => e.FolderId).HasColumnName("FolderID");

                entity.Property(e => e.CreatedBy)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.FolderName)
                    .IsRequired()
                    .HasMaxLength(250);

                entity.Property(e => e.ModifiedBy)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.ParentId).HasColumnName("ParentID");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
