using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobileOperatorClassLibrary.Data.dtos
{
    public interface ITariffRow
    {
        int TariffId { get; }
        string TariffName { get; }
        decimal MonthlyFee { get; }
        decimal CallCostPerMinute { get; }
        decimal MbCost { get; }
        string TariffType { get; }
        decimal? CashbackPercent { get; }
        decimal? CreditLimit { get; }
        decimal? PenaltyPercent { get; }
    }
}
