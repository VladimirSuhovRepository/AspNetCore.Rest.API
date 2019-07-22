using System.ComponentModel.DataAnnotations;
using Verivox.API.Model;

namespace Verivox.API.DTO
{
    internal class PackagedTariff
    {
        [Key]
        public int PackagedTariffId { get; set; }

        public TariffType TariffType { get; set; }

        public string Name { get; set; }

        public long IncludedConsumptionLevel { get; set; }

        public float PackageCost { get; set; }

        public float ConsumptionCost { get; set; }
    }
}
