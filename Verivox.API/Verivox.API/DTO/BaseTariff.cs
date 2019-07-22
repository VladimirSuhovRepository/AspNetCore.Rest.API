using System.ComponentModel.DataAnnotations;
using Verivox.API.Model;

namespace Verivox.API.DTO
{
    internal class BaseTariff
    {
        [Key]
        public int BaseTariffId { get; set; }

        public TariffType TariffType { get; set; }

        public string Name { get; set; }

        public float BaseCost { get; set; }

        public float ConsumptionCost { get; set; }
    }
}
