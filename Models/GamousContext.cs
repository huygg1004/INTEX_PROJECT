using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace Intex_app.Models
{
    public partial class GamousContext : DbContext
    {
        public GamousContext()
        {
        }

        public GamousContext(DbContextOptions<GamousContext> options)
            : base(options)
        {
        }

        public virtual DbSet<ArtifactBioNote> ArtifactBioNotes { get; set; }
        public virtual DbSet<Demographic> Demographics { get; set; }
        public virtual DbSet<LocationMeasurement> LocationMeasurements { get; set; }
        public virtual DbSet<Osteology> Osteologies { get; set; }
        public virtual DbSet<OsteologySkull> OsteologySkulls { get; set; }

       public static GamousContext Create()
        {
            return new GamousContext();
        }



        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=BENHAUSER6F60\\SQLEXPRESS;Database=Gamous;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<ArtifactBioNote>(entity =>
            {
                entity.ToTable("ArtifactBioNote");

                entity.Property(e => e.Id)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("id");

                entity.Property(e => e.AdditionalNotes)
                    .IsUnicode(false)
                    .HasColumnName("Additional_Notes");

                entity.Property(e => e.ArtifactDescription)
                    .HasMaxLength(600)
                    .IsUnicode(false);

                entity.Property(e => e.ArtifactFound)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.BioNotes).IsUnicode(false);

                entity.Property(e => e.BurialWraping)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedBy)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.FaceBundle)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.LastModifiedBy)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.LastModifiedTimestamp).HasColumnType("datetime");

                entity.Property(e => e.PathologyAnomalies)
                    .HasMaxLength(600)
                    .IsUnicode(false);

                entity.Property(e => e.PreservationIndex)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Rack)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.SampleTaken)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Demographic>(entity =>
            {
                entity.ToTable("Demographic");

                entity.Property(e => e.Id)
                    .HasMaxLength(50)
                    .HasColumnName("id");

                entity.Property(e => e.AgeAtDeath).HasMaxLength(50);

                entity.Property(e => e.AgeMethod).HasMaxLength(50);

                entity.Property(e => e.CreatedBy).HasMaxLength(50);

                entity.Property(e => e.HairColor).HasMaxLength(50);

                entity.Property(e => e.LastModifiedBy).HasMaxLength(50);

                entity.Property(e => e.LastModifiedTimestamp).HasColumnType("datetime");

                entity.Property(e => e.Sex)
                    .HasMaxLength(1)
                    .IsUnicode(false);

                entity.Property(e => e.SexGe)
                    .HasMaxLength(50)
                    .HasColumnName("SexGE");
            });

            modelBuilder.Entity<LocationMeasurement>(entity =>
            {
                entity.ToTable("LocationMeasurement");

                entity.Property(e => e.Id)
                    .HasMaxLength(50)
                    .HasColumnName("id");

                entity.Property(e => e.BurialLength)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.BurialNum).HasMaxLength(50);

                entity.Property(e => e.BurialRack).HasMaxLength(50);

                entity.Property(e => e.Cluster).HasMaxLength(50);

                entity.Property(e => e.CreatedBy).HasMaxLength(50);

                entity.Property(e => e.Depth).HasMaxLength(50);

                entity.Property(e => e.Direction).HasMaxLength(50);

                entity.Property(e => e.DiscoveryDay)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.DiscoveryMonth)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.DiscoveryYear)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Eorw)
                    .HasMaxLength(50)
                    .HasColumnName("EORW");

                entity.Property(e => e.FeetSouth).HasMaxLength(50);

                entity.Property(e => e.FeetWest).HasMaxLength(50);

                entity.Property(e => e.HeadSouth).HasMaxLength(50);

                entity.Property(e => e.HeadWest).HasMaxLength(50);

                entity.Property(e => e.HighEw)
                    .HasMaxLength(50)
                    .HasColumnName("HighEW");

                entity.Property(e => e.HighNs)
                    .HasMaxLength(50)
                    .HasColumnName("HighNS");

                entity.Property(e => e.LastModifiedBy).HasMaxLength(50);

                entity.Property(e => e.LastModifiedTimestamp).HasColumnType("datetime");

                entity.Property(e => e.LowEw)
                    .HasMaxLength(50)
                    .HasColumnName("LowEW");

                entity.Property(e => e.LowNs)
                    .HasMaxLength(50)
                    .HasColumnName("LowNS");

                entity.Property(e => e.Nors)
                    .HasMaxLength(50)
                    .HasColumnName("NORS");

                entity.Property(e => e.Square).HasMaxLength(50);
            });

            modelBuilder.Entity<Osteology>(entity =>
            {
                entity.ToTable("Osteology");

                entity.Property(e => e.Id)
                    .HasMaxLength(50)
                    .HasColumnName("id");

                entity.Property(e => e.BasilarSuture).HasMaxLength(50);

                entity.Property(e => e.CreatedBy).HasMaxLength(50);

                entity.Property(e => e.ForemanMagnum).HasMaxLength(1);

                entity.Property(e => e.LastModifiedBy).HasMaxLength(50);

                entity.Property(e => e.LastModifiedTimestamp).HasColumnType("datetime");

                entity.Property(e => e.MedialIpramus).HasColumnName("MedialIPRamus");

                entity.Property(e => e.Osteophytosis).HasMaxLength(50);

                entity.Property(e => e.PostcraniaTrauma).HasMaxLength(1);

                entity.Property(e => e.PubicSymphysis).HasMaxLength(50);

                entity.Property(e => e.ToothAttrition).HasMaxLength(50);

                entity.Property(e => e.ToothEruption).HasMaxLength(50);
            });

            modelBuilder.Entity<OsteologySkull>(entity =>
            {
                entity.ToTable("OsteologySkull");

                entity.Property(e => e.Id)
                    .HasMaxLength(50)
                    .HasColumnName("id");

                entity.Property(e => e.BizygomaticDiameter).HasMaxLength(50);

                entity.Property(e => e.CranialSuture).HasMaxLength(50);

                entity.Property(e => e.CreatedBy).HasMaxLength(50);

                entity.Property(e => e.InterorbitalBreadth).HasMaxLength(50);

                entity.Property(e => e.LastModifiedBy).HasMaxLength(50);

                entity.Property(e => e.LastModifiedTimestamp).HasColumnType("datetime");

                entity.Property(e => e.SkullTrauma).HasMaxLength(50);

                entity.Property(e => e.ZygomaticCrest).HasMaxLength(50);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
