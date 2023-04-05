using SwaggerDocumentsDemo.Api.Config;
using SwaggerDocumentsDemo.Api.Endpoints;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddCustomSwagger();

var app = builder.Build();

app.UseCustomSwagger();

app.UseHttpsRedirection();

app.AddPublicWeatherApi();
app.AddInternalWeatherApi();

app.Run();
