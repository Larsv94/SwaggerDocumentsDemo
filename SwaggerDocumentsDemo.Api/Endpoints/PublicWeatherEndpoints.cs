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
            .WithGroupName("public")
            .WithTags("Public weather endpoints")
            .WithOpenApi();

        weatherApi.MapGet("/", () => InMemoryDatabase.Weather)
            .WithName("GetWeather");

        weatherApi.MapGet("/{date}", (DateOnly date) => InMemoryDatabase.Weather.FirstOrDefault(weather => weather.Date == date))
            .WithName("GetWeatherByDate");

        return app;
    }
}

