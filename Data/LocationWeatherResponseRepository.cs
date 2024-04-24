using System;
using System.Collections.Generic;
using System.Linq;
using WeatherApp.Context;
using WeatherApp.DTOs;
using WeatherApp.Models;

namespace WeatherApp.Data;

public class LocationWeatherResponseRepository
{
    private readonly DataContext _dataContext;
    public LocationWeatherResponseRepository()
    {
        _dataContext = new DataContext();
    }

    public void CreateWeatherData(LocationWeatherEntity entity )
    {
      _dataContext.LocationWeatherData.Add(entity); //Insert
      _dataContext.SaveChanges(); //save
    }

    public void UpdateWeatherData(LocationWeatherEntity entity)
    {
      _dataContext.LocationWeatherData.Update(entity);
      _dataContext.SaveChanges();
    }

    public LocationWeatherEntity GetWeatherById(string id)
    {
        var fieldExists = _dataContext.LocationWeatherData.FirstOrDefault(f => f.id == id ) ;
        return fieldExists;
    }
    public LocationWeatherEntity GetWeatherByLonAndLat(double lon, double lat)
    {
        var fieldExists = _dataContext.LocationWeatherData.FirstOrDefault(f => f.lon == lon && f.lat == lat) ;
        return fieldExists;
    }
    
    // string? DeleteWeatherData(int id)
    // {
    //   var weatherDataToDelete = _dataContext.LocationWeatherData.FirstOrDefault(data => data.id == id);
    //   if (weatherDataToDelete is not null)
    //   {
    //     _dataContext.LocationWeatherData.Remove(weatherDataToDelete);
    //     _dataContext.SaveChanges();
    //     return id.ToString();
    //   }
    //   return null;
    // }

    // List <LocationWeatherEntity> ListWeatherData()
    // {
    //   return _dataContext.LocationWeatherData.ToList();
    // }

}
