﻿// <auto-generated />
using Checkmunk.Data.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using System;

namespace Checkmunk.Data.Migrations
{
    [DbContext(typeof(CheckmunkContext))]
    [Migration("20170920182904_Initial")]
    partial class Initial
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.0.0-rtm-26452");

            modelBuilder.Entity("Checkmunk.Domain.Checklists.AggregateRoots.Checklist", b =>
                {
                    b.Property<int>("PersistenceId")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("CreatedAt");

                    b.Property<int?>("CreatedByPersistenceId");

                    b.Property<Guid>("Id");

                    b.Property<string>("Title");

                    b.HasKey("PersistenceId");

                    b.HasIndex("CreatedByPersistenceId");

                    b.ToTable("Checklist","Checklists");
                });

            modelBuilder.Entity("Checkmunk.Domain.Checklists.Entities.User", b =>
                {
                    b.Property<int>("PersistenceId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("EmailAddress")
                        .IsRequired();

                    b.HasKey("PersistenceId");

                    b.ToTable("ChecklistUser","Checklists");
                });

            modelBuilder.Entity("Checkmunk.Domain.Checklists.ValueObjects.ChecklistItem", b =>
                {
                    b.Property<int>("PersistenceId")
                        .ValueGeneratedOnAdd();

                    b.Property<int?>("ChecklistPersistenceId");

                    b.Property<Guid>("Id");

                    b.Property<bool>("IsChecked");

                    b.Property<string>("Text");

                    b.HasKey("PersistenceId");

                    b.HasIndex("ChecklistPersistenceId");

                    b.ToTable("ChecklistItem","Checklists");
                });

            modelBuilder.Entity("Checkmunk.Domain.Users.AggregateRoots.User", b =>
                {
                    b.Property<int>("PersistenceId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("EmailAddress")
                        .IsRequired();

                    b.Property<string>("FirstName");

                    b.Property<bool>("IsEnabled");

                    b.Property<string>("LastName");

                    b.HasKey("PersistenceId");

                    b.ToTable("User","Users");
                });

            modelBuilder.Entity("Checkmunk.Domain.Checklists.AggregateRoots.Checklist", b =>
                {
                    b.HasOne("Checkmunk.Domain.Checklists.Entities.User", "CreatedBy")
                        .WithMany("Checklists")
                        .HasForeignKey("CreatedByPersistenceId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Checkmunk.Domain.Checklists.ValueObjects.ChecklistItem", b =>
                {
                    b.HasOne("Checkmunk.Domain.Checklists.AggregateRoots.Checklist", "Checklist")
                        .WithMany("Items")
                        .HasForeignKey("ChecklistPersistenceId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Checkmunk.Domain.Users.AggregateRoots.User", b =>
                {
                    b.OwnsOne("Checkmunk.Domain.Users.ValueObjects.Address", "BillingAddress", b1 =>
                        {
                            b1.Property<int>("UserPersistenceId");

                            b1.ToTable("User","Users");

                            b1.HasOne("Checkmunk.Domain.Users.AggregateRoots.User")
                                .WithOne("BillingAddress")
                                .HasForeignKey("Checkmunk.Domain.Users.ValueObjects.Address", "UserPersistenceId")
                                .OnDelete(DeleteBehavior.Cascade);
                        });

                    b.OwnsOne("Checkmunk.Domain.Users.ValueObjects.Address", "MailingAddress", b1 =>
                        {
                            b1.Property<int?>("UserPersistenceId");

                            b1.ToTable("User","Users");

                            b1.HasOne("Checkmunk.Domain.Users.AggregateRoots.User")
                                .WithOne("MailingAddress")
                                .HasForeignKey("Checkmunk.Domain.Users.ValueObjects.Address", "UserPersistenceId")
                                .OnDelete(DeleteBehavior.Cascade);
                        });
                });
#pragma warning restore 612, 618
        }
    }
}
