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

namespace WeatherApp.Services
{
    public class OpenWeatherService : IOpenWeatherService
    {
        private readonly HttpClient _httpClient;
        private readonly LocationWeatherResponseRepository _locationWeatherResponseRepository;
        public OpenWeatherService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _locationWeatherResponseRepository = new LocationWeatherResponseRepository();
        }

        public async Task<LocationWeatherResponseDTO> GetLocationData(FetchLocationDataDTO locationDataDTO)
        {
            // Check if weather data for the given latitude and longitude exists in the database
            var existingData = _locationWeatherResponseRepository.GetWeatherByLonAndLat(locationDataDTO.Lat, locationDataDTO.Long);
            
            if (existingData != null)
            {
                // Check if the existing data is still valid (less than 2 minutes old)
                // if (existingData.createdAt < DateTime.UtcNow.AddMinutes(2))
                if (existingData.createdAt >= DateTime.UtcNow.AddMinutes(-2))
                {
                    // If the data is still valid, map it to DTO and return
                    return MapToDTO(existingData);
                }
                else
                {
                  // Data is outdated, update it with new data from the API
                  var newData = await GetOpenWeatherData(locationDataDTO);
                  UpdateCache(existingData, newData);
                  return newData;
                }
            }
            else
            {
              // Data doesn't exist, fetch new data from the API
              var newData = await GetOpenWeatherData(locationDataDTO);
              UpdateCache(null, newData);
              return newData;
            }
        }

        private async Task<LocationWeatherResponseDTO> GetOpenWeatherData(FetchLocationDataDTO locationDataDTO)
        {
          var httpResponse = await _httpClient.GetAsync($"https://api.openweathermap.org/data/2.5/weather?lat={locationDataDTO.Lat}&lon={locationDataDTO.Long}&appid=867a62be15a34814758faccd0ae342ea");
          
          if (httpResponse.IsSuccessStatusCode)
          {
              var responseDataString = await httpResponse.Content.ReadAsStringAsync();
              var locationWeatherResponseDTO = JsonConvert.DeserializeObject<LocationWeatherResponseDTO>(responseDataString);
              return locationWeatherResponseDTO;
          }
          else
          {
              // Handle unsuccessful API response
              throw new Exception("Failed to fetch weather data from the API.");
          }
        }

        private void UpdateCache(LocationWeatherEntity existingData, LocationWeatherResponseDTO newWeatherData)
        {
            if (existingData != null)
            {
                // Update existing data in the database
                existingData.createdAt = DateTime.UtcNow;
                existingData.name = newWeatherData.name;
                existingData.lat = newWeatherData.coord.lat;
                existingData.lon = newWeatherData.coord.lon;
                existingData.clouds = JsonConvert.SerializeObject(newWeatherData.clouds);
                existingData.weather = JsonConvert.SerializeObject(newWeatherData.weather);
                existingData.main = JsonConvert.SerializeObject(newWeatherData.main);
                existingData.sys = JsonConvert.SerializeObject(newWeatherData.sys);
                existingData.wind = JsonConvert.SerializeObject(newWeatherData.wind);
                existingData.@base = newWeatherData.@base;
                existingData.visibility = newWeatherData.visibility;
                existingData.timezone = newWeatherData.timezone;
                existingData.dt = newWeatherData.dt;
                existingData.cod = newWeatherData.cod;

                _locationWeatherResponseRepository.UpdateWeatherData(existingData);
            }
            else
            {
                // Create a new record in the database
                var locationData = new LocationWeatherEntity
                {
                    id = Guid.NewGuid().ToString(),
                    createdAt = DateTime.UtcNow,
                    name = newWeatherData.name,
                    lat = newWeatherData.coord.lat,
                    lon = newWeatherData.coord.lon,
                    clouds = JsonConvert.SerializeObject(newWeatherData.clouds),
                    weather = JsonConvert.SerializeObject(newWeatherData.weather),
                    main = JsonConvert.SerializeObject(newWeatherData.main),
                    sys = JsonConvert.SerializeObject(newWeatherData.sys),
                    wind = JsonConvert.SerializeObject(newWeatherData.wind),
                    @base = newWeatherData.@base,
                    visibility = newWeatherData.visibility,
                    timezone = newWeatherData.timezone,
                    dt = newWeatherData.dt,
                    cod = newWeatherData.cod,
                };

                _locationWeatherResponseRepository.CreateWeatherData(locationData);
            }
        }

        private LocationWeatherResponseDTO MapToDTO(LocationWeatherEntity entity)
        {
            return new LocationWeatherResponseDTO
            {
                id = entity.id,
                name = entity.name,
                lat = entity.lat,
                lon = entity.lon,
                createdAt = entity.createdAt,
                clouds = JsonConvert.DeserializeObject<Clouds>(entity.clouds),
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
        }
    }
}