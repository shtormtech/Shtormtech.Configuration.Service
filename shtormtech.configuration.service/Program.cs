using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

using shtormtech.configuration.service.Extensions;

using System.Threading.Tasks;

namespace shtormtech.configuration.service
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            await CreateHostBuilder(args)
                    .Build()
                    .Initialize()
                    .RunAsync();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
