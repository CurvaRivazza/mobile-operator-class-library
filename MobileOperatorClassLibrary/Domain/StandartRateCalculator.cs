using MobileOperatorClassLibrary.Entities;
using MobileOperatorClassLibrary.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobileOperatorClassLibrary.Domain
{
    /// <summary>
    /// Стандартная стратегия расчёта стоимости услуг
    /// </summary>
    public class StandardRateCalculator : IRateCalculator
    {
        public decimal CalculateCallCost(int seconds, Tariff tariff)
        {
            if (tariff == null) throw new ArgumentNullException(nameof(tariff));

            // Округление вверх до минуты (61 сек = 2 минуты)
            int minutes = (int)Math.Ceiling(seconds / 60.0);
            return minutes * tariff.CallCostPerMinute;
        }

        public decimal CalculateInternetCost(double mb, Tariff tariff)
        {
            if (tariff == null) throw new ArgumentNullException(nameof(tariff));

            // Округление вверх до целого МБ
            double roundedMb = Math.Ceiling(mb);
            return (decimal)roundedMb * tariff.MbCost;
        }
    }
}
