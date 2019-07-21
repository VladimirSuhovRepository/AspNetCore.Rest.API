using System;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using Verivox.API.Model;
using Verivox.API.Services;

namespace Verivox.API.Controllers
{
    /// <summary>
    /// Get products depending on tariff and consumption
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductsService productsService;

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="productsService"></param>
        public ProductsController(IProductsService productsService)
        {
            this.productsService = productsService ?? throw new ArgumentNullException(nameof(productsService));
        }

        /// <summary>
        /// Calculates products for all tariffs according to consumed parameter
        /// </summary>
        /// <param name="consumption">Consumed amount of kWh</param>
        /// <returns>List of products sorted by Annual Cost in ascending order</returns>
        [HttpGet("{consumption}")]
        public async Task<IEnumerable<Product>> Get(long consumption)
        {
            if (consumption <= 0) throw new ArgumentOutOfRangeException(nameof(consumption));

            return await productsService.GetAllProducts(consumption);
        }

        /// <summary>
        /// Calculates exact product for defined tariff and consumed parameter
        /// </summary>
        /// <param name="tariffType">The type of Tariff: (Base, Packaged)</param>
        /// <param name="consumption">Consumed amount of kWh</param>
        /// <returns>Product details for defined tariff and consumed parameter</returns>
        [HttpGet]
        [Route("product/{tariffType}/{consumption}")]
        public async Task<Product> Get(TariffType tariffType, long consumption)
        {
            if (consumption <= 0) throw new ArgumentOutOfRangeException(nameof(consumption));
            if (!Enum.IsDefined(typeof(TariffType), tariffType))
                throw new InvalidEnumArgumentException(nameof(tariffType), (int) tariffType, typeof(TariffType));

            return await productsService.GetProduct(tariffType, consumption);
        }
    }
}