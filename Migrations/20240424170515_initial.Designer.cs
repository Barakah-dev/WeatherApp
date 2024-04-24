﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using WeatherApp.Context;

#nullable disable

namespace WeatherApp.Migrations
{
    [DbContext(typeof(DataContext))]
    [Migration("20240424170515_initial")]
    partial class initial
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.16")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("WeatherApp.Models.LocationWeatherEntity", b =>
                {
                    b.Property<string>("id")
                        .HasColumnType("text");

                    b.Property<string>("base")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("clouds")
                        .IsRequired()
                        .HasColumnType("jsonb");

                    b.Property<int>("cod")
                        .HasColumnType("integer");

                    b.Property<int>("dt")
                        .HasColumnType("integer");

                    b.Property<double>("lat")
                        .HasColumnType("double precision");

                    b.Property<double>("lon")
                        .HasColumnType("double precision");

                    b.Property<string>("main")
                        .IsRequired()
                        .HasColumnType("jsonb");

                    b.Property<string>("name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("sys")
                        .IsRequired()
                        .HasColumnType("jsonb");

                    b.Property<int>("timezone")
                        .HasColumnType("integer");

                    b.Property<int>("visibility")
                        .HasColumnType("integer");

                    b.Property<string>("weather")
                        .IsRequired()
                        .HasColumnType("jsonb");

                    b.Property<string>("wind")
                        .IsRequired()
                        .HasColumnType("jsonb");

                    b.HasKey("id");

                    b.ToTable("LocationWeatherData");
                });
#pragma warning restore 612, 618
        }
    }
}