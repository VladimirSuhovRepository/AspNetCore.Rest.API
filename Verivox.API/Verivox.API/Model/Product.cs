using System;

namespace Verivox.API.Model
{
    /// <summary>
    /// The result product according to tariff.
    /// </summary>
    public class Product
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Product"/> class.
        /// </summary>
        /// <param name="tariffName">Name of tariff.</param>
        /// <param name="annualCosts">Consumption Costs per year in euro (€/year).</param>
        public Product(string tariffName, double annualCosts)
        {
            if (annualCosts <= 0) throw new ArgumentOutOfRangeException(nameof(annualCosts));

            this.TariffName = tariffName ?? throw new ArgumentNullException(nameof(tariffName));
            this.AnnualCosts = annualCosts;
        }

        /// <summary>
        /// Gets Name of tariff.
        /// </summary>
        public string TariffName { get; }

        /// <summary>
        /// Gets Consumption Costs per year in euro (€/year).
        /// </summary>
        public double AnnualCosts { get; }
    }
}
