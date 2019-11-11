using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Orleans;
using Orleans.Configuration;
using Orleans.Hosting;
using System;
using System.Threading.Tasks;
using TheoryEngineers.Identity.Grains.UserStore;
using TheoryEngineers.Models.Data;

namespace TheoryEngineers.SiloServer
{
    public class Program
    {
        public static int Main(string[] args)
        {
            return RunMainAsync().Result;
        }

        private static async Task<int> RunMainAsync()
        {
            try
            {
                var host = await StartSilo();
                Console.WriteLine("\n\n Press Enter to terminate silo...\n\n");
                Console.ReadLine();

                await host.StopAsync();

                return 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return 1;
            }
        }

        private static async Task<ISiloHost> StartSilo()
        {
            // define the cluster configuration
            ISiloHost host = new SiloHostBuilder()
                .UseLocalhostClustering()
                .Configure<ClusterOptions>(options =>
                {
                    options.ClusterId = "dev";
                    options.ServiceId = "Theory";
                })
                .AddMemoryGrainStorageAsDefault()
                .ConfigureApplicationParts(parts => parts.AddApplicationPart(typeof(UserStoreGrain).Assembly).WithReferences())
                .ConfigureServices(x => x.AddDbContext<DataContext>(options => options.UseSqlite("Data Source=../TheoryEngineers.AspServer/Test.db")))
                .ConfigureLogging(x => x.AddConsole())
                .Build();

            await host.StartAsync();
            return host;
        }
    }
}
