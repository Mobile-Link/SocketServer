﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SocketServer.Data;

#nullable disable

namespace SocketServer.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20241008001109_AddUniqueConstraint")]
    partial class AddUniqueConstraint
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "8.0.8");

            modelBuilder.Entity("SocketServer.Entities.AccessLog", b =>
                {
                    b.Property<int>("IdAccessLog")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("AccessLocation")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("Date")
                        .HasColumnType("TEXT");

                    b.Property<int>("IdDevice")
                        .HasColumnType("INTEGER");

                    b.Property<int>("IdUser")
                        .HasColumnType("INTEGER");

                    b.HasKey("IdAccessLog");

                    b.HasIndex("IdDevice");

                    b.HasIndex("IdUser");

                    b.ToTable("AccessLogs");
                });

            modelBuilder.Entity("SocketServer.Entities.Device", b =>
                {
                    b.Property<int>("IdDevice")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("AlterationDate")
                        .HasColumnType("TEXT");

                    b.Property<long>("AvailableSpace")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("CreationDate")
                        .HasColumnType("TEXT");

                    b.Property<int>("EnDeviceOs")
                        .HasColumnType("INTEGER");

                    b.Property<int>("IdUser")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("INTEGER");

                    b.Property<string>("LastLocation")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<long>("OccupiedSpace")
                        .HasColumnType("INTEGER");

                    b.HasKey("IdDevice");

                    b.HasIndex("IdUser");

                    b.ToTable("Devices");
                });

            modelBuilder.Entity("SocketServer.Entities.EntitieAction", b =>
                {
                    b.Property<int>("IdAction")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("IdAction");

                    b.ToTable("EnActions");
                });

            modelBuilder.Entity("SocketServer.Entities.EntitieDeviceOS", b =>
                {
                    b.Property<int>("IdDeviceOs")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("IdDeviceOs");

                    b.ToTable("EnDeviceOSs");
                });

            modelBuilder.Entity("SocketServer.Entities.EntitieStatus", b =>
                {
                    b.Property<int>("IdStatus")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("IdStatus");

                    b.ToTable("EnStatuses");
                });

            modelBuilder.Entity("SocketServer.Entities.History", b =>
                {
                    b.Property<int>("IdHistory")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("Date")
                        .HasColumnType("TEXT");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("EnAction")
                        .HasColumnType("INTEGER");

                    b.Property<int>("FkIdUser")
                        .HasColumnType("INTEGER");

                    b.Property<int>("IdDevice")
                        .HasColumnType("INTEGER");

                    b.HasKey("IdHistory");

                    b.HasIndex("FkIdUser");

                    b.HasIndex("IdDevice");

                    b.ToTable("Histories");
                });

            modelBuilder.Entity("SocketServer.Entities.Storage", b =>
                {
                    b.Property<int>("IdStorage")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("IdUser")
                        .HasColumnType("INTEGER");

                    b.Property<long>("StorageLimitBytes")
                        .HasColumnType("INTEGER");

                    b.Property<long>("UsedStorageBytes")
                        .HasColumnType("INTEGER");

                    b.HasKey("IdStorage");

                    b.HasIndex("IdUser");

                    b.ToTable("Storages");
                });

            modelBuilder.Entity("SocketServer.Entities.Transference", b =>
                {
                    b.Property<int>("IdTranference")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("DestinationPath")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("FileExtention")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("FileName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("IdDeviceDestination")
                        .HasColumnType("INTEGER");

                    b.Property<int>("IdDeviceOrigin")
                        .HasColumnType("INTEGER");

                    b.Property<int>("IdUser")
                        .HasColumnType("INTEGER");

                    b.Property<long>("Size")
                        .HasColumnType("INTEGER");

                    b.HasKey("IdTranference");

                    b.HasIndex("IdDeviceDestination");

                    b.HasIndex("IdDeviceOrigin");

                    b.HasIndex("IdUser");

                    b.ToTable("Transfers");
                });

            modelBuilder.Entity("SocketServer.Entities.TransferenceLog", b =>
                {
                    b.Property<int>("IdTransferenceLog")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("Date")
                        .HasColumnType("TEXT");

                    b.Property<int>("EnStatus")
                        .HasColumnType("INTEGER");

                    b.Property<int>("IdTransference")
                        .HasColumnType("INTEGER");

                    b.Property<string>("ServePath")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("IdTransferenceLog");

                    b.HasIndex("IdTransference");

                    b.ToTable("TransferenceLogs");
                });

            modelBuilder.Entity("SocketServer.Entities.User", b =>
                {
                    b.Property<int>("IdUser")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("CreationDate")
                        .HasColumnType("TEXT");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("IdUser");

                    b.HasIndex("Email")
                        .IsUnique();

                    b.HasIndex("Username")
                        .IsUnique();

                    b.ToTable("Users");
                });

            modelBuilder.Entity("SocketServer.Entities.AccessLog", b =>
                {
                    b.HasOne("SocketServer.Entities.Device", "Device")
                        .WithMany()
                        .HasForeignKey("IdDevice")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("SocketServer.Entities.User", "User")
                        .WithMany()
                        .HasForeignKey("IdUser")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Device");

                    b.Navigation("User");
                });

            modelBuilder.Entity("SocketServer.Entities.Device", b =>
                {
                    b.HasOne("SocketServer.Entities.User", "User")
                        .WithMany()
                        .HasForeignKey("IdUser")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("SocketServer.Entities.History", b =>
                {
                    b.HasOne("SocketServer.Entities.User", "User")
                        .WithMany()
                        .HasForeignKey("FkIdUser")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("SocketServer.Entities.Device", "Device")
                        .WithMany()
                        .HasForeignKey("IdDevice")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Device");

                    b.Navigation("User");
                });

            modelBuilder.Entity("SocketServer.Entities.Storage", b =>
                {
                    b.HasOne("SocketServer.Entities.User", "User")
                        .WithMany()
                        .HasForeignKey("IdUser")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("SocketServer.Entities.Transference", b =>
                {
                    b.HasOne("SocketServer.Entities.Device", "DeviceDestination")
                        .WithMany()
                        .HasForeignKey("IdDeviceDestination")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("SocketServer.Entities.Device", "DeviceOrigin")
                        .WithMany()
                        .HasForeignKey("IdDeviceOrigin")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("SocketServer.Entities.User", "User")
                        .WithMany()
                        .HasForeignKey("IdUser")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("DeviceDestination");

                    b.Navigation("DeviceOrigin");

                    b.Navigation("User");
                });

            modelBuilder.Entity("SocketServer.Entities.TransferenceLog", b =>
                {
                    b.HasOne("SocketServer.Entities.Transference", "Transference")
                        .WithMany()
                        .HasForeignKey("IdTransference")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Transference");
                });
#pragma warning restore 612, 618
        }
    }
}
