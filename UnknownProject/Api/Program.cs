using Api;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.ConfigureConfigurationBuilder(args);
builder.ConfigureUnknownProjectApi();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.DisplayRequestDuration();
    });
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