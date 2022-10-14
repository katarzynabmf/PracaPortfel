﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Portfel.Data;

#nullable disable

namespace Portfel.Intranet.Migrations
{
    [DbContext(typeof(PortfelContext))]
    [Migration("20221014173832_M2")]
    partial class M2
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("Portfel.Data.Data.Konto", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<double>("Gotowka")
                        .HasColumnType("float");

                    b.Property<string>("Nazwa")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("UzytkownikId")
                        .HasColumnType("int");

                    b.Property<string>("Waluta")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("UzytkownikId");

                    b.ToTable("Konto");
                });

            modelBuilder.Entity("Portfel.Data.Data.RodzajOplaty", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Nazwa")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("Promowany")
                        .HasColumnType("bit");

                    b.HasKey("Id");

                    b.ToTable("RodzajOplaty");
                });

            modelBuilder.Entity("Portfel.Data.Data.RodzajTransakcji", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Nazwa")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("RodzajTransakcji");
                });

            modelBuilder.Entity("Portfel.Data.Data.SymbolGieldowy", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Nazwa")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("SymbolGieldowy");
                });

            modelBuilder.Entity("Portfel.Data.Data.Transakcja", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<int>("Ilosc")
                        .HasColumnType("int");

                    b.Property<string>("Komentarz")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("KontoId")
                        .IsRequired()
                        .HasColumnType("int");

                    b.Property<double>("Kwota")
                        .HasColumnType("float");

                    b.Property<int?>("RodzajOplatyId")
                        .HasColumnType("int");

                    b.Property<int?>("RodzajTransakcjiId")
                        .IsRequired()
                        .HasColumnType("int");

                    b.Property<int?>("SymbolGieldowyId")
                        .HasColumnType("int");

                    b.Property<string>("Waluta")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("KontoId");

                    b.HasIndex("RodzajOplatyId");

                    b.HasIndex("RodzajTransakcjiId");

                    b.HasIndex("SymbolGieldowyId");

                    b.ToTable("Transakcja");
                });

            modelBuilder.Entity("Portfel.Data.Data.Uzytkownik", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Haslo")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Imie")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Uzytkownik");
                });

            modelBuilder.Entity("Portfel.Data.Data.Konto", b =>
                {
                    b.HasOne("Portfel.Data.Data.Uzytkownik", "Uzytkownik")
                        .WithMany("Konto")
                        .HasForeignKey("UzytkownikId");

                    b.Navigation("Uzytkownik");
                });

            modelBuilder.Entity("Portfel.Data.Data.Transakcja", b =>
                {
                    b.HasOne("Portfel.Data.Data.Konto", "Konto")
                        .WithMany()
                        .HasForeignKey("KontoId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Portfel.Data.Data.RodzajOplaty", "RodzajOplaty")
                        .WithMany("Transakcja")
                        .HasForeignKey("RodzajOplatyId");

                    b.HasOne("Portfel.Data.Data.RodzajTransakcji", "RodzajTransakcji")
                        .WithMany("Transakcja")
                        .HasForeignKey("RodzajTransakcjiId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Portfel.Data.Data.SymbolGieldowy", "SymbolGieldowy")
                        .WithMany("Transakcja")
                        .HasForeignKey("SymbolGieldowyId");

                    b.Navigation("Konto");

                    b.Navigation("RodzajOplaty");

                    b.Navigation("RodzajTransakcji");

                    b.Navigation("SymbolGieldowy");
                });

            modelBuilder.Entity("Portfel.Data.Data.RodzajOplaty", b =>
                {
                    b.Navigation("Transakcja");
                });

            modelBuilder.Entity("Portfel.Data.Data.RodzajTransakcji", b =>
                {
                    b.Navigation("Transakcja");
                });

            modelBuilder.Entity("Portfel.Data.Data.SymbolGieldowy", b =>
                {
                    b.Navigation("Transakcja");
                });

            modelBuilder.Entity("Portfel.Data.Data.Uzytkownik", b =>
                {
                    b.Navigation("Konto");
                });
#pragma warning restore 612, 618
        }
    }
}
