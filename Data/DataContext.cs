using System;
using Microsoft.EntityFrameworkCore;
using WeatherApp.Models;

namespace WeatherApp.Context;

public class DataContext : DbContext
{
    public DbSet<LocationWeatherEntity> LocationWeatherData { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseNpgsql(@"Host=localhost;Port=5432;Username=postgres;Password=omolola2003;Database=LocationWeatherDb");
}

