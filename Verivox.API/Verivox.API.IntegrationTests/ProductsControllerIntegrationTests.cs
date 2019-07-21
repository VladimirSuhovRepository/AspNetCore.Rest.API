using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Verivox.API.Model;
using Xunit;

namespace Verivox.API.IntegrationTests
{
    public class ProductsControllerIntegrationTests : IClassFixture<WebApplicationFactory<Startup>>
    {
        private const string BaseTariffName = "basic electricity tariff";
        private const string PackagedTariffName = "Packaged  tariff";
        private readonly WebApplicationFactory<Startup> factory;

        public ProductsControllerIntegrationTests(WebApplicationFactory<Startup> factory)
        {
            this.factory = factory;
        }

        [Theory]
        [InlineData(3500, 830, BaseTariffName)]
        [InlineData(4500, 1050, BaseTariffName)]
        [InlineData(6000, 1380, BaseTariffName)]
        public async Task ProductsControllerGetBaseProductWithDifferentConsumptionShouldReturnCorrectProduct(long consumption, 
            long expectedAnnualCosts,
            string tariffName)
        {
            var client = factory.CreateClient();
            var response = await client.GetAsync($"api/products/product/{TariffType.Base}/{consumption}");

            var product = JObject.Parse(await response.Content.ReadAsStringAsync());

            Assert.Equal(expectedAnnualCosts, product["annualCosts"].Value<long>());
            Assert.Equal(tariffName, product["tariffName"].Value<string>());
        }

        [Theory]
        [InlineData(3500, 800, PackagedTariffName)]
        [InlineData(4500, 950, PackagedTariffName)]
        [InlineData(6000, 1400, PackagedTariffName)]
        public async Task ProductsControllerGetPackagedProductWithDifferentConsumptionShouldReturnCorrectProduct(long consumption,
            long expectedAnnualCosts,
            string tariffName)
        {
            var client = factory.CreateClient();
            var response = await client.GetAsync($"api/products/product/{TariffType.Packaged}/{consumption}");

            var product = JObject.Parse(await response.Content.ReadAsStringAsync());

            Assert.Equal(expectedAnnualCosts, product["annualCosts"].Value<long>());
            Assert.Equal(tariffName, product["tariffName"].Value<string>());
        }

        [Theory]
        [InlineData(3500, 800, 830, PackagedTariffName, BaseTariffName)]
        [InlineData(4500, 950, 1050, PackagedTariffName, BaseTariffName)]
        [InlineData(6000, 1380, 1400, BaseTariffName, PackagedTariffName)]
        public async Task ProductsControllerGetAllProductWithDifferentConsumptionShouldBeSortedCorrectly(long consumption,
            long expectedAnnualCostsFirst,
            long expectedAnnualCostsSecond,
            string expectedTariffNameFirst,
            string expectedTariffNameSecond)
        {
            var client = factory.CreateClient();
            var response = await client.GetAsync($"api/products/{consumption}");

            var products = JArray.Parse(await response.Content.ReadAsStringAsync());

            Assert.Equal(expectedAnnualCostsFirst, products[0]["annualCosts"].Value<long>());
            Assert.Equal(expectedAnnualCostsSecond, products[1]["annualCosts"].Value<long>());
            Assert.Equal(expectedTariffNameFirst, products[0]["tariffName"].Value<string>());
            Assert.Equal(expectedTariffNameSecond, products[1]["tariffName"].Value<string>());
        }
    }
}
