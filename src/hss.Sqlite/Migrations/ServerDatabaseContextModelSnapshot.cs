﻿// <auto-generated />

using System;
using HacknetSharp.Server;
using HacknetSharp.Server.EF;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace hss.Sqlite.Migrations
{
    [DbContext(typeof(ServerDatabaseContext))]
    partial class ServerDatabaseContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "5.0.0");

            modelBuilder.Entity("HacknetSharp.Server.Models.CronModel", b =>
            {
                b.Property<Guid>("Key")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("TEXT");

                b.Property<string>("Content")
                    .IsRequired()
                    .HasColumnType("TEXT");

                b.Property<double>("Delay")
                    .HasColumnType("REAL");

                b.Property<double>("End")
                    .HasColumnType("REAL");

                b.Property<double>("LastRunAt")
                    .HasColumnType("REAL");

                b.Property<Guid>("SystemKey")
                    .HasColumnType("TEXT");

                b.Property<Guid?>("WorldKey")
                    .HasColumnType("TEXT");

                b.HasKey("Key");

                b.HasIndex("SystemKey");

                b.HasIndex("WorldKey");

                b.ToTable("CronModel");
            });

            modelBuilder.Entity("HacknetSharp.Server.Models.FileModel", b =>
            {
                b.Property<Guid>("Key")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("TEXT");

                b.Property<string>("Content")
                    .HasColumnType("TEXT");

                b.Property<byte>("Execute")
                    .HasColumnType("INTEGER");

                b.Property<bool>("Hidden")
                    .HasColumnType("INTEGER");

                b.Property<byte>("Kind")
                    .HasColumnType("INTEGER");

                b.Property<string>("Name")
                    .IsRequired()
                    .HasColumnType("TEXT");

                b.Property<Guid?>("OwnerKey")
                    .HasColumnType("TEXT");

                b.Property<string>("Path")
                    .IsRequired()
                    .HasColumnType("TEXT");

                b.Property<byte>("Read")
                    .HasColumnType("INTEGER");

                b.Property<Guid>("SystemKey")
                    .HasColumnType("TEXT");

                b.Property<Guid?>("WorldKey")
                    .HasColumnType("TEXT");

                b.Property<byte>("Write")
                    .HasColumnType("INTEGER");

                b.HasKey("Key");

                b.HasIndex("OwnerKey");

                b.HasIndex("SystemKey");

                b.HasIndex("WorldKey");

                b.ToTable("FileModel");
            });

            modelBuilder.Entity("HacknetSharp.Server.Models.KnownSystemModel", b =>
            {
                b.Property<Guid>("FromKey")
                    .HasColumnType("TEXT");

                b.Property<Guid>("ToKey")
                    .HasColumnType("TEXT");

                b.Property<Guid>("Key")
                    .HasColumnType("TEXT");

                b.Property<bool>("Local")
                    .HasColumnType("INTEGER");

                b.Property<Guid?>("WorldKey")
                    .HasColumnType("TEXT");

                b.HasKey("FromKey", "ToKey");

                b.HasIndex("ToKey");

                b.HasIndex("WorldKey");

                b.ToTable("KnownSystemModel");
            });

            modelBuilder.Entity("HacknetSharp.Server.Models.LoginModel", b =>
            {
                b.Property<Guid>("Key")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("TEXT");

                b.Property<bool>("Admin")
                    .HasColumnType("INTEGER");

                b.Property<byte[]>("Hash")
                    .IsRequired()
                    .HasColumnType("BLOB");

                b.Property<Guid>("Person")
                    .HasColumnType("TEXT");

                b.Property<byte[]>("Salt")
                    .IsRequired()
                    .HasColumnType("BLOB");

                b.Property<Guid>("SystemKey")
                    .HasColumnType("TEXT");

                b.Property<string>("User")
                    .IsRequired()
                    .HasColumnType("TEXT");

                b.Property<Guid?>("WorldKey")
                    .HasColumnType("TEXT");

                b.HasKey("Key");

                b.HasIndex("SystemKey");

                b.HasIndex("WorldKey");

                b.ToTable("LoginModel");
            });

            modelBuilder.Entity("HacknetSharp.Server.Models.MissionModel", b =>
            {
                b.Property<Guid>("Key")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("TEXT");

                b.Property<Guid>("CampaignKey")
                    .HasColumnType("TEXT");

                b.Property<long>("Flags")
                    .HasColumnType("INTEGER");

                b.Property<Guid>("PersonKey")
                    .HasColumnType("TEXT");

                b.Property<string>("Template")
                    .IsRequired()
                    .HasColumnType("TEXT");

                b.Property<Guid?>("WorldKey")
                    .HasColumnType("TEXT");

                b.HasKey("Key");

                b.HasIndex("PersonKey");

                b.HasIndex("WorldKey");

                b.ToTable("MissionModel");
            });

            modelBuilder.Entity("HacknetSharp.Server.Models.PersonModel", b =>
            {
                b.Property<Guid>("Key")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("TEXT");

                b.Property<double>("ClockSpeed")
                    .HasColumnType("REAL");

                b.Property<Guid>("DefaultSystem")
                    .HasColumnType("TEXT");

                b.Property<int>("DiskCapacity")
                    .HasColumnType("INTEGER");

                b.Property<string>("Name")
                    .IsRequired()
                    .HasColumnType("TEXT");

                b.Property<double>("ProxyClocks")
                    .HasColumnType("REAL");

                b.Property<double>("RebootDuration")
                    .HasColumnType("REAL");

                b.Property<Guid>("SpawnGroup")
                    .HasColumnType("TEXT");

                b.Property<bool>("StartedUp")
                    .HasColumnType("INTEGER");

                b.Property<long>("SystemMemory")
                    .HasColumnType("INTEGER");

                b.Property<string>("Tag")
                    .HasColumnType("TEXT");

                b.Property<string>("UserKey")
                    .HasColumnType("TEXT");

                b.Property<string>("UserName")
                    .IsRequired()
                    .HasColumnType("TEXT");

                b.Property<Guid>("WorldKey")
                    .HasColumnType("TEXT");

                b.HasKey("Key");

                b.HasIndex("UserKey");

                b.HasIndex("WorldKey");

                b.ToTable("PersonModel");
            });

            modelBuilder.Entity("HacknetSharp.Server.Models.RegistrationToken", b =>
            {
                b.Property<string>("Key")
                    .HasColumnType("TEXT");

                b.Property<string>("ForgerKey")
                    .HasColumnType("TEXT");

                b.HasKey("Key");

                b.HasIndex("ForgerKey");

                b.ToTable("RegistrationToken");
            });

            modelBuilder.Entity("HacknetSharp.Server.Models.SystemModel", b =>
            {
                b.Property<Guid>("Key")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("TEXT");

                b.Property<uint>("Address")
                    .HasColumnType("INTEGER");

                b.Property<double>("BootTime")
                    .HasColumnType("REAL");

                b.Property<double>("ClockSpeed")
                    .HasColumnType("REAL");

                b.Property<string>("ConnectCommandLine")
                    .HasColumnType("TEXT");

                b.Property<int>("DiskCapacity")
                    .HasColumnType("INTEGER");

                b.Property<double>("FirewallDelay")
                    .HasColumnType("REAL");

                b.Property<int>("FirewallIterations")
                    .HasColumnType("INTEGER");

                b.Property<int>("FirewallLength")
                    .HasColumnType("INTEGER");

                b.Property<string>("FixedFirewall")
                    .HasColumnType("TEXT");

                b.Property<string>("Name")
                    .IsRequired()
                    .HasColumnType("TEXT");

                b.Property<string>("OsName")
                    .IsRequired()
                    .HasColumnType("TEXT");

                b.Property<Guid>("OwnerKey")
                    .HasColumnType("TEXT");

                b.Property<double>("ProxyClocks")
                    .HasColumnType("REAL");

                b.Property<double>("RebootDuration")
                    .HasColumnType("REAL");

                b.Property<int>("RequiredExploits")
                    .HasColumnType("INTEGER");

                b.Property<Guid>("SpawnGroup")
                    .HasColumnType("TEXT");

                b.Property<long>("SystemMemory")
                    .HasColumnType("INTEGER");

                b.Property<string>("Tag")
                    .HasColumnType("TEXT");

                b.Property<string>("Template")
                    .IsRequired()
                    .HasColumnType("TEXT");

                b.Property<Guid>("WorldKey")
                    .HasColumnType("TEXT");

                b.HasKey("Key");

                b.HasIndex("OwnerKey");

                b.HasIndex("WorldKey");

                b.ToTable("SystemModel");
            });

            modelBuilder.Entity("HacknetSharp.Server.Models.UserModel", b =>
            {
                b.Property<string>("Key")
                    .HasColumnType("TEXT");

                b.Property<Guid>("ActiveWorld")
                    .HasColumnType("TEXT");

                b.Property<bool>("Admin")
                    .HasColumnType("INTEGER");

                b.Property<byte[]>("Hash")
                    .IsRequired()
                    .HasColumnType("BLOB");

                b.Property<string>("PasswordResetToken")
                    .HasColumnType("TEXT");

                b.Property<long>("PasswordResetTokenExpiry")
                    .HasColumnType("INTEGER");

                b.Property<byte[]>("Salt")
                    .IsRequired()
                    .HasColumnType("BLOB");

                b.HasKey("Key");

                b.ToTable("UserModel");
            });

            modelBuilder.Entity("HacknetSharp.Server.Models.VulnerabilityModel", b =>
            {
                b.Property<Guid>("Key")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("TEXT");

                b.Property<string>("Cve")
                    .HasColumnType("TEXT");

                b.Property<string>("EntryPoint")
                    .IsRequired()
                    .HasColumnType("TEXT");

                b.Property<int>("Exploits")
                    .HasColumnType("INTEGER");

                b.Property<string>("Protocol")
                    .IsRequired()
                    .HasColumnType("TEXT");

                b.Property<Guid>("SystemKey")
                    .HasColumnType("TEXT");

                b.Property<Guid?>("WorldKey")
                    .HasColumnType("TEXT");

                b.HasKey("Key");

                b.HasIndex("SystemKey");

                b.HasIndex("WorldKey");

                b.ToTable("VulnerabilityModel");
            });

            modelBuilder.Entity("HacknetSharp.Server.Models.WorldModel", b =>
            {
                b.Property<Guid>("Key")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("TEXT");

                b.Property<int>("DiskCapacity")
                    .HasColumnType("INTEGER");

                b.Property<string>("Label")
                    .IsRequired()
                    .HasColumnType("TEXT");

                b.Property<string>("Name")
                    .IsRequired()
                    .HasColumnType("TEXT");

                b.Property<double>("Now")
                    .HasColumnType("REAL");

                b.Property<string>("PlayerAddressRange")
                    .IsRequired()
                    .HasColumnType("TEXT");

                b.Property<string>("PlayerSystemTemplate")
                    .IsRequired()
                    .HasColumnType("TEXT");

                b.Property<double>("RebootDuration")
                    .HasColumnType("REAL");

                b.Property<string>("StartingMission")
                    .HasColumnType("TEXT");

                b.Property<string>("StartupCommandLine")
                    .HasColumnType("TEXT");

                b.Property<long>("SystemMemory")
                    .HasColumnType("INTEGER");

                b.HasKey("Key");

                b.ToTable("WorldModel");
            });

            modelBuilder.Entity("HacknetSharp.Server.Models.CronModel", b =>
            {
                b.HasOne("HacknetSharp.Server.Models.SystemModel", "System")
                    .WithMany("Tasks")
                    .HasForeignKey("SystemKey")
                    .OnDelete(DeleteBehavior.Cascade)
                    .IsRequired();

                b.HasOne("HacknetSharp.Server.Models.WorldModel", "World")
                    .WithMany()
                    .HasForeignKey("WorldKey");

                b.Navigation("System");

                b.Navigation("World");
            });

            modelBuilder.Entity("HacknetSharp.Server.Models.FileModel", b =>
            {
                b.HasOne("HacknetSharp.Server.Models.LoginModel", "Owner")
                    .WithMany()
                    .HasForeignKey("OwnerKey");

                b.HasOne("HacknetSharp.Server.Models.SystemModel", "System")
                    .WithMany("Files")
                    .HasForeignKey("SystemKey")
                    .OnDelete(DeleteBehavior.Cascade)
                    .IsRequired();

                b.HasOne("HacknetSharp.Server.Models.WorldModel", "World")
                    .WithMany()
                    .HasForeignKey("WorldKey");

                b.Navigation("Owner");

                b.Navigation("System");

                b.Navigation("World");
            });

            modelBuilder.Entity("HacknetSharp.Server.Models.KnownSystemModel", b =>
            {
                b.HasOne("HacknetSharp.Server.Models.SystemModel", "From")
                    .WithMany("KnownSystems")
                    .HasForeignKey("FromKey")
                    .OnDelete(DeleteBehavior.Cascade)
                    .IsRequired();

                b.HasOne("HacknetSharp.Server.Models.SystemModel", "To")
                    .WithMany("KnowingSystems")
                    .HasForeignKey("ToKey")
                    .OnDelete(DeleteBehavior.Cascade)
                    .IsRequired();

                b.HasOne("HacknetSharp.Server.Models.WorldModel", "World")
                    .WithMany()
                    .HasForeignKey("WorldKey");

                b.Navigation("From");

                b.Navigation("To");

                b.Navigation("World");
            });

            modelBuilder.Entity("HacknetSharp.Server.Models.LoginModel", b =>
            {
                b.HasOne("HacknetSharp.Server.Models.SystemModel", "System")
                    .WithMany("Logins")
                    .HasForeignKey("SystemKey")
                    .OnDelete(DeleteBehavior.Cascade)
                    .IsRequired();

                b.HasOne("HacknetSharp.Server.Models.WorldModel", "World")
                    .WithMany()
                    .HasForeignKey("WorldKey");

                b.Navigation("System");

                b.Navigation("World");
            });

            modelBuilder.Entity("HacknetSharp.Server.Models.MissionModel", b =>
            {
                b.HasOne("HacknetSharp.Server.Models.PersonModel", "Person")
                    .WithMany("Missions")
                    .HasForeignKey("PersonKey")
                    .OnDelete(DeleteBehavior.Cascade)
                    .IsRequired();

                b.HasOne("HacknetSharp.Server.Models.WorldModel", "World")
                    .WithMany()
                    .HasForeignKey("WorldKey");

                b.Navigation("Person");

                b.Navigation("World");
            });

            modelBuilder.Entity("HacknetSharp.Server.Models.PersonModel", b =>
            {
                b.HasOne("HacknetSharp.Server.Models.UserModel", "User")
                    .WithMany("Identities")
                    .HasForeignKey("UserKey")
                    .OnDelete(DeleteBehavior.Cascade);

                b.HasOne("HacknetSharp.Server.Models.WorldModel", "World")
                    .WithMany("Persons")
                    .HasForeignKey("WorldKey")
                    .OnDelete(DeleteBehavior.Cascade)
                    .IsRequired();

                b.Navigation("User");

                b.Navigation("World");
            });

            modelBuilder.Entity("HacknetSharp.Server.Models.RegistrationToken", b =>
            {
                b.HasOne("HacknetSharp.Server.Models.UserModel", "Forger")
                    .WithMany()
                    .HasForeignKey("ForgerKey");

                b.Navigation("Forger");
            });

            modelBuilder.Entity("HacknetSharp.Server.Models.SystemModel", b =>
            {
                b.HasOne("HacknetSharp.Server.Models.PersonModel", "Owner")
                    .WithMany("Systems")
                    .HasForeignKey("OwnerKey")
                    .OnDelete(DeleteBehavior.Cascade)
                    .IsRequired();

                b.HasOne("HacknetSharp.Server.Models.WorldModel", "World")
                    .WithMany("Systems")
                    .HasForeignKey("WorldKey")
                    .OnDelete(DeleteBehavior.Cascade)
                    .IsRequired();

                b.Navigation("Owner");

                b.Navigation("World");
            });

            modelBuilder.Entity("HacknetSharp.Server.Models.VulnerabilityModel", b =>
            {
                b.HasOne("HacknetSharp.Server.Models.SystemModel", "System")
                    .WithMany("Vulnerabilities")
                    .HasForeignKey("SystemKey")
                    .OnDelete(DeleteBehavior.Cascade)
                    .IsRequired();

                b.HasOne("HacknetSharp.Server.Models.WorldModel", "World")
                    .WithMany()
                    .HasForeignKey("WorldKey");

                b.Navigation("System");

                b.Navigation("World");
            });

            modelBuilder.Entity("HacknetSharp.Server.Models.PersonModel", b =>
            {
                b.Navigation("Missions");

                b.Navigation("Systems");
            });

            modelBuilder.Entity("HacknetSharp.Server.Models.SystemModel", b =>
            {
                b.Navigation("Files");

                b.Navigation("KnowingSystems");

                b.Navigation("KnownSystems");

                b.Navigation("Logins");

                b.Navigation("Tasks");

                b.Navigation("Vulnerabilities");
            });

            modelBuilder.Entity("HacknetSharp.Server.Models.UserModel", b => { b.Navigation("Identities"); });

            modelBuilder.Entity("HacknetSharp.Server.Models.WorldModel", b =>
            {
                b.Navigation("Persons");

                b.Navigation("Systems");
            });
#pragma warning restore 612, 618
        }
    }
}
