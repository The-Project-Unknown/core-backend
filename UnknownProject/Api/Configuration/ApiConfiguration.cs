using Amazon.S3;
using Api.Auth;
using Api.Providers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Serilog;

namespace Api;

public static class ApiConfiguration
{
    public static void ConfigureConfigurationBuilder(this IConfigurationBuilder configuration, string[] args)
    {
        // === Configuration Setup ===
        configuration.AddJsonFile("Settings/appsettings.Development.json", optional: true, reloadOnChange: true);
        configuration.AddCommandLine(args);
    }
    
    
    public static void ConfigureUnknownProjectApi(this WebApplicationBuilder builder)
    {
        ConfigureLogging(builder.Configuration, builder.Logging);

        ConfigureDatabase(builder.Configuration, builder.Services);

        ConfigureAuth(builder.Configuration, builder.Services);

        ConfigureCors(builder.Services);

        ConfigureWebServerService(builder.Configuration, builder.Services);

        ConfigureRuntimeDiService(builder.Configuration, builder.Services);

        ConfigureSwagger(builder.Services);
    }
    

    static void ConfigureLogging(IConfiguration configuration, ILoggingBuilder logging)
    {
        // === Logging Setup ===
        logging.ClearProviders();
        logging.AddSerilog(new LoggerConfiguration().ReadFrom.Configuration(configuration).CreateLogger());
    }


    static void ConfigureDatabase(IConfiguration configuration, IServiceCollection serviceCollection)
    {
        // === Database Setup ===
        var dbConnectionSettings = configuration.GetSection(nameof(DbConfig)).Get<DbConfig>();

        serviceCollection.AddDbContext<ApiDbContext>(optionsBuilder =>
        {
            optionsBuilder.UseNpgsql(
                $@"User ID={dbConnectionSettings.User};Password={dbConnectionSettings.Password};Host={dbConnectionSettings.Host};Port={dbConnectionSettings.Port};Database={dbConnectionSettings.Database};");
            optionsBuilder.EnableSensitiveDataLogging();
        });
    }
    

    static void ConfigureCors(IServiceCollection services)
    {
        // === Cors Setup ===
        services.AddCors(options =>
        {
            options.AddPolicy("AllowAllOrigin", policy =>
            {
                policy.AllowAnyHeader()
                    .AllowAnyMethod()
                    .SetIsOriginAllowed(origin => true)
                    .AllowCredentials();
            });
        });
    }

    
    static void ConfigureAuth(IConfiguration configuration, IServiceCollection services)
    {
        var domain = $"https://{configuration["Auth0:Domain"]}/";

        // === Auth and Authorization Setup ===
        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(options =>
        {
            options.Authority = domain;
            options.Audience = configuration["Auth0:Audience"];
        });

        services.AddAuthorization(options =>
        {
            Enum.GetNames(typeof(AuthorizePolicy)).ToList().ForEach(policyName =>
            {
                options.AddPolicy(policyName,
                    policy => policy.Requirements.Add(new HasPermissionRequirement(policyName, domain)));
            });
        });

        services.AddSingleton<IAuthorizationHandler, HasScopeHandler>();
    }
    

    static void ConfigureWebServerService(IConfiguration configuration, IServiceCollection services)
    {
        // === Other services ===
        services.AddControllers();
        services.AddEndpointsApiExplorer();
        services.AddHttpContextAccessor();
    }


    static void ConfigureRuntimeDiService(IConfiguration configuration, IServiceCollection services)
    {
        services.AddScoped<IAWSS3StorageService, AWSS3StorageService>();
        services.AddDefaultAWSOptions(configuration.GetAWSOptions());
        services.AddAWSService<IAmazonS3>();

        //Services written by us should be registered right here

    }


    static void ConfigureSwagger(IServiceCollection services)
    {
        services.AddSwaggerGen(option =>
        {
            option.SwaggerDoc("v1", new OpenApiInfo { Title = "Demo API", Version = "v1" });

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
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    new string[] { }
                }
            });
        });
    }
}