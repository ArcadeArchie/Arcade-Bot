// <auto-generated />
using System;
using ArcadeBot.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace ArcadeBot.Migrations
{
    [DbContext(typeof(ArcadeBotDbContext))]
    partial class SCOBotDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "6.0.9");

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
                        .HasColumnType("TEXT");

                    b.Property<ulong>("QuarantineChannel")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("QuarantineEnabled")
                        .HasColumnType("INTEGER");

                    b.Property<int>("QuarantineHandleBehavior")
                        .HasColumnType("INTEGER");

                    b.Property<ulong>("QuarantineRole")
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
                        .HasColumnType("TEXT");

                    b.Property<string>("OverrideValue")
                        .HasColumnType("TEXT");

                    b.Property<ulong?>("ServerId")
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
                        .HasColumnType("TEXT");

                    b.Property<bool>("IsBot")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Username")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("ArcadeBot.Services.UserMessage", b =>
                {
                    b.Property<ulong>("MessageId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<ulong>("ChannelId")
                        .HasColumnType("INTEGER");

                    b.Property<DateTimeOffset>("CreatedAt")
                        .HasColumnType("TEXT");

                    b.HasKey("MessageId");

                    b.ToTable("CachedMessages");
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
                        .HasForeignKey("ServerId");

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
