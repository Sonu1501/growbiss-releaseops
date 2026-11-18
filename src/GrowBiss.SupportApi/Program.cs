using System.Text.Json.Serialization;
using GrowBiss.SupportApi.Services;

var builder = WebApplication.CreateBuilder(args);

// Register controllers and JSON enum conversion.
builder.Services
    .AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(
            new JsonStringEnumConverter());
    });

// Swagger/OpenAPI.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Health checks.
builder.Services.AddHealthChecks();

// Register ticket storage as a singleton.
builder.Services.AddSingleton<ITicketStore, InMemoryTicketStore>();

var app = builder.Build();

// Swagger is enabled in Development only.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Map API controllers.
app.MapControllers();

// Application health endpoint.
app.MapHealthChecks("/health");

app.Run();

// Makes the application entry point accessible to integration tests later.
public partial class Program { }