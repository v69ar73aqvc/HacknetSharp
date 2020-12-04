﻿// <auto-generated />
using System;
using hss.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace hss.Sqlite.Migrations
{
    [DbContext(typeof(ServerStorageContext))]
    [Migration("20201203011039_Initial")]
    partial class Initial
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "5.0.0");

            modelBuilder.Entity("hss.Core.Common.Models.FileModel", b =>
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

            modelBuilder.Entity("hss.Core.Common.Models.LoginModel", b =>
                {
                    b.Property<Guid>("Key")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

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

            modelBuilder.Entity("hss.Core.Common.Models.PersonModel", b =>
                {
                    b.Property<Guid>("Key")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<Guid>("DefaultSystem")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("PlayerKey")
                        .HasColumnType("TEXT");

                    b.Property<bool>("StartedUp")
                        .HasColumnType("INTEGER");

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("WorkingDirectory")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<Guid>("WorldKey")
                        .HasColumnType("TEXT");

                    b.HasKey("Key");

                    b.HasIndex("PlayerKey");

                    b.HasIndex("WorldKey");

                    b.ToTable("PersonModel");
                });

            modelBuilder.Entity("hss.Core.Common.Models.PlayerModel", b =>
                {
                    b.Property<string>("Key")
                        .HasColumnType("TEXT");

                    b.Property<Guid>("ActiveWorld")
                        .HasColumnType("TEXT");

                    b.Property<string>("UserForeignKey")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Key");

                    b.HasIndex("UserForeignKey")
                        .IsUnique();

                    b.ToTable("PlayerModel");
                });

            modelBuilder.Entity("hss.Core.Common.Models.RegistrationToken", b =>
                {
                    b.Property<string>("Key")
                        .HasColumnType("TEXT");

                    b.Property<string>("ForgerKey")
                        .HasColumnType("TEXT");

                    b.HasKey("Key");

                    b.HasIndex("ForgerKey");

                    b.ToTable("RegistrationToken");
                });

            modelBuilder.Entity("hss.Core.Common.Models.SystemModel", b =>
                {
                    b.Property<Guid>("Key")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<uint>("Address")
                        .HasColumnType("INTEGER");

                    b.Property<string>("ConnectCommandLine")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("OsName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<Guid>("OwnerKey")
                        .HasColumnType("TEXT");

                    b.Property<Guid>("WorldKey")
                        .HasColumnType("TEXT");

                    b.HasKey("Key");

                    b.HasIndex("OwnerKey");

                    b.HasIndex("WorldKey");

                    b.ToTable("SystemModel");
                });

            modelBuilder.Entity("hss.Core.Common.Models.UserModel", b =>
                {
                    b.Property<string>("Key")
                        .HasColumnType("TEXT");

                    b.Property<bool>("Admin")
                        .HasColumnType("INTEGER");

                    b.Property<byte[]>("Hash")
                        .IsRequired()
                        .HasColumnType("BLOB");

                    b.Property<byte[]>("Salt")
                        .IsRequired()
                        .HasColumnType("BLOB");

                    b.HasKey("Key");

                    b.ToTable("UserModel");
                });

            modelBuilder.Entity("hss.Core.Common.Models.WorldModel", b =>
                {
                    b.Property<Guid>("Key")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("Label")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("PlayerAddressRange")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("PlayerSystemTemplate")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("StartupCommandLine")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Key");

                    b.ToTable("WorldModel");
                });

            modelBuilder.Entity("hss.Core.Common.Models.FileModel", b =>
                {
                    b.HasOne("hss.Core.Common.Models.LoginModel", "Owner")
                        .WithMany()
                        .HasForeignKey("OwnerKey");

                    b.HasOne("hss.Core.Common.Models.SystemModel", "System")
                        .WithMany("Files")
                        .HasForeignKey("SystemKey")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("hss.Core.Common.Models.WorldModel", "World")
                        .WithMany()
                        .HasForeignKey("WorldKey");

                    b.Navigation("Owner");

                    b.Navigation("System");

                    b.Navigation("World");
                });

            modelBuilder.Entity("hss.Core.Common.Models.LoginModel", b =>
                {
                    b.HasOne("hss.Core.Common.Models.SystemModel", "System")
                        .WithMany("Logins")
                        .HasForeignKey("SystemKey")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("hss.Core.Common.Models.WorldModel", "World")
                        .WithMany()
                        .HasForeignKey("WorldKey");

                    b.Navigation("System");

                    b.Navigation("World");
                });

            modelBuilder.Entity("hss.Core.Common.Models.PersonModel", b =>
                {
                    b.HasOne("hss.Core.Common.Models.PlayerModel", "Player")
                        .WithMany("Identities")
                        .HasForeignKey("PlayerKey")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("hss.Core.Common.Models.WorldModel", "World")
                        .WithMany("Persons")
                        .HasForeignKey("WorldKey")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Player");

                    b.Navigation("World");
                });

            modelBuilder.Entity("hss.Core.Common.Models.PlayerModel", b =>
                {
                    b.HasOne("hss.Core.Common.Models.UserModel", "User")
                        .WithOne("Player")
                        .HasForeignKey("hss.Core.Common.Models.PlayerModel", "UserForeignKey")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("hss.Core.Common.Models.RegistrationToken", b =>
                {
                    b.HasOne("hss.Core.Common.Models.UserModel", "Forger")
                        .WithMany()
                        .HasForeignKey("ForgerKey");

                    b.Navigation("Forger");
                });

            modelBuilder.Entity("hss.Core.Common.Models.SystemModel", b =>
                {
                    b.HasOne("hss.Core.Common.Models.PersonModel", "Owner")
                        .WithMany("Systems")
                        .HasForeignKey("OwnerKey")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("hss.Core.Common.Models.WorldModel", "World")
                        .WithMany("Systems")
                        .HasForeignKey("WorldKey")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Owner");

                    b.Navigation("World");
                });

            modelBuilder.Entity("hss.Core.Common.Models.PersonModel", b =>
                {
                    b.Navigation("Systems");
                });

            modelBuilder.Entity("hss.Core.Common.Models.PlayerModel", b =>
                {
                    b.Navigation("Identities");
                });

            modelBuilder.Entity("hss.Core.Common.Models.SystemModel", b =>
                {
                    b.Navigation("Files");

                    b.Navigation("Logins");
                });

            modelBuilder.Entity("hss.Core.Common.Models.UserModel", b =>
                {
                    b.Navigation("Player")
                        .IsRequired();
                });

            modelBuilder.Entity("hss.Core.Common.Models.WorldModel", b =>
                {
                    b.Navigation("Persons");

                    b.Navigation("Systems");
                });
#pragma warning restore 612, 618
        }
    }
}