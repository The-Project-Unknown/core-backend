using Amazon;
using Amazon.S3;
using Amazon.Util;
using Api;
using Api.Auth;
using Api.Providers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// === Configuration Setup ===
builder.Configuration.AddJsonFile("Settings/appsettings.Development.json", optional: true, reloadOnChange: true);
builder.Configuration.AddCommandLine(args);


// === Logging Setup ===
builder.Logging.ClearProviders();
builder.Logging.AddConsole();


// === Database Setup ===
var dbConnectionSettings = builder.Configuration.GetSection("DbSettings").Get<DbConnectionSettings>();

builder.Services.AddDbContext<ApiDbContext>(optionsBuilder =>
{
    optionsBuilder.UseNpgsql(
        $@"User ID={dbConnectionSettings.User};Password={dbConnectionSettings.Password};Host={dbConnectionSettings.Host};Port={dbConnectionSettings.Port};Database={dbConnectionSettings.Database};");
    optionsBuilder.EnableSensitiveDataLogging();
});


// === Cors Setup ===
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigin", policy =>
    {
        policy.AllowAnyHeader()
            .AllowAnyMethod()
            .SetIsOriginAllowed(origin => true)
            .AllowCredentials();
    });
});


var domain = $"https://{builder.Configuration["Auth0:Domain"]}/";
var t = "https://" + builder.Configuration["Auth0:Domain"].ToString() + "/authorize?audience=" +
        builder.Configuration["Auth0:Audience"];

// === Auth and Authorization Setup ===
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.Authority = domain;
    options.Audience = builder.Configuration["Auth0:Audience"];
});


builder.Services.AddAuthorization(options =>
{
    Enum.GetNames(typeof(AuthorizePolicy)).ToList().ForEach(policyName =>
    {
        options.AddPolicy(policyName,
            policy => policy.Requirements.Add(new HasPermissionRequirement(policyName, domain)));
    });
});

builder.Services.AddSingleton<IAuthorizationHandler, HasScopeHandler>();

// === Add AWS services
builder.Services.AddScoped<IAWSS3StorageService, AWSS3StorageService>();

builder.Services.AddDefaultAWSOptions(builder.Configuration.GetAWSOptions());
builder.Services.AddAWSService<IAmazonS3>();


// === Oother services ===
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddHttpContextAccessor();

builder.Services.AddSwaggerGen(option =>
{
    option.SwaggerDoc("v1", new OpenApiInfo { Title = "Demo API", Version = "v1"});

    option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter a valid token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });
    option.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id="Bearer"
                }
            },
            new string[]{}
        }
    });
});

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.DisplayRequestDuration();
        options.InjectStylesheet("/css/swagger-ui/custom.css");
    });
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseCors("AllowSpecificOrigin");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.Run();


public partial class Program { }

public enum AuthorizePolicy
{
    read,
    write
}


public class DbConnectionSettings
{
    public string Host { get; set; }
    public string User { get; set; }
    public string Password { get; set; }
    public string Database { get; set; }
    public string Port { get; set; }
}

