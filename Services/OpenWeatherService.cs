using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using WeatherApp.Data;
using WeatherApp.DTOs;
using WeatherApp.Models;
using static WeatherApp.Models.FetchLocationDataDTO;

namespace WeatherApp.Services;

public class OpenWeatherService : IOpenWeatherService
{
  private readonly HttpClient _httpClient;
  private readonly LocationWeatherResponseRepository _locationWeatherResponseRepository;
  private LocationWeatherEntity _cachedWeatherData;
  private DateTime _lastFetchTime;
  public OpenWeatherService(HttpClient httpClient)
  {
      _httpClient = httpClient;
      _locationWeatherResponseRepository = new LocationWeatherResponseRepository();
      _cachedWeatherData = null;
      _lastFetchTime = DateTime.MinValue;
  }
  public async Task<LocationWeatherResponseDTO> GetLocationData(FetchLocationDataDTO locationDataDTO)
  {
      // Check if cached data is still valid
      // if (_cachedWeatherData != null && (DateTime.Now - _lastFetchTime).TotalMinutes < 2)
      // {
      //   return MapToDTO(_cachedWeatherData);
      // }

      // Call openWeather API to get location data
        var httpResponse = await _httpClient.GetAsync($"https://api.openweathermap.org/data/2.5/weather?lat={locationDataDTO.Lat}&lon={locationDataDTO.Long}&appid=867a62be15a34814758faccd0ae342ea");
        
        var locationWeatherResponseDTO = new LocationWeatherResponseDTO();

        if(httpResponse.IsSuccessStatusCode){
          var responseDataString = await httpResponse.Content.ReadAsStringAsync();
          // on sucess, parse string response to a defined type
          locationWeatherResponseDTO = JsonConvert.DeserializeObject<LocationWeatherResponseDTO>(responseDataString);

          // locationWeatherResponseDTO.id = Guid.NewGuid().ToString();

          // Return data to the caller
          return locationWeatherResponseDTO;
        }
          
      var existsingLonAndLat = _locationWeatherResponseRepository.GetWeatherByLonAndLat(locationDataDTO.Long, locationDataDTO.Lat);
      if (existsingLonAndLat != null && existsingLonAndLat.createdAt < DateTime.Now.AddMinutes(2))
      {
        return MapToDTO(_cachedWeatherData);
      }
      if (existsingLonAndLat != null)
      {
        if (existsingLonAndLat.createdAt < DateTime.Now.AddMinutes(2))
        {   
          var locationWeatherEntity = new LocationWeatherEntity();       
          locationWeatherEntity.id = locationWeatherResponseDTO.id;
          locationWeatherEntity.name = locationWeatherResponseDTO.name;
          locationWeatherEntity.lon = locationWeatherResponseDTO.lon;
          locationWeatherEntity.lat = locationWeatherResponseDTO.lat;
          locationWeatherEntity.createdAt = locationWeatherResponseDTO.createdAt;
          locationWeatherEntity.clouds = JsonConvert.DeserializeObject<Clouds>(locationWeatherResponseDTO.clouds);
          locationWeatherEntity.weather = JsonConvert.SerializeObject<weather>(locationWeatherResponseDTO.weather);
          locationWeatherEntity.main = JsonConvert.SerializeObject<Clouds>(locationWeatherResponseDTO.clouds)
          locationWeatherEntity.sys = locationWeatherResponseDTO.sys;
          locationWeatherEntity.wind = locationWeatherResponseDTO.wind;
          locationWeatherEntity.@base = locationWeatherResponseDTO.@base;
          locationWeatherEntity.visibility = locationWeatherResponseDTO.visibility;
          locationWeatherEntity.timezone = locationWeatherResponseDTO.timezone;
          locationWeatherEntity.dt = locationWeatherResponseDTO.dt;
          locationWeatherEntity.cod = locationWeatherResponseDTO.cod;

          // Save changes to db
          _locationWeatherResponseRepository.UpdateWeatherData(locationWeatherResponseDTO);

          return locationWeatherResponseDTO;

          // Save changes to db
        // _locationWeatherResponseRepository.UpdateWeatherData(locationWeatherResponseDTO);

        // var weatherResponse = MapToDTO(locationWeatherResponseDTO);
        }
        else
        {
          locationWeatherResponseDTO.id = existsingLonAndLat.id;
          locationWeatherResponseDTO.name = existsingLonAndLat.name;
          locationWeatherResponseDTO.lon = existsingLonAndLat.lon;
          locationWeatherResponseDTO.lat = existsingLonAndLat.lat;
          locationWeatherResponseDTO.createdAt = existsingLonAndLat.createdAt;
          locationWeatherResponseDTO.clouds = JsonConvert.DeserializeObject<Clouds>(existsingLonAndLat.clouds);
          locationWeatherResponseDTO.weather = JsonConvert.DeserializeObject<List<Weather>>(existsingLonAndLat.weather);
          locationWeatherResponseDTO.main = JsonConvert.DeserializeObject<Main>(existsingLonAndLat.main);
          locationWeatherResponseDTO.sys = JsonConvert.DeserializeObject<Sys>(existsingLonAndLat.sys);
          locationWeatherResponseDTO.wind = JsonConvert.DeserializeObject<Wind>(existsingLonAndLat.wind);
          locationWeatherResponseDTO.@base = existsingLonAndLat.@base;
          locationWeatherResponseDTO.visibility = existsingLonAndLat.visibility;
          locationWeatherResponseDTO.timezone = existsingLonAndLat.timezone;
          locationWeatherResponseDTO.dt = existsingLonAndLat.dt;
          locationWeatherResponseDTO.cod = existsingLonAndLat.cod;

          // Save changes to db
          _locationWeatherResponseRepository.UpdateWeatherData(existsingLonAndLat);

          return locationWeatherResponseDTO;
        }
        
        
      }
      else
      {
        // Update cache with new data
         UpdateCache(locationWeatherResponseDTO);

        return null; 
      }
  }

