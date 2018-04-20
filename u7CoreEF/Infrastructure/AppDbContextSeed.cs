using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Polly;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace u7CoreEF.Infrastructure
{
    public class AppDbContextSeed
    {
        public async Task SeedAsync(AppDbContext context, IHostingEnvironment env, ILogger<AppDbContextSeed> logger)
        {
            var policy = CreatePolicy(logger, nameof(AppDbContextSeed));

            await policy.ExecuteAsync(async () =>
            {
                //var contentRootPath = env.ContentRootPath;

                using (context)
                {
                    context.Database.Migrate();

                    //if (!context.CardTypes.Any())
                    //{
                    //    context.CardTypes.AddRange(useCustomizationData
                    //                            ? GetCardTypesFromFile(contentRootPath, logger)
                    //                            : GetPredefinedCardTypes());

                    //    await context.SaveChangesAsync();
                    //}

                    //if (!context.OrderStatus.Any())
                    //{
                    //    context.OrderStatus.AddRange(useCustomizationData
                    //                            ? GetOrderStatusFromFile(contentRootPath, logger)
                    //                            : GetPredefinedOrderStatus());
                    //}

                    if (!context.SysUser.Any())
                    {
                        context.SysUser.Add(new Models.SysUser() { Id = "1", PassWord = "1", UserName = "1", Email = "1@163.com" });
                    }

                    await context.SaveChangesAsync();
                }
            });
        }

        private Policy CreatePolicy(ILogger<AppDbContextSeed> logger, string prefix, int retries = 3)
        {
            return Policy.Handle<SqlException>().
                WaitAndRetryAsync(
                    retryCount: retries,
                    sleepDurationProvider: retry => TimeSpan.FromSeconds(5),
                    onRetry: (exception, timeSpan, retry, ctx) =>
                    {
                        logger.LogTrace($"[{prefix}] Exception {exception.GetType().Name} with message ${exception.Message} detected on attempt {retry} of {retries}");
                    }
                );
        }
    }
}
