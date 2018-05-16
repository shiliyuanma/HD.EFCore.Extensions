using HD.Host;
using HD.Host.Abstractors;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace HD.EFCore.Extensions.Test
{
    class Program
    {
        static void Main(string[] args)
        {
            BuildConsoleHost(args).Run();
        }


        public static IConsoleHost BuildConsoleHost(string[] args) =>
            ConsoleHostHelper.CreateDefaultBuilder(args)
                .UseConfiguration(new ConfigurationBuilder()
                    .AddEnvironmentVariables()
                    .AddCommandLine(args)
                    .Build())
                .ConfigureServices(services =>
                {
                    services.AddDbContextPool<HDDbContext>(q => q.UseMySql("Server=119.23.168.162;Port=53326;Database=fm_sso;Uid=betareader;Pwd=V6Wq8Z4mxihz;"));
                })
                .UseStartup<Startup>()
                .Build();
    }
}
