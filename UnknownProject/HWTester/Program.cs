using HWTester;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;

public class Program
{
    public static void Main(string[] args)
    {
        var service = new ServiceCollection()
            .AddLogging(builder => builder.AddConsole());
        
        service.AddSingleton<CommandHandler>();
        service.AddSingleton<ZooKeeper>();

        var serviceProvider = service.BuildServiceProvider();

        var logger = serviceProvider.GetRequiredService<ILogger<Program>>();
        
        logger.LogDebug("Starting application");

        //do the actual work here
        
        var handler = serviceProvider.GetService<CommandHandler>();
        handler?.HandleCommands();
        

        logger.LogDebug("All done!");
    }
}