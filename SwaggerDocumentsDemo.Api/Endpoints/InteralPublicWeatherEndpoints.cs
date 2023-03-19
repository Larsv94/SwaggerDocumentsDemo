using SwaggerDocumentsDemo.Api.Data;
using SwaggerDocumentsDemo.Api.Models;

namespace SwaggerDocumentsDemo.Api.Endpoints;

public static class InteralPublicWeatherEndpoints
{

    private static readonly string[] summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    public static IEndpointRouteBuilder AddInternalWeatherApi(this IEndpointRouteBuilder app)
    {
        var weatherApi = app
            .MapGroup("internal/weather")
            .WithGroupName("internal")
            .WithTags("Internal weather endpoints")
            .WithOpenApi();

        weatherApi.MapPost("/", AddWeather)
            .WithName("PostWeather");

        weatherApi.MapPut("/", UpdateWeather)
            .WithName("PutWeather");

        weatherApi.MapPost("/forecast", Forecast)
        .WithName("ForecastWeather");


        return app;
    }

    private static WeatherModel AddWeather(WeatherModel weather)
    {
        InMemoryDatabase.Weather.Add(weather);
        return weather;
    }

    private static WeatherModel Forecast(DateOnly date)
    {
        var forecast = new WeatherModel
            (
                date,
                Random.Shared.Next(-20, 55),
                summaries[Random.Shared.Next(summaries.Length)]
            );
        InMemoryDatabase.Weather.Add(forecast);
        return forecast;
    }

    private static WeatherModel UpdateWeather(WeatherModel weather)
    {

        var existingWeather = InMemoryDatabase.Weather.FirstOrDefault(x => x.Date == weather.Date);
        if (existingWeather != null)
        {
            existingWeather.Summary = weather.Summary;
            existingWeather.TemperatureC = weather.TemperatureC;
        }
        else
        {
            InMemoryDatabase.Weather.Add(weather);
        }
        return weather;

    }
}
