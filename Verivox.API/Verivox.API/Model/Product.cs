using System;

namespace Verivox.API.Model
{
    /// <summary>
    /// The result product according to tariff
    /// </summary>
    public class Product
    {
        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="tariffName">Name of tariff</param>
        /// <param name="annualCosts">Consumption Costs per year in euro</param>
        public Product(string tariffName, double annualCosts)
        {
            if (annualCosts <= 0) throw new ArgumentOutOfRangeException(nameof(annualCosts));

            TariffName = tariffName ?? throw new ArgumentNullException(nameof(tariffName));
            AnnualCosts = annualCosts;
        }

        /// <summary>
        /// Name of tariff
        /// </summary>
        public string TariffName { get; }

        /// <summary>
        /// Consumption Costs per year in euro
        /// </summary>
        public double AnnualCosts { get; }
    }
}
