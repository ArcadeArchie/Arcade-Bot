﻿// <auto-generated />
using System;
using ArcadeBot.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace ArcadeBot.Migrations
{
    [DbContext(typeof(ArcadeBotDbContext))]
    [Migration("20211022202434_DefaultRoleCol")]
    partial class DefaultRoleCol
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "5.0.9");

            modelBuilder.Entity("ArcadeBot.Infrastructure.Entities.Quote", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("AuthorId")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("AuthorName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<DateTimeOffset>("Posted_on")
                        .HasColumnType("TEXT");

                    b.Property<string>("ServerId")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("ServerName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Quotes");
                });

            modelBuilder.Entity("ArcadeBot.Infrastructure.Entities.Server", b =>
                {
                    b.Property<ulong>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<ulong>("AdminRole")
                        .HasColumnType("INTEGER");

                    b.Property<ulong>("DefaultRole")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<ulong>("TwitchNotificationChannel")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.ToTable("Servers");
                });

            modelBuilder.Entity("ArcadeBot.Infrastructure.Entities.ServerConfigOverride", b =>
                {
                    b.Property<string>("Key")
                        .HasColumnType("TEXT");

                    b.Property<bool>("Active")
                        .HasColumnType("INTEGER");

                    b.Property<string>("ConfigKey")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("OverrideValue")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<ulong>("ServerId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Key");

                    b.HasIndex("ServerId");

                    b.ToTable("ServerConfigOverride");
                });

            modelBuilder.Entity("ArcadeBot.Infrastructure.Entities.ServerUser", b =>
                {
                    b.Property<ulong>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTimeOffset>("CreatedAt")
                        .HasColumnType("TEXT");

                    b.Property<string>("Discriminator")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<bool>("IsBot")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("ServerServerUser", b =>
                {
                    b.Property<ulong>("GuildsId")
                        .HasColumnType("INTEGER");

                    b.Property<ulong>("UsersId")
                        .HasColumnType("INTEGER");

                    b.HasKey("GuildsId", "UsersId");

                    b.HasIndex("UsersId");

                    b.ToTable("ServerServerUser");
                });

            modelBuilder.Entity("ArcadeBot.Infrastructure.Entities.ServerConfigOverride", b =>
                {
                    b.HasOne("ArcadeBot.Infrastructure.Entities.Server", "Server")
                        .WithMany("ConfigOverrides")
                        .HasForeignKey("ServerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Server");
                });

            modelBuilder.Entity("ServerServerUser", b =>
                {
                    b.HasOne("ArcadeBot.Infrastructure.Entities.Server", null)
                        .WithMany()
                        .HasForeignKey("GuildsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ArcadeBot.Infrastructure.Entities.ServerUser", null)
                        .WithMany()
                        .HasForeignKey("UsersId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("ArcadeBot.Infrastructure.Entities.Server", b =>
                {
                    b.Navigation("ConfigOverrides");
                });
#pragma warning restore 612, 618
        }
    }
}
