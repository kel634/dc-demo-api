using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace dc_demo_api.Models
{
    public partial class DCDemoContext : DbContext
    {
        public DCDemoContext()
        {
        }

        public DCDemoContext(DbContextOptions<DCDemoContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Asset> Asset { get; set; }
        public virtual DbSet<AssetMetadata> AssetMetadata { get; set; }
        public virtual DbSet<AssetVariant> AssetVariant { get; set; }
        public virtual DbSet<Folder> Folder { get; set; }
        public virtual DbSet<VariantType> VariantType { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Asset>(entity =>
            {
                entity.Property(e => e.DisplayName)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.FileName)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.PreviewUrl)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.HasOne(d => d.Folder)
                    .WithMany(p => p.Asset)
                    .HasForeignKey(d => d.FolderId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Asset_fk0");
            });

            modelBuilder.Entity<AssetMetadata>(entity =>
            {
                entity.HasOne(d => d.Asset)
                    .WithMany(p => p.AssetMetadata)
                    .HasForeignKey(d => d.AssetId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("AssetMetadata_fk0");
            });

            modelBuilder.Entity<AssetVariant>(entity =>
            {
                entity.Property(e => e.Url)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.HasOne(d => d.Asset)
                    .WithMany(p => p.AssetVariant)
                    .HasForeignKey(d => d.AssetId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("AssetVariant_fk0");

                entity.HasOne(d => d.VariantType)
                    .WithMany(p => p.AssetVariant)
                    .HasForeignKey(d => d.VariantTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("AssetVariant_fk1");
            });

            modelBuilder.Entity<Folder>(entity =>
            {
                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.HasOne(d => d.Parent)
                    .WithMany(p => p.InverseParent)
                    .HasForeignKey(d => d.ParentId)
                    .HasConstraintName("Folder_fk0");
            });

            modelBuilder.Entity<VariantType>(entity =>
            {
                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
