using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Verivox.API.Database;
using Verivox.API.DTO;

namespace Verivox.API.Repositories
{
    internal interface IPackagedTariffRepository
    {
        Task<PackagedTariff> GetActualTariff();
    }

    internal class PackagedTariffRepository : IPackagedTariffRepository
    {
        private readonly ProductsDbContext productsDbContext;

        public PackagedTariffRepository(ProductsDbContext productsDbContext)
        {
            this.productsDbContext = productsDbContext;
        }

        public Task<PackagedTariff> GetActualTariff()
        {
            return productsDbContext.PackagedTariffs.FirstOrDefaultAsync();
        }
    }
}
