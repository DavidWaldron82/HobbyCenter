﻿// <auto-generated />
using System;
using HobbyCenter.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace HobbyCenter.Migrations
{
    [DbContext(typeof(MyContext))]
    partial class MyContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.4-servicing-10062")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("HobbyCenter.Models.Hobby", b =>
                {
                    b.Property<int>("HobbyId")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("CreatedAt");

                    b.Property<string>("Description")
                        .IsRequired();

                    b.Property<string>("Name")
                        .IsRequired();

                    b.Property<DateTime>("UpdatedAt");

                    b.HasKey("HobbyId");

                    b.ToTable("Hobbies");
                });

            modelBuilder.Entity("HobbyCenter.Models.User", b =>
                {
                    b.Property<int>("UserId")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("CreatedAt");

                    b.Property<string>("Email")
                        .IsRequired();

                    b.Property<string>("FirstName")
                        .IsRequired();

                    b.Property<string>("LastName")
                        .IsRequired();

                    b.Property<string>("Password")
                        .IsRequired();

                    b.Property<DateTime>("UpdatedAt");

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasMaxLength(15);

                    b.HasKey("UserId");

                    b.ToTable("User");
                });

            modelBuilder.Entity("HobbyCenter.Models.UserHob", b =>
                {
                    b.Property<int>("UserHobId")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("HobbyId");

                    b.Property<int>("UserId");

                    b.HasKey("UserHobId");

                    b.HasIndex("HobbyId");

                    b.HasIndex("UserId");

                    b.ToTable("UserHobs");
                });

            modelBuilder.Entity("HobbyCenter.Models.UserHob", b =>
                {
                    b.HasOne("HobbyCenter.Models.Hobby", "Hobby")
                        .WithMany("Ethusiasts")
                        .HasForeignKey("HobbyId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("HobbyCenter.Models.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
