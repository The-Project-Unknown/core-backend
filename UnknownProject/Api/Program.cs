using System;
using Api.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(
    options: new WebApplicationOptions
    {
        EnvironmentName = Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT") ?? "Development",
        Args = args
    }
);
builder.Configuration.ConfigureConfigurationBuilder(builder.Environment.EnvironmentName);
builder.ConfigureUnknownProjectApi();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options => { options.DisplayRequestDuration(); });
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseCors("AllowSpecificOrigin");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.Run();
public abstract partial class Program
{
}