using System;
using System.Threading.Tasks;
using WeatherApp.DTOs;
using WeatherApp.Models;
using static WeatherApp.Models.FetchLocationDataDTO;

namespace WeatherApp.Services;

public interface IOpenWeatherService
{
    Task<LocationWeatherResponseDTO> GetLocationData(FetchLocationDataDTO locationDataDTO);
}
