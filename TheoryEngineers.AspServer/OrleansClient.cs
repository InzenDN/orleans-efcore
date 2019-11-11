using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Orleans;
using Orleans.Configuration;
using System;
using System.Threading.Tasks;
using TheoryEngineers.Models.Data;

namespace TheoryEngineers.AspServer
{
    public static class OrleansClient
    {
        public static IClusterClient ConnectClient()
        {
            IClusterClient client = new ClientBuilder()
                .UseLocalhostClustering()
                .Configure<ClusterOptions>(options =>
                {
                    options.ClusterId = "dev";
                    options.ServiceId = "Theory";
                })
                .ConfigureLogging(logging => logging.AddConsole())
                .Build();

            client.Connect(async ex => {
                Console.WriteLine(ex);
                Console.WriteLine("Retrying...");
                await Task.Delay(3000);
                return true;
            }).Wait();

            Console.WriteLine("ASP.NET server connected successfully to Silo host... \n");

            return client;
        }
    }
}
