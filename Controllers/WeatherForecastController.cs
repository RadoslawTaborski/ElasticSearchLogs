using Microsoft.AspNetCore.Mvc;

namespace ElasticSearchLogs.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    private readonly Serilog.ILogger _logger;

    public WeatherForecastController(Serilog.ILogger logger)
    {
        _logger = logger;
    }

    [HttpGet(Name = "GetWeatherForecast")]
    public IActionResult Get()
    {
        try
        {
            var random = new Random();
            var randomValue = Random.Shared.Next(-50, 55);
            _logger.Information($"Random Value is {randomValue}");

            if (randomValue < -20)
            {
                throw new Exception("Too cold.");
            }

            return Ok(Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = randomValue,
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray());
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Something bad happened {CustomProperty}", 50);
            return new StatusCodeResult(500);
        }
    }
}
