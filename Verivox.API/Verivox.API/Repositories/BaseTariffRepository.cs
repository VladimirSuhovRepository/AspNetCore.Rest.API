using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Verivox.API.Database;
using Verivox.API.DTO;

namespace Verivox.API.Repositories
{
    internal interface IBaseTariffRepository
    {
        Task<BaseTariff> GetActualTariff();
    }

    internal class BaseTariffRepository : IBaseTariffRepository
    {
        private readonly ProductsDbContext productsDbContext;

        public BaseTariffRepository(ProductsDbContext productsDbContext)
        {
            this.productsDbContext = productsDbContext;
        }

        public Task<BaseTariff> GetActualTariff()
        {
            return this.productsDbContext.BaseTariffs.FirstOrDefaultAsync();
        }
    }
}
