using System.Linq;
using Microsoft.EntityFrameworkCore;
using Verivox.API.DTO;

namespace Verivox.API.Database
{
    internal class ProductsDbContext : DbContext
    {
        public ProductsDbContext(DbContextOptions<ProductsDbContext> dbContextOptions)
            : base(dbContextOptions)
        {
            this.SeedData();
        }

        public DbSet<BaseTariff> BaseTariffs { get; set; }

        public DbSet<PackagedTariff> PackagedTariffs { get; set; }

        public Model.TariffType TariffType { get; set; }

        private void SeedData()
        {
            this.Database.EnsureCreated();

            if (this.BaseTariffs.FirstOrDefault() == null)
            {
                this.BaseTariffs.Add(new BaseTariff
                {
                    BaseTariffId = 1,
                    Name = "basic electricity tariff",
                    BaseCost = 5,
                    ConsumptionCost = 0.22F,
                });
            }

            if (this.PackagedTariffs.FirstOrDefault() == null)
            {
                this.PackagedTariffs.Add(new PackagedTariff
                {
                    PackagedTariffId = 1,
                    Name = "Packaged  tariff",
                    IncludedConsumptionLevel = 4000,
                    PackageCost = 800,
                    ConsumptionCost = 0.30F,
                });
            }

            this.SaveChanges();
        }
    }
}
