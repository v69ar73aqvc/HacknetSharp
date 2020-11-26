﻿// <auto-generated />
using System;
using HacknetSharp.Server;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace hssqlite.Migrations
{
    [DbContext(typeof(ServerStorageContext))]
    [Migration("20201125094606_Initial")]
    partial class Initial
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "5.0.0");

            modelBuilder.Entity("HacknetSharp.Server.Common.Models.FileModel", b =>
                {
                    b.Property<Guid>("Key")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<byte>("Kind")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<Guid>("OwnerKey")
                        .HasColumnType("TEXT");

                    b.Property<string>("Path")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<Guid>("World")
                        .HasColumnType("TEXT");

                    b.HasKey("Key");

                    b.HasIndex("OwnerKey");

                    b.ToTable("FileModel");
                });

            modelBuilder.Entity("HacknetSharp.Server.Common.Models.PersonModel", b =>
                {
                    b.Property<Guid>("Key")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("PlayerKey")
                        .HasColumnType("TEXT");

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<Guid>("World")
                        .HasColumnType("TEXT");

                    b.HasKey("Key");

                    b.HasIndex("PlayerKey");

                    b.ToTable("PersonModel");
                });

            modelBuilder.Entity("HacknetSharp.Server.Common.Models.PlayerModel", b =>
                {
                    b.Property<string>("Key")
                        .HasColumnType("TEXT");

                    b.Property<Guid>("ActiveWorld")
                        .HasColumnType("TEXT");

                    b.Property<Guid?>("CurrentSystemKey")
                        .HasColumnType("TEXT");

                    b.Property<Guid>("DefaultSystem")
                        .HasColumnType("TEXT");

                    b.HasKey("Key");

                    b.HasIndex("CurrentSystemKey");

                    b.ToTable("PlayerModel");
                });

            modelBuilder.Entity("HacknetSharp.Server.Common.Models.SystemModel", b =>
                {
                    b.Property<Guid>("Key")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<Guid>("OwnerKey")
                        .HasColumnType("TEXT");

                    b.Property<Guid>("World")
                        .HasColumnType("TEXT");

                    b.HasKey("Key");

                    b.HasIndex("OwnerKey");

                    b.ToTable("SystemModel");
                });

            modelBuilder.Entity("HacknetSharp.Server.RegistrationToken", b =>
                {
                    b.Property<string>("Key")
                        .HasColumnType("TEXT");

                    b.Property<string>("ForgerKey")
                        .HasColumnType("TEXT");

                    b.HasKey("Key");

                    b.HasIndex("ForgerKey");

                    b.ToTable("RegistrationToken");
                });

            modelBuilder.Entity("HacknetSharp.Server.UserModel", b =>
                {
                    b.Property<string>("Key")
                        .HasColumnType("TEXT");

                    b.Property<bool>("Admin")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Base64Password")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Base64Salt")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Key");

                    b.ToTable("UserModel");
                });

            modelBuilder.Entity("HacknetSharp.Server.Common.Models.FileModel", b =>
                {
                    b.HasOne("HacknetSharp.Server.Common.Models.SystemModel", "Owner")
                        .WithMany("Files")
                        .HasForeignKey("OwnerKey")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Owner");
                });

            modelBuilder.Entity("HacknetSharp.Server.Common.Models.PersonModel", b =>
                {
                    b.HasOne("HacknetSharp.Server.Common.Models.PlayerModel", "Player")
                        .WithMany("Identities")
                        .HasForeignKey("PlayerKey");

                    b.Navigation("Player");
                });

            modelBuilder.Entity("HacknetSharp.Server.Common.Models.PlayerModel", b =>
                {
                    b.HasOne("HacknetSharp.Server.Common.Models.SystemModel", "CurrentSystem")
                        .WithMany()
                        .HasForeignKey("CurrentSystemKey");

                    b.Navigation("CurrentSystem");
                });

            modelBuilder.Entity("HacknetSharp.Server.Common.Models.SystemModel", b =>
                {
                    b.HasOne("HacknetSharp.Server.Common.Models.PersonModel", "Owner")
                        .WithMany("Systems")
                        .HasForeignKey("OwnerKey")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Owner");
                });

            modelBuilder.Entity("HacknetSharp.Server.RegistrationToken", b =>
                {
                    b.HasOne("HacknetSharp.Server.UserModel", "Forger")
                        .WithMany()
                        .HasForeignKey("ForgerKey");

                    b.Navigation("Forger");
                });

            modelBuilder.Entity("HacknetSharp.Server.Common.Models.PersonModel", b =>
                {
                    b.Navigation("Systems");
                });

            modelBuilder.Entity("HacknetSharp.Server.Common.Models.PlayerModel", b =>
                {
                    b.Navigation("Identities");
                });

            modelBuilder.Entity("HacknetSharp.Server.Common.Models.SystemModel", b =>
                {
                    b.Navigation("Files");
                });
#pragma warning restore 612, 618
        }
    }
}