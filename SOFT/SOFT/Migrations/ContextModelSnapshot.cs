﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using startApp.Classes;

#nullable disable

namespace SOFT.Migrations
{
    [DbContext(typeof(Context))]
    partial class ContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("startApp.Classes.FavoriteMobilni", b =>
                {
                    b.Property<int>("FavoriteMobilniID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("FavoriteMobilniID"));

                    b.Property<int>("oglasID")
                        .HasColumnType("int");

                    b.Property<int>("userID")
                        .HasColumnType("int");

                    b.HasKey("FavoriteMobilniID");

                    b.HasIndex("oglasID");

                    b.HasIndex("userID");

                    b.ToTable("favorites");
                });

            modelBuilder.Entity("startApp.Classes.Komentar", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("Datum")
                        .HasColumnType("datetime2");

                    b.Property<int?>("KomentarisanOglasid")
                        .HasColumnType("int");

                    b.Property<int?>("Komentatorid")
                        .HasColumnType("int");

                    b.Property<string>("tekst")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("KomentarisanOglasid");

                    b.HasIndex("Komentatorid");

                    b.ToTable("komentari");
                });

            modelBuilder.Entity("startApp.Classes.Mobilni", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("id"));

                    b.Property<string>("brand")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("model")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("tip")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("id");

                    b.ToTable("mobilni");
                });

            modelBuilder.Entity("startApp.Classes.Ocena", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("ocena")
                        .HasColumnType("int");

                    b.Property<int?>("ocenioid")
                        .HasColumnType("int");

                    b.Property<int?>("ocenjenid")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ocenioid");

                    b.HasIndex("ocenjenid");

                    b.ToTable("ocene");
                });

            modelBuilder.Entity("startApp.Classes.Oglas", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("id"));

                    b.Property<int>("batteryHealth")
                        .HasColumnType("int");

                    b.Property<int>("color")
                        .HasColumnType("int");

                    b.Property<int>("memory")
                        .HasColumnType("int");

                    b.Property<int?>("mojMobilniid")
                        .HasColumnType("int");

                    b.Property<string>("picture1")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("picture2")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("picture3")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("price")
                        .HasColumnType("int");

                    b.Property<bool>("prihvacen")
                        .HasColumnType("bit");

                    b.Property<int?>("userid")
                        .HasColumnType("int");

                    b.Property<int?>("year")
                        .HasColumnType("int");

                    b.Property<int?>("zeljeniMobilniid")
                        .HasColumnType("int");

                    b.HasKey("id");

                    b.HasIndex("mojMobilniid");

                    b.HasIndex("userid");

                    b.HasIndex("zeljeniMobilniid");

                    b.ToTable("oglasi");
                });

            modelBuilder.Entity("startApp.Classes.Request", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("id"));

                    b.Property<DateTime>("date")
                        .HasColumnType("datetime2");

                    b.Property<bool>("odgovoren")
                        .HasColumnType("bit");

                    b.Property<int?>("oglasid")
                        .HasColumnType("int");

                    b.Property<bool>("prihvacen")
                        .HasColumnType("bit");

                    b.Property<int?>("recieverid")
                        .HasColumnType("int");

                    b.Property<int?>("senderid")
                        .HasColumnType("int");

                    b.Property<DateTime>("time")
                        .HasColumnType("datetime2");

                    b.HasKey("id");

                    b.HasIndex("oglasid");

                    b.HasIndex("recieverid");

                    b.HasIndex("senderid");

                    b.ToTable("requests");
                });

            modelBuilder.Entity("startApp.Classes.Response", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("id"));

                    b.Property<bool>("accept")
                        .HasColumnType("bit");

                    b.Property<int?>("requestid")
                        .HasColumnType("int");

                    b.HasKey("id");

                    b.HasIndex("requestid");

                    b.ToTable("responses");
                });

            modelBuilder.Entity("startApp.Classes.User", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("id"));

                    b.Property<string>("address")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("admin")
                        .HasColumnType("bit");

                    b.Property<bool>("approved")
                        .HasColumnType("bit");

                    b.Property<string>("brojTelefona")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("odgovorNaPitanje")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("password")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("picture")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("sigurnosnoPitanje")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("surname")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("username")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("id");

                    b.ToTable("users");
                });

            modelBuilder.Entity("startApp.Classes.FavoriteMobilni", b =>
                {
                    b.HasOne("startApp.Classes.Oglas", "oglas")
                        .WithMany("favorites")
                        .HasForeignKey("oglasID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("startApp.Classes.User", "user")
                        .WithMany("favorites")
                        .HasForeignKey("userID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("oglas");

                    b.Navigation("user");
                });

            modelBuilder.Entity("startApp.Classes.Komentar", b =>
                {
                    b.HasOne("startApp.Classes.Oglas", "KomentarisanOglas")
                        .WithMany("Komentari")
                        .HasForeignKey("KomentarisanOglasid");

                    b.HasOne("startApp.Classes.User", "Komentator")
                        .WithMany()
                        .HasForeignKey("Komentatorid");

                    b.Navigation("KomentarisanOglas");

                    b.Navigation("Komentator");
                });

            modelBuilder.Entity("startApp.Classes.Ocena", b =>
                {
                    b.HasOne("startApp.Classes.User", "ocenio")
                        .WithMany()
                        .HasForeignKey("ocenioid");

                    b.HasOne("startApp.Classes.User", "ocenjen")
                        .WithMany()
                        .HasForeignKey("ocenjenid");

                    b.Navigation("ocenio");

                    b.Navigation("ocenjen");
                });

            modelBuilder.Entity("startApp.Classes.Oglas", b =>
                {
                    b.HasOne("startApp.Classes.Mobilni", "mojMobilni")
                        .WithMany()
                        .HasForeignKey("mojMobilniid");

                    b.HasOne("startApp.Classes.User", "user")
                        .WithMany("oglasi")
                        .HasForeignKey("userid");

                    b.HasOne("startApp.Classes.Mobilni", "zeljeniMobilni")
                        .WithMany()
                        .HasForeignKey("zeljeniMobilniid");

                    b.Navigation("mojMobilni");

                    b.Navigation("user");

                    b.Navigation("zeljeniMobilni");
                });

            modelBuilder.Entity("startApp.Classes.Request", b =>
                {
                    b.HasOne("startApp.Classes.Oglas", "oglas")
                        .WithMany()
                        .HasForeignKey("oglasid");

                    b.HasOne("startApp.Classes.User", "reciever")
                        .WithMany()
                        .HasForeignKey("recieverid");

                    b.HasOne("startApp.Classes.User", "sender")
                        .WithMany()
                        .HasForeignKey("senderid");

                    b.Navigation("oglas");

                    b.Navigation("reciever");

                    b.Navigation("sender");
                });

            modelBuilder.Entity("startApp.Classes.Response", b =>
                {
                    b.HasOne("startApp.Classes.Request", "request")
                        .WithMany()
                        .HasForeignKey("requestid");

                    b.Navigation("request");
                });

            modelBuilder.Entity("startApp.Classes.Oglas", b =>
                {
                    b.Navigation("Komentari");

                    b.Navigation("favorites");
                });

            modelBuilder.Entity("startApp.Classes.User", b =>
                {
                    b.Navigation("favorites");

                    b.Navigation("oglasi");
                });
#pragma warning restore 612, 618
        }
    }
}
