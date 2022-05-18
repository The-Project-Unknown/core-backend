using Api;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);


// === Configuration Setup ===
builder.Configuration.AddJsonFile("Settings/appsettings.Development.json", optional: true, reloadOnChange: true);
builder.Configuration.AddCommandLine(args);


// === Logging Setup ===
builder.Logging.ClearProviders();
builder.Logging.AddConsole();


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();


// === Database Setup ===
var dbConnectionSettings = builder.Configuration.GetSection("DbSettings").Get<DbConnectionSettings>();

builder.Services.AddDbContext<ApiDbContext>(optionsBuilder =>
{
    optionsBuilder.UseNpgsql(
        $@"User ID={dbConnectionSettings.User};Password={dbConnectionSettings.Password};Host={dbConnectionSettings.Host};Port={dbConnectionSettings.Port};Database={dbConnectionSettings.Database};");
    optionsBuilder.EnableSensitiveDataLogging();
});


// === Swagger Setup ===
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "UnknownProject API",
        Description = "Meh.."
    });
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");

        options.InjectStylesheet("/swagger-ui/custom.css");
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseStaticFiles();
app.MapControllers();

app.Run();


public class DbConnectionSettings
{
    public string Host { get; set; }
    public string User { get; set; }
    public string Password { get; set; }
    public string Database { get; set; }
    public string Port { get; set; }
}

