﻿// <auto-generated />
using System;
using Intex_app.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Intex_app.Migrations.Gamous
{
    [DbContext(typeof(GamousContext))]
    [Migration("20210411002950_Initial2")]
    partial class Initial2
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.5")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Intex_app.Models.ArtifactBioNote", b =>
                {
                    b.Property<string>("Id")
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("varchar(50)")
                        .HasColumnName("id");

                    b.Property<string>("AdditionalNotes")
                        .IsUnicode(false)
                        .HasColumnType("varchar(max)")
                        .HasColumnName("Additional_Notes");

                    b.Property<string>("ArtifactDescription")
                        .HasMaxLength(600)
                        .IsUnicode(false)
                        .HasColumnType("varchar(600)");

                    b.Property<string>("ArtifactFound")
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("varchar(50)");

                    b.Property<string>("BioNotes")
                        .IsUnicode(false)
                        .HasColumnType("varchar(max)");

                    b.Property<string>("BurialWraping")
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("varchar(50)");

                    b.Property<string>("CreatedBy")
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("varchar(50)");

                    b.Property<string>("FaceBundle")
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("varchar(50)");

                    b.Property<string>("LastModifiedBy")
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("varchar(50)");

                    b.Property<DateTime?>("LastModifiedTimestamp")
                        .HasColumnType("datetime");

                    b.Property<string>("PathologyAnomalies")
                        .HasMaxLength(600)
                        .IsUnicode(false)
                        .HasColumnType("varchar(600)");

                    b.Property<string>("PreservationIndex")
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("varchar(50)");

                    b.Property<string>("Rack")
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("varchar(50)");

                    b.Property<string>("SampleTaken")
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("varchar(50)");

                    b.HasKey("Id");

                    b.ToTable("ArtifactBioNote");
                });

            modelBuilder.Entity("Intex_app.Models.Demographic", b =>
                {
                    b.Property<string>("Id")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)")
                        .HasColumnName("id");

                    b.Property<string>("AgeAtDeath")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<byte?>("AgeAtDeathMax")
                        .HasColumnType("tinyint");

                    b.Property<byte?>("AgeAtDeathMin")
                        .HasColumnType("tinyint");

                    b.Property<string>("AgeMethod")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("CreatedBy")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("HairColor")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<byte?>("Height")
                        .HasColumnType("tinyint");

                    b.Property<string>("LastModifiedBy")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<DateTime?>("LastModifiedTimestamp")
                        .HasColumnType("datetime");

                    b.Property<string>("Sex")
                        .HasMaxLength(1)
                        .IsUnicode(false)
                        .HasColumnType("varchar(1)");

                    b.Property<string>("SexGe")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)")
                        .HasColumnName("SexGE");

                    b.HasKey("Id");

                    b.ToTable("Demographic");
                });

            modelBuilder.Entity("Intex_app.Models.LocationMeasurement", b =>
                {
                    b.Property<string>("Id")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)")
                        .HasColumnName("id");

                    b.Property<string>("BurialLength")
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("varchar(50)");

                    b.Property<string>("BurialNum")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("BurialRack")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Cluster")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("CreatedBy")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Depth")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Direction")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("DiscoveryDay")
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("varchar(50)");

                    b.Property<string>("DiscoveryMonth")
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("varchar(50)");

                    b.Property<string>("DiscoveryYear")
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("varchar(50)");

                    b.Property<string>("Eorw")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)")
                        .HasColumnName("EORW");

                    b.Property<string>("FeetSouth")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("FeetWest")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("HeadSouth")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("HeadWest")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("HighEw")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)")
                        .HasColumnName("HighEW");

                    b.Property<string>("HighNs")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)")
                        .HasColumnName("HighNS");

                    b.Property<string>("LastModifiedBy")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<DateTime?>("LastModifiedTimestamp")
                        .HasColumnType("datetime");

                    b.Property<string>("LowEw")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)")
                        .HasColumnName("LowEW");

                    b.Property<string>("LowNs")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)")
                        .HasColumnName("LowNS");

                    b.Property<string>("Nors")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)")
                        .HasColumnName("NORS");

                    b.Property<string>("Square")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("Id");

                    b.ToTable("LocationMeasurement");
                });

            modelBuilder.Entity("Intex_app.Models.Osteology", b =>
                {
                    b.Property<string>("Id")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)")
                        .HasColumnName("id");

                    b.Property<string>("BasilarSuture")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<byte?>("BoneLength")
                        .HasColumnType("tinyint");

                    b.Property<string>("CreatedBy")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<byte?>("DorsalPitting")
                        .HasColumnType("tinyint");

                    b.Property<byte?>("FemurDiameter")
                        .HasColumnType("tinyint");

                    b.Property<byte?>("FemurHead")
                        .HasColumnType("tinyint");

                    b.Property<byte?>("FemurLength")
                        .HasColumnType("tinyint");

                    b.Property<string>("ForemanMagnum")
                        .HasMaxLength(1)
                        .HasColumnType("nvarchar(1)");

                    b.Property<byte?>("Humerus")
                        .HasColumnType("tinyint");

                    b.Property<byte?>("HumerusHead")
                        .HasColumnType("tinyint");

                    b.Property<byte?>("HumerusLength")
                        .HasColumnType("tinyint");

                    b.Property<byte?>("IliacCrest")
                        .HasColumnType("tinyint");

                    b.Property<string>("LastModifiedBy")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<DateTime?>("LastModifiedTimestamp")
                        .HasColumnType("datetime");

                    b.Property<byte?>("MedialClavicle")
                        .HasColumnType("tinyint");

                    b.Property<byte?>("MedialIpramus")
                        .HasColumnType("tinyint")
                        .HasColumnName("MedialIPRamus");

                    b.Property<string>("Osteophytosis")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("PostcraniaTrauma")
                        .HasMaxLength(1)
                        .HasColumnType("nvarchar(1)");

                    b.Property<byte?>("PreaurSulcus")
                        .HasColumnType("tinyint");

                    b.Property<byte?>("PubicBone")
                        .HasColumnType("tinyint");

                    b.Property<string>("PubicSymphysis")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<byte?>("SciaticNotch")
                        .HasColumnType("tinyint");

                    b.Property<byte?>("SubpubicAngle")
                        .HasColumnType("tinyint");

                    b.Property<byte?>("TibiaLength")
                        .HasColumnType("tinyint");

                    b.Property<string>("ToothAttrition")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("ToothEruption")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<byte?>("VentralArc")
                        .HasColumnType("tinyint");

                    b.HasKey("Id");

                    b.ToTable("Osteology");
                });

            modelBuilder.Entity("Intex_app.Models.OsteologySkull", b =>
                {
                    b.Property<string>("Id")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)")
                        .HasColumnName("id");

                    b.Property<byte?>("BasionBregmaHeight")
                        .HasColumnType("tinyint");

                    b.Property<byte?>("BasionNasion")
                        .HasColumnType("tinyint");

                    b.Property<byte?>("BasionProsthionLength")
                        .HasColumnType("tinyint");

                    b.Property<string>("BizygomaticDiameter")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("CranialSuture")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("CreatedBy")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<byte?>("Gonian")
                        .HasColumnType("tinyint");

                    b.Property<string>("InterorbitalBreadth")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("LastModifiedBy")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<DateTime?>("LastModifiedTimestamp")
                        .HasColumnType("datetime");

                    b.Property<byte?>("MaxCranialBreadth")
                        .HasColumnType("tinyint");

                    b.Property<byte?>("MaxCranialLength")
                        .HasColumnType("tinyint");

                    b.Property<byte?>("MaxNasalBreadth")
                        .HasColumnType("tinyint");

                    b.Property<byte?>("NasionProsthion")
                        .HasColumnType("tinyint");

                    b.Property<byte?>("NuchalCrest")
                        .HasColumnType("tinyint");

                    b.Property<byte?>("OrbitEdge")
                        .HasColumnType("tinyint");

                    b.Property<byte?>("ParietalBossing")
                        .HasColumnType("tinyint");

                    b.Property<byte?>("Robust")
                        .HasColumnType("tinyint");

                    b.Property<string>("SkullTrauma")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<byte?>("SupraorbitalRidges")
                        .HasColumnType("tinyint");

                    b.Property<string>("ZygomaticCrest")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("Id");

                    b.ToTable("OsteologySkull");
                });
#pragma warning restore 612, 618
        }
    }
}
