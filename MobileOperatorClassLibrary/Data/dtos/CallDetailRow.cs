using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobileOperatorClassLibrary.Data.dtos
{
    public class CallDetailRow : ITariffRow
    {
        public int Id { get; set; }
        public DateTime DateTime { get; set; }
        public int DurationSeconds { get; set; }
        public string DestinationNumber { get; set; }
        public decimal Cost { get; set; }
        public int ClientId { get; set; }

        public int TariffId { get; set; }
        public string TariffName { get; set; }
        public decimal MonthlyFee { get; set; }
        public decimal CallCostPerMinute { get; set; }
        public decimal MbCost { get; set; }
        public string TariffType { get; set; }
        public decimal? CashbackPercent { get; set; }
        public decimal? CreditLimit { get; set; }
        public decimal? PenaltyPercent { get; set; }
    }
}
