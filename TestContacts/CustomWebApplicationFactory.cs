using Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;


namespace TestContacts
{
    public class CustomWebApplicationFactory : WebApplicationFactory<Program>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            base.ConfigureWebHost(builder);

            builder.UseEnvironment("Test");

            builder.ConfigureServices(services =>
            {
                // 1. Remove ALL existing DbContext registrations for ApplicationDbContext
                var descriptors = services
                    .Where(d =>
                        d.ServiceType == typeof(DbContextOptions<ApplicationDbContext>) ||
                        d.ServiceType == typeof(IDbContextOptionsConfiguration<ApplicationDbContext>) ||
                        d.ImplementationType?.IsAssignableTo(typeof(ApplicationDbContext)) == true ||
                        d.ServiceType == typeof(ApplicationDbContext))
                    .ToList();

                foreach (var descriptor in descriptors)
                {
                    services.Remove(descriptor);
                }               

                // 2. Register fresh DbContext with InMemory
                services.AddDbContext<ApplicationDbContext>(options =>
                {
                    options.UseInMemoryDatabase("DatabaseForTesting_" + Guid.NewGuid().ToString("N"));
                   
                });

                // Very useful in many cases — force EF to validate the model only once
                services.AddSingleton<DbContextOptions<ApplicationDbContext>>(provider =>
                {
                    var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
                    optionsBuilder.UseInMemoryDatabase("DatabaseForTesting");
                    return optionsBuilder.Options;
                });
            });
        }

        // Optional: give each test run a fresh in-memory database name
        protected override IHost CreateHost(IHostBuilder builder)
        {
            var host = base.CreateHost(builder);
            // You can also seed data here if needed (one-time per factory lifetime)
            return host;
        }

    }
}
