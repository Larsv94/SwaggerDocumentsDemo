using Microsoft.OpenApi.Models;
using SwaggerDocumentsDemo.Api.Endpoints;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(config =>
{
    config.SwaggerDoc("public", new OpenApiInfo { Title = "My Weather Api - Public endpoints" });
    config.SwaggerDoc("internal", new OpenApiInfo { Title = "My Weather Api - Internal endpoints" });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(config =>
    {
        config.SwaggerEndpoint("public/swagger.json", "Public endpoints");
        config.SwaggerEndpoint("internal/swagger.json", "Internal endpoints");
    });
}

app.UseHttpsRedirection();

app.AddPublicWeatherApi();
app.AddInternalWeatherApi();

app.Run();
