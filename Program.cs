using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;//para o CreateScope
using Microsoft.EntityFrameworkCore; //para o MigrateAsync

namespace WebApiSqlLite
{
    public class Program
    {
        public static async Task Main(string[] args)//trocou void por async Task
        {
            //first build
            var host = CreateHostBuilder(args).Build();

            //initialize
            using (var serviceScope = host.Services.CreateScope())
            {
                var serviceProvider = serviceScope.ServiceProvider;
                var isDevelopment = 
                    serviceProvider.GetRequiredService<IWebHostEnvironment>().IsDevelopment();

                using var context = serviceProvider.GetRequiredService<Models.DemoDbContext>();

                if (isDevelopment)
                    await context.Database.EnsureCreatedAsync();
                else
                    await context.Database.MigrateAsync();

    /*           if (isDevelopment)
                {
                    using var userManager = serviceProvider.GetRequiredService<UserManager<AppUser>>();
                    await userManager
                    .CreateAsync(new AppUser { UserName = "dummy", Email = "dummy@dumail.com" },  password: "1234");
                } */
            }

            host.Run();
            //CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
