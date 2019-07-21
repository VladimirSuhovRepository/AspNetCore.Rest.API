using System.Linq;
using Microsoft.EntityFrameworkCore;
using Verivox.API.DTO;

namespace Verivox.API.Database
{
    internal class ProductsDbContext : DbContext
    {
        public ProductsDbContext(DbContextOptions<ProductsDbContext> dbContextOptions) : base(dbContextOptions)
        {
            SeedData();
        }
        
        public DbSet<BaseTariff> BaseTariffs { get; set; }
        public DbSet<PackagedTariff> PackagedTariffs { get; set; }
        public Model.TariffType TariffType { get; set; }

        private void SeedData()
        {
            Database.EnsureCreated();

            if (BaseTariffs.FirstOrDefault() == null)
            {
                BaseTariffs.Add(new BaseTariff
                {
                    BaseTariffId = 1,
                    Name = "basic electricity tariff",
                    BaseCost = 5,
                    ConsumptionCost = 0.22F
                });
            }

            if (PackagedTariffs.FirstOrDefault() == null)
            {
                PackagedTariffs.Add(new PackagedTariff
                {
                    PackagedTariffId = 1,
                    Name = "Packaged  tariff",
                    IncludedConsumptionLevel = 4000,
                    PackageCost = 800,
                    ConsumptionCost = 0.30F
                });
            }

            SaveChanges();
        }
    }
}
