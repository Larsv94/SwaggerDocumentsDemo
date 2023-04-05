using SwaggerDocumentsDemo.Api.Config;
using SwaggerDocumentsDemo.Api.Data;

namespace SwaggerDocumentsDemo.Api.Endpoints;

public static class PublicWeatherEndpoints
{
    private static readonly string[] summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    public static IEndpointRouteBuilder AddPublicWeatherApi(this IEndpointRouteBuilder app)
    {
        var weatherApi = app
            .MapGroup("weather")
            .WithMetadata(new DocumentData { RequiresAuthorization = false })
            .WithOpenApi();

        weatherApi.MapGet("/", () => InMemoryDatabase.Weather)
            .WithName("GetWeather");

        weatherApi.MapGet("/{date}", (DateOnly date) => InMemoryDatabase.Weather.FirstOrDefault(weather => weather.Date == date))
            .WithName("GetWeatherByDate");

        return app;
    }
}

