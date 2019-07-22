using System;
using System.Net;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
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
            string expectedTariffName)
        {
            var client = factory.CreateClient();
            var response = await client.GetAsync(new Uri(client.BaseAddress, $"api/products/product/{TariffType.Base}/{consumption}")).ConfigureAwait(false);

            var product = JObject.Parse(await response.Content.ReadAsStringAsync().ConfigureAwait(false));

            product["annualCosts"].Value<long>().Should().Be(expectedAnnualCosts);
            product["tariffName"].Value<string>().Should().Be(expectedTariffName);
        }

        [Theory]
        [InlineData(3500, 800, PackagedTariffName)]
        [InlineData(4500, 950, PackagedTariffName)]
        [InlineData(6000, 1400, PackagedTariffName)]
        public async Task ProductsControllerGetPackagedProductWithDifferentConsumptionShouldReturnCorrectProduct(long consumption,
            long expectedAnnualCosts,
            string expectedTariffName)
        {
            var client = factory.CreateClient();
            var response = await client.GetAsync(new Uri(client.BaseAddress, $"api/products/product/{TariffType.Packaged}/{consumption}")).ConfigureAwait(false);

            var product = JObject.Parse(await response.Content.ReadAsStringAsync().ConfigureAwait(false));

            product["annualCosts"].Value<long>().Should().Be(expectedAnnualCosts);
            product["tariffName"].Value<string>().Should().Be(expectedTariffName);
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
            var response = await client.GetAsync(new Uri(client.BaseAddress, $"api/products/{consumption}")).ConfigureAwait(false);

            var products = JArray.Parse(await response.Content.ReadAsStringAsync().ConfigureAwait(false));

            products[0]["annualCosts"].Value<long>().Should().Be(expectedAnnualCostsFirst);
            products[1]["annualCosts"].Value<long>().Should().Be(expectedAnnualCostsSecond);
            products[0]["tariffName"].Value<string>().Should().Be(expectedTariffNameFirst);
            products[1]["tariffName"].Value<string>().Should().Be(expectedTariffNameSecond);
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(0)]
        public async Task ProductsControllerGetAllProductWithIncorrectConsumptionShouldReturnBadRequest(long consumption)
        {
            var client = factory.CreateClient();
            var response = await client.GetAsync(new Uri(client.BaseAddress, $"api/products/{consumption}")).ConfigureAwait(false);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Theory]
        [InlineData("test")]
        public async Task ProductsControllerGetAllProductWithStringConsumptionShouldReturnBadRequest(string consumption)
        {
            var client = factory.CreateClient();
            var response = await client.GetAsync(new Uri(client.BaseAddress, $"api/products/{consumption}")).ConfigureAwait(false);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }
    }
}
