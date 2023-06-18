using EntityFrameworkProject.DbModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace EntityFrameworkProject.DbContexts
{
    public class CountryDbContext : DbContext
    {
        public DbSet<Country> Countries { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            ConfigurationBuilder builder = new();

            builder.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

            IConfigurationRoot configuration = builder.Build();

            string connectionString = configuration.GetConnectionString("LocalConnection");

            optionsBuilder.UseSqlServer(connectionString);
        }

        protected void OnModelCreating(ModelBuilder modelBuilder)
        {
            var country = modelBuilder.Entity<Country>();

            country.HasKey(x => x.Id);
            country.Property(x => x.Name).IsRequired();
            country.Property(x => x.Government).IsRequired();
            country.Property(x => x.ImageURL).IsRequired();
            country.Property(x => x.Population).IsRequired();
            country.Property(x => x.Area).IsRequired();
            country.Property(x => x.GDP).IsRequired();
        }



    }
}
