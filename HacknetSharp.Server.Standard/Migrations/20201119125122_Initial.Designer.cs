﻿// <auto-generated />
using HacknetSharp.Server;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace HacknetSharp.Server.Standard.Migrations
{
    [DbContext(typeof(WorldStorageContext))]
    [Migration("20201119125122_Initial")]
    partial class Initial
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "5.0.0");
#pragma warning restore 612, 618
        }
    }
}