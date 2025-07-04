﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Portfolio.Data.Data;

#nullable disable

namespace Portfolio.Data.Migrations
{
    [DbContext(typeof(PortfolioDbContext))]
    [Migration("20250622194459_Add Tags Class")]
    partial class AddTagsClass
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.17")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Portfolio.Data.Entities.Link", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("PortfolioUserId")
                        .HasColumnType("int");

                    b.Property<int>("Type")
                        .HasColumnType("int");

                    b.Property<string>("Url")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("PortfolioUserId");

                    b.ToTable("SocialLinks");
                });

            modelBuilder.Entity("Portfolio.Data.Entities.PortfolioUser", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("About")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("MainPhotoUrl")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TokenName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("Portfolio.Data.Entities.Project", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("GitHubUrl")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsTop")
                        .HasColumnType("bit");

                    b.Property<string>("LiveUrl")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhotoUrl")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("PortfolioUserId")
                        .HasColumnType("int");

                    b.Property<string>("Tools")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("PortfolioUserId");

                    b.ToTable("Projects");
                });

            modelBuilder.Entity("Portfolio.Data.Entities.Skill", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("PortfolioUserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("PortfolioUserId");

                    b.ToTable("Skills");
                });

            modelBuilder.Entity("Portfolio.Data.Entities.Tag", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("SkillId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("SkillId");

                    b.ToTable("Tags");
                });

            modelBuilder.Entity("Portfolio.Data.Entities.Link", b =>
                {
                    b.HasOne("Portfolio.Data.Entities.PortfolioUser", "PortfolioUser")
                        .WithMany("Links")
                        .HasForeignKey("PortfolioUserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("PortfolioUser");
                });

            modelBuilder.Entity("Portfolio.Data.Entities.Project", b =>
                {
                    b.HasOne("Portfolio.Data.Entities.PortfolioUser", "PortfolioUser")
                        .WithMany("Projects")
                        .HasForeignKey("PortfolioUserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("PortfolioUser");
                });

            modelBuilder.Entity("Portfolio.Data.Entities.Skill", b =>
                {
                    b.HasOne("Portfolio.Data.Entities.PortfolioUser", "portfolioUser")
                        .WithMany("Skills")
                        .HasForeignKey("PortfolioUserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("portfolioUser");
                });

            modelBuilder.Entity("Portfolio.Data.Entities.Tag", b =>
                {
                    b.HasOne("Portfolio.Data.Entities.Skill", "Skill")
                        .WithMany("Tags")
                        .HasForeignKey("SkillId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Skill");
                });

            modelBuilder.Entity("Portfolio.Data.Entities.PortfolioUser", b =>
                {
                    b.Navigation("Links");

                    b.Navigation("Projects");

                    b.Navigation("Skills");
                });

            modelBuilder.Entity("Portfolio.Data.Entities.Skill", b =>
                {
                    b.Navigation("Tags");
                });
#pragma warning restore 612, 618
        }
    }
}
