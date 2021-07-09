﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SystemInfo.Wpf.Data;

namespace SystemInfo.Wpf.Migrations
{
    [DbContext(typeof(OfflineApplicationDbContext))]
    [Migration("20210708144422_AddTablePreferences")]
    partial class AddTablePreferences
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "5.0.7");

            modelBuilder.Entity("SystemInfo.Models.Domain.Enterprise", b =>
                {
                    b.Property<string>("RNC")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("ModifiedOn")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.HasKey("RNC");

                    b.ToTable("Enterprises");
                });

            modelBuilder.Entity("SystemInfo.Models.Domain.HardDisk", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("FreeSpaceInGigabytes")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Label")
                        .HasColumnType("TEXT");

                    b.Property<int>("SizeInGigabytes")
                        .HasColumnType("INTEGER");

                    b.Property<int>("SystemSpecsId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("SystemSpecsId");

                    b.ToTable("HardDisk");
                });

            modelBuilder.Entity("SystemInfo.Models.Domain.SystemSpecs", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("TEXT");

                    b.Property<string>("EnterpriseRNC")
                        .HasColumnType("TEXT");

                    b.Property<bool>("IsOperatingSystem64bits")
                        .HasColumnType("INTEGER");

                    b.Property<string>("MachineName")
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("ModifiedOn")
                        .HasColumnType("TEXT");

                    b.Property<string>("OperatingSystemVersion")
                        .HasColumnType("TEXT");

                    b.Property<int>("ProcessorCount")
                        .HasColumnType("INTEGER");

                    b.Property<string>("ProcessorName")
                        .HasColumnType("TEXT");

                    b.Property<int>("TotalMemoryInGigaBytes")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("EnterpriseRNC");

                    b.ToTable("SystemSpecs");
                });

            modelBuilder.Entity("SystemInfo.Wpf.Data.PreferencesKeyValues", b =>
                {
                    b.Property<string>("Key")
                        .HasColumnType("TEXT");

                    b.Property<string>("Value")
                        .HasColumnType("TEXT");

                    b.HasKey("Key");

                    b.ToTable("Preferences");
                });

            modelBuilder.Entity("SystemInfo.Models.Domain.HardDisk", b =>
                {
                    b.HasOne("SystemInfo.Models.Domain.SystemSpecs", "SystemSpecs")
                        .WithMany("HardDisks")
                        .HasForeignKey("SystemSpecsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("SystemSpecs");
                });

            modelBuilder.Entity("SystemInfo.Models.Domain.SystemSpecs", b =>
                {
                    b.HasOne("SystemInfo.Models.Domain.Enterprise", "Enterprise")
                        .WithMany("SystemSpecs")
                        .HasForeignKey("EnterpriseRNC")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.Navigation("Enterprise");
                });

            modelBuilder.Entity("SystemInfo.Models.Domain.Enterprise", b =>
                {
                    b.Navigation("SystemSpecs");
                });

            modelBuilder.Entity("SystemInfo.Models.Domain.SystemSpecs", b =>
                {
                    b.Navigation("HardDisks");
                });
#pragma warning restore 612, 618
        }
    }
}