﻿// <auto-generated />
using System;
using HacknetSharp.Server;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace hss.Postgres.Migrations
{
    [DbContext(typeof(ServerStorageContext))]
    [Migration("20201130054646_Initial")]
    partial class Initial
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .UseIdentityByDefaultColumns()
                .HasAnnotation("Relational:MaxIdentifierLength", 63)
                .HasAnnotation("ProductVersion", "5.0.0");

            modelBuilder.Entity("HacknetSharp.Server.Common.Models.FileModel", b =>
                {
                    b.Property<Guid>("Key")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<byte>("Execute")
                        .HasColumnType("smallint");

                    b.Property<bool>("Hidden")
                        .HasColumnType("boolean");

                    b.Property<byte>("Kind")
                        .HasColumnType("smallint");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<Guid?>("OwnerKey")
                        .HasColumnType("uuid");

                    b.Property<string>("Path")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<byte>("Read")
                        .HasColumnType("smallint");

                    b.Property<Guid>("SystemKey")
                        .HasColumnType("uuid");

                    b.Property<Guid?>("WorldKey")
                        .HasColumnType("uuid");

                    b.Property<byte>("Write")
                        .HasColumnType("smallint");

                    b.HasKey("Key");

                    b.HasIndex("OwnerKey");

                    b.HasIndex("SystemKey");

                    b.HasIndex("WorldKey");

                    b.ToTable("FileModel");
                });

            modelBuilder.Entity("HacknetSharp.Server.Common.Models.LoginModel", b =>
                {
                    b.Property<Guid>("Key")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<byte[]>("Hash")
                        .IsRequired()
                        .HasColumnType("bytea");

                    b.Property<Guid>("PersonForeignKey")
                        .HasColumnType("uuid");

                    b.Property<byte[]>("Salt")
                        .IsRequired()
                        .HasColumnType("bytea");

                    b.Property<Guid>("SystemKey")
                        .HasColumnType("uuid");

                    b.Property<string>("User")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<Guid?>("WorldKey")
                        .HasColumnType("uuid");

                    b.HasKey("Key");

                    b.HasIndex("PersonForeignKey")
                        .IsUnique();

                    b.HasIndex("SystemKey");

                    b.HasIndex("WorldKey");

                    b.ToTable("LoginModel");
                });

            modelBuilder.Entity("HacknetSharp.Server.Common.Models.PersonModel", b =>
                {
                    b.Property<Guid>("Key")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid?>("CurrentSystemKey")
                        .HasColumnType("uuid");

                    b.Property<Guid?>("DefaultSystemKey")
                        .HasColumnType("uuid");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("PlayerKey")
                        .HasColumnType("text");

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("WorkingDirectory")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<Guid>("WorldKey")
                        .HasColumnType("uuid");

                    b.HasKey("Key");

                    b.HasIndex("CurrentSystemKey");

                    b.HasIndex("DefaultSystemKey");

                    b.HasIndex("PlayerKey");

                    b.HasIndex("WorldKey");

                    b.ToTable("PersonModel");
                });

            modelBuilder.Entity("HacknetSharp.Server.Common.Models.PlayerModel", b =>
                {
                    b.Property<string>("Key")
                        .HasColumnType("text");

                    b.Property<Guid>("ActiveWorld")
                        .HasColumnType("uuid");

                    b.Property<string>("UserForeignKey")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Key");

                    b.HasIndex("UserForeignKey")
                        .IsUnique();

                    b.ToTable("PlayerModel");
                });

            modelBuilder.Entity("HacknetSharp.Server.Common.Models.RegistrationToken", b =>
                {
                    b.Property<string>("Key")
                        .HasColumnType("text");

                    b.Property<string>("ForgerKey")
                        .HasColumnType("text");

                    b.HasKey("Key");

                    b.HasIndex("ForgerKey");

                    b.ToTable("RegistrationToken");
                });

            modelBuilder.Entity("HacknetSharp.Server.Common.Models.SystemModel", b =>
                {
                    b.Property<Guid>("Key")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<long>("Address")
                        .HasColumnType("bigint");

                    b.Property<string>("InitialProgram")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("OsName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<Guid>("OwnerKey")
                        .HasColumnType("uuid");

                    b.Property<Guid>("WorldKey")
                        .HasColumnType("uuid");

                    b.HasKey("Key");

                    b.HasIndex("OwnerKey");

                    b.HasIndex("WorldKey");

                    b.ToTable("SystemModel");
                });

            modelBuilder.Entity("HacknetSharp.Server.Common.Models.UserModel", b =>
                {
                    b.Property<string>("Key")
                        .HasColumnType("text");

                    b.Property<bool>("Admin")
                        .HasColumnType("boolean");

                    b.Property<byte[]>("Hash")
                        .IsRequired()
                        .HasColumnType("bytea");

                    b.Property<byte[]>("Salt")
                        .IsRequired()
                        .HasColumnType("bytea");

                    b.HasKey("Key");

                    b.ToTable("UserModel");
                });

            modelBuilder.Entity("HacknetSharp.Server.Common.Models.WorldModel", b =>
                {
                    b.Property<Guid>("Key")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Label")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("PlayerAddressRange")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("PlayerSystemTemplate")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("StartupCommandLine")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("StartupProgram")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Key");

                    b.ToTable("WorldModel");
                });

            modelBuilder.Entity("HacknetSharp.Server.Common.Models.FileModel", b =>
                {
                    b.HasOne("HacknetSharp.Server.Common.Models.LoginModel", "Owner")
                        .WithMany()
                        .HasForeignKey("OwnerKey");

                    b.HasOne("HacknetSharp.Server.Common.Models.SystemModel", "System")
                        .WithMany("Files")
                        .HasForeignKey("SystemKey")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("HacknetSharp.Server.Common.Models.WorldModel", "World")
                        .WithMany()
                        .HasForeignKey("WorldKey");

                    b.Navigation("Owner");

                    b.Navigation("System");

                    b.Navigation("World");
                });

            modelBuilder.Entity("HacknetSharp.Server.Common.Models.LoginModel", b =>
                {
                    b.HasOne("HacknetSharp.Server.Common.Models.PersonModel", "Person")
                        .WithOne("CurrentLogin")
                        .HasForeignKey("HacknetSharp.Server.Common.Models.LoginModel", "PersonForeignKey")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("HacknetSharp.Server.Common.Models.SystemModel", "System")
                        .WithMany("Logins")
                        .HasForeignKey("SystemKey")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("HacknetSharp.Server.Common.Models.WorldModel", "World")
                        .WithMany()
                        .HasForeignKey("WorldKey");

                    b.Navigation("Person");

                    b.Navigation("System");

                    b.Navigation("World");
                });

            modelBuilder.Entity("HacknetSharp.Server.Common.Models.PersonModel", b =>
                {
                    b.HasOne("HacknetSharp.Server.Common.Models.SystemModel", "CurrentSystem")
                        .WithMany()
                        .HasForeignKey("CurrentSystemKey");

                    b.HasOne("HacknetSharp.Server.Common.Models.SystemModel", "DefaultSystem")
                        .WithMany()
                        .HasForeignKey("DefaultSystemKey");

                    b.HasOne("HacknetSharp.Server.Common.Models.PlayerModel", "Player")
                        .WithMany("Identities")
                        .HasForeignKey("PlayerKey")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("HacknetSharp.Server.Common.Models.WorldModel", "World")
                        .WithMany("Persons")
                        .HasForeignKey("WorldKey")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("CurrentSystem");

                    b.Navigation("DefaultSystem");

                    b.Navigation("Player");

                    b.Navigation("World");
                });

            modelBuilder.Entity("HacknetSharp.Server.Common.Models.PlayerModel", b =>
                {
                    b.HasOne("HacknetSharp.Server.Common.Models.UserModel", "User")
                        .WithOne("Player")
                        .HasForeignKey("HacknetSharp.Server.Common.Models.PlayerModel", "UserForeignKey")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("HacknetSharp.Server.Common.Models.RegistrationToken", b =>
                {
                    b.HasOne("HacknetSharp.Server.Common.Models.UserModel", "Forger")
                        .WithMany()
                        .HasForeignKey("ForgerKey");

                    b.Navigation("Forger");
                });

            modelBuilder.Entity("HacknetSharp.Server.Common.Models.SystemModel", b =>
                {
                    b.HasOne("HacknetSharp.Server.Common.Models.PersonModel", "Owner")
                        .WithMany("Systems")
                        .HasForeignKey("OwnerKey")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("HacknetSharp.Server.Common.Models.WorldModel", "World")
                        .WithMany("Systems")
                        .HasForeignKey("WorldKey")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Owner");

                    b.Navigation("World");
                });

            modelBuilder.Entity("HacknetSharp.Server.Common.Models.PersonModel", b =>
                {
                    b.Navigation("CurrentLogin");

                    b.Navigation("Systems");
                });

            modelBuilder.Entity("HacknetSharp.Server.Common.Models.PlayerModel", b =>
                {
                    b.Navigation("Identities");
                });

            modelBuilder.Entity("HacknetSharp.Server.Common.Models.SystemModel", b =>
                {
                    b.Navigation("Files");

                    b.Navigation("Logins");
                });

            modelBuilder.Entity("HacknetSharp.Server.Common.Models.UserModel", b =>
                {
                    b.Navigation("Player")
                        .IsRequired();
                });

            modelBuilder.Entity("HacknetSharp.Server.Common.Models.WorldModel", b =>
                {
                    b.Navigation("Persons");

                    b.Navigation("Systems");
                });
#pragma warning restore 612, 618
        }
    }
}
