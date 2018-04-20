using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using u7CoreEF.Extensions;
using Microsoft.Extensions.DependencyInjection;
using u7CoreEF.Infrastructure;

namespace u7CoreEF
{
    public class Program
    {
        public static void Main(string[] args)
        {
            BuildWebHost(args)
                .MigrateDbContext<AppDbContext>((context, services) =>
                {
                    var env = services.GetService<IHostingEnvironment>();
                    //var settings = services.GetService<IOptions<OrderingSettings>>();
                    var logger = services.GetService<ILogger<AppDbContextSeed>>();

                    new AppDbContextSeed()
                       .SeedAsync(context, env, logger)
                       .Wait();
                })
                .Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .Build();
    }
}