  private void UpdateCache(LocationWeatherResponseDTO locationWeatherResponseDTO)
  {
    // // Check if a field with the same id already exists in the cache
    // var existingField = _locationWeatherResponseRepository.GetWeatherById(locationWeatherResponseDTO.id);

    // if (existingField != null)
    // {
    //   // Update the existing field with new data
    //   // existingField.ID = locationWeatherResponseDTO.ID;
    //   existingField.id = locationWeatherResponseDTO.id;
    //   existingField.name = locationWeatherResponseDTO.name;
    //   existingField.lon = locationWeatherResponseDTO.lon;
    //   existingField.lat = locationWeatherResponseDTO.lat;
    //   existingField.clouds = JsonConvert.SerializeObject(locationWeatherResponseDTO.clouds);
    //   // existingField.coord = JsonConvert.SerializeObject(locationWeatherResponseDTO.coord);
    //   existingField.weather = JsonConvert.SerializeObject(locationWeatherResponseDTO.weather);
    //   existingField.main = JsonConvert.SerializeObject(locationWeatherResponseDTO.main);
    //   existingField.sys = JsonConvert.SerializeObject(locationWeatherResponseDTO.sys);
    //   existingField.wind = JsonConvert.SerializeObject(locationWeatherResponseDTO.wind);
    //   existingField.@base = locationWeatherResponseDTO.@base;
    //   existingField.visibility = locationWeatherResponseDTO.visibility;
    //   existingField.timezone = locationWeatherResponseDTO.timezone;
    //   existingField.dt = locationWeatherResponseDTO.dt;
    //   existingField.cod = locationWeatherResponseDTO.cod;

    //   // Save changes to db
    //   _locationWeatherResponseRepository.UpdateWeatherData(existingField);
    // }
    // else
    // {
      // Create db entity in object form and save in db
      var locationData = new LocationWeatherEntity
      {
        // ID = locationWeatherResponseDTO.ID,
        id = Guid.NewGuid().ToString(),
        name = locationWeatherResponseDTO.name,
        lon = locationWeatherResponseDTO.lon,
        lat = locationWeatherResponseDTO.lat,
        clouds = JsonConvert.SerializeObject(locationWeatherResponseDTO.clouds),
        // coord = JsonConvert.SerializeObject(locationWeatherResponseDTO.coord),
        weather = JsonConvert.SerializeObject(locationWeatherResponseDTO.weather),
        main = JsonConvert.SerializeObject(locationWeatherResponseDTO.main),
        sys = JsonConvert.SerializeObject(locationWeatherResponseDTO.sys),
        wind = JsonConvert.SerializeObject(locationWeatherResponseDTO.wind),
        @base = locationWeatherResponseDTO.@base,
        visibility = locationWeatherResponseDTO.visibility,
        timezone = locationWeatherResponseDTO.timezone,
        dt = locationWeatherResponseDTO.dt,
        cod = locationWeatherResponseDTO.cod,
      };

      // Save data in cache
        _cachedWeatherData = locationData;
        _lastFetchTime = DateTime.Now;

        // Save to the database
        _locationWeatherResponseRepository.CreateWeatherData(locationData);
    // }
  }



  private LocationWeatherResponseDTO MapToDTO(LocationWeatherEntity entity)
  {
    var mapToDTO = new LocationWeatherResponseDTO
      {
        // ID = entity.ID,
        id = entity.id,
        name = entity.name,
        lon = entity.lon,
        lat = entity.lat,
        createdAt = entity.createdAt,
        clouds = JsonConvert.DeserializeObject<Clouds>(entity.clouds),
        // coord = JsonConvert.DeserializeObject<Coord>(entity.coord),
        weather = JsonConvert.DeserializeObject<List<Weather>>(entity.weather),
        main = JsonConvert.DeserializeObject<Main>(entity.main),
        sys = JsonConvert.DeserializeObject<Sys>(entity.sys),
        wind = JsonConvert.DeserializeObject<Wind>(entity.wind),
        @base = entity.@base,
        visibility = entity.visibility,
        timezone = entity.timezone,
        dt = entity.dt,
        cod = entity.cod,
      };

      return mapToDTO;
  }
}
