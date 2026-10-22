using MobileOperatorClassLibrary.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobileOperatorClassLibrary.Entities
{
    /// <summary>
    /// Запись 
    /// </summary>
    public class CallDetail : Detail
    {
        public int Id { get; set; }
        public DateTime DateTime { get; set; }
        public int DurationSeconds { get; set; }
        public string DestinationNumber { get; set; }
        public decimal Cost { get; set; }
        public Client ClientOwner { get; set; }
        public Tariff UsedTariff { get; set; }

        /// <summary>
        /// Возвращает описание звонка
        /// </summary>
        /// <returns>Описание звонка</returns>
        public override string GetDescription() => $"Звонок на {DestinationNumber}, {DurationSeconds} сек., {Cost:C}";
    }
}
