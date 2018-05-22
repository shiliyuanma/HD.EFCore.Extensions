using HD.EFCore.Extensions.Test.Data;
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
                    services.AddDbContextPool<MyDbContext>(q => q.UseMySql("Server=192.168.4.157;Port=3306;Database=shiliyuanma;Uid=root;Pwd=hd123456;"));
                })
                .UseStartup<Startup>()
                .Build();
    }
}
