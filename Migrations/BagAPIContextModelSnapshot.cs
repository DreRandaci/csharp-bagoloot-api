using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using BagAPI.Data;

namespace BagAPI.Migrations
{
    [DbContext(typeof(BagAPIContext))]
    partial class BagAPIContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.1.2");

            modelBuilder.Entity("BagAPI.Models.Child", b =>
                {
                    b.Property<int>("ChildId")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("Delivered");

                    b.Property<string>("Name")
                        .IsRequired();

                    b.HasKey("ChildId");

                    b.ToTable("Child");
                });

            modelBuilder.Entity("BagAPI.Models.FavoriteReindeer", b =>
                {
                    b.Property<int>("FavoriteReindeerId")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("ChildId");

                    b.Property<int>("ReindeerId");

                    b.HasKey("FavoriteReindeerId");

                    b.HasIndex("ChildId");

                    b.HasIndex("ReindeerId");

                    b.ToTable("FavoriteReindeer");
                });

            modelBuilder.Entity("BagAPI.Models.Reindeer", b =>
                {
                    b.Property<int>("ReindeerId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name")
                        .IsRequired();

                    b.HasKey("ReindeerId");

                    b.ToTable("Reindeer");
                });

            modelBuilder.Entity("BagAPI.Models.Toy", b =>
                {
                    b.Property<int>("ToyId")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("ChildId");

                    b.Property<string>("Name")
                        .IsRequired();

                    b.HasKey("ToyId");

                    b.HasIndex("ChildId");

                    b.ToTable("Toy");
                });

            modelBuilder.Entity("BagAPI.Models.FavoriteReindeer", b =>
                {
                    b.HasOne("BagAPI.Models.Child", "Child")
                        .WithMany()
                        .HasForeignKey("ChildId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("BagAPI.Models.Reindeer", "Reindeer")
                        .WithMany("Fans")
                        .HasForeignKey("ReindeerId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("BagAPI.Models.Toy", b =>
                {
                    b.HasOne("BagAPI.Models.Child", "Child")
                        .WithMany("Toys")
                        .HasForeignKey("ChildId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
        }
    }
}
