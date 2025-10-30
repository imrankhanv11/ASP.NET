using BookManagement.API;
using BookManagement.DataAccessLayer.Data;
using BookManagement.DataAccessLayer.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;

namespace BookManagement.Tests
{
    public class CustomWebApplicationFactory : WebApplicationFactory<Program>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                // Remove the existing BookContext registration
                var descriptor = services.SingleOrDefault(
                    d => d.ServiceType == typeof(DbContextOptions<BookContext>));

                if (descriptor != null)
                    services.Remove(descriptor);

                // Add a new InMemory EF Core database
                services.AddDbContext<BookContext>(options =>
                {
                    // Use a unique name per test run to isolate data
                    options.UseInMemoryDatabase(databaseName: "test_db");
                });

                // Build service provider and ensure DB created
                var sp = services.BuildServiceProvider();

                using (var scope = sp.CreateScope())
                {
                    var db = scope.ServiceProvider.GetRequiredService<BookContext>();
                    db.Database.EnsureDeleted();
                    db.Database.EnsureCreated();

                     //Optionally seed initial data
                    SeedTestData(db);
                }
            });
        }

        private void SeedTestData(BookContext db)
        {
            db.Books.AddRange(
                new Book { BookId = 1, Title = "Clean Code", Author = "Robert Martin" },
                new Book { BookId = 2, Title = "The Pragmatic Programmer", Author = "Andrew Hunt" }
            );

            db.SaveChanges();
        }
    }
}
