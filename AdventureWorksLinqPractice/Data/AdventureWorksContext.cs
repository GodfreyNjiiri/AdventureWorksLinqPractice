using Microsoft.EntityFrameworkCore;

namespace AdventureWorksLinqPractice.Data
{
    public class AdventureWorksContext : DbContext
    {
        // Customer DBset
        //public DbSet<Customer> Customers { get; set; }
        //public DbSet<Product> Products { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //var credentials = JsonSerializer.Deserialize<dynamic>(File.ReadAllText("Credentials.json"));
            //var connectionString = $"Server=localhost;Database={credentials.Database};Trusted_Connection=True;";
            var connectionString = "Server=localhost;Database=AdventureWorks2022;Trusted_Connection=True;";

            optionsBuilder.UseSqlServer(connectionString);
        }
    }
}
