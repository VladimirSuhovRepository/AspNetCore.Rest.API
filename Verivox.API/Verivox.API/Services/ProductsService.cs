using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Verivox.API.Model;
using Verivox.API.UOW;

namespace Verivox.API.Services
{
    /// <summary>
    /// Service to calculate the product details.
    /// </summary>
    public interface IProductsService
    {
        /// <summary>
        /// Calculates exact product for defined tariff and consumed parameter.
        /// </summary>
        /// <param name="tariffType">The type of Tariff: (Base, Packaged).</param>
        /// <param name="consumption">Consumed amount of kWh per year (kWh/year).</param>
        /// <returns>Product details for defined tariff and consumed parameter.</returns>
        Task<Product> GetProduct(TariffType tariffType, long consumption);

        /// <summary>
        /// Calculates products for all tariffs according to consumed parameter.
        /// </summary>
        /// <param name="consumption">Consumed amount of kWh per year (kWh/year).</param>
        /// <returns>List of products sorted by Annual Cost in ascending order.</returns>
        Task<IEnumerable<Product>> GetAllProducts(long consumption);
    }

    internal class ProductsService : IProductsService
    {
        private readonly IUnitOfWork unitOfWork;

        public ProductsService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public async Task<Product> GetProduct(TariffType tariffType, long consumption)
        {
            switch (tariffType)
            {
                case TariffType.Base:
                    return await this.GetBaseProduct(consumption).ConfigureAwait(false);
                case TariffType.Packaged:
                    return await this.GetPackagedProduct(consumption).ConfigureAwait(false);
                default:
                    throw new ArgumentOutOfRangeException(nameof(tariffType), tariffType, null);
            }
        }

        public async Task<IEnumerable<Product>> GetAllProducts(long consumption)
        {
            var result = new List<Product>();
            var tasks = new List<Task<Product>>();

            foreach (TariffType tariff in Enum.GetValues(typeof(TariffType)))
            {
                tasks.Add(this.GetProduct(tariff, consumption));
            }

            var products = await Task.WhenAll(tasks).ConfigureAwait(false);
            products = products.OrderBy(s => s.AnnualCosts).ToArray();
            result.AddRange(products);

            return result;
        }

        private async Task<Product> GetBaseProduct(long consumption)
        {
            var baseTariff = await this.unitOfWork.BaseTariffRepository.GetActualTariff().ConfigureAwait(false);
            var annualCosts = (baseTariff.BaseCost * 12) + (baseTariff.ConsumptionCost * consumption);

            return new Product(baseTariff.Name, annualCosts);
        }

        private async Task<Product> GetPackagedProduct(long consumption)
        {
            var packagedTariff = await this.unitOfWork.PackagedTariffRepository.GetActualTariff().ConfigureAwait(false);

            if (consumption <= packagedTariff.IncludedConsumptionLevel)
            {
                return new Product(packagedTariff.Name, packagedTariff.PackageCost);
            }

            var annualCosts = packagedTariff.PackageCost + (packagedTariff.ConsumptionCost * (consumption - packagedTariff.IncludedConsumptionLevel));

            return new Product(packagedTariff.Name, annualCosts);
        }
    }
}
