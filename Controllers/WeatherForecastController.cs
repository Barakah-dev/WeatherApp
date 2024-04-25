using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WeatherApp.Context;
using WeatherApp.Data;
using WeatherApp.DTOs;
using WeatherApp.Models;
using WeatherApp.Services;

namespace WeatherApp.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    private readonly ILogger<WeatherForecastController> _logger;
    private readonly IOpenWeatherService _openWeatherService;

    public WeatherForecastController(ILogger<WeatherForecastController> logger, IOpenWeatherService openWeatherService)
    {
        _logger = logger;
        _openWeatherService = openWeatherService;
    }

    [HttpGet(Name = "GetWeatherForecast")]
    public IEnumerable<WeatherForecast> Get()
    {
        return Enumerable.Range(1, 5).Select(index => new WeatherForecast
        {
            Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            TemperatureC = Random.Shared.Next(-20, 55),
            Summary = Summaries[Random.Shared.Next(Summaries.Length)]
        })
        .ToArray();
    }

    [HttpPost("get-location-data")]
    public async Task<IActionResult> Post([FromBody] FetchLocationDataDTO fetchLocationDataDTO)
    {
        var response = await _openWeatherService.GetLocationData(fetchLocationDataDTO);

        return Ok(response);        
    }
}
