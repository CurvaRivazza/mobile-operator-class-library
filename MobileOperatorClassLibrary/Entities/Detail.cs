using MobileOperatorClassLibrary.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobileOperatorClassLibrary.Entities
{
    /// <summary>
    /// Базовый класс для записей детализации расходов
    /// </summary>
    public abstract class Detail : IEntity
    {
        public int Id { get; set; }

        public DateTime DateTime { get; set; }

        public decimal Cost { get; set; }

        public Client ClientOwner { get; set; }

        public Tariff UsedTariff { get; set; }

        /// <summary>
        /// Возвращает краткое описание записи детализации
        /// </summary>
        public abstract string GetDescription();
    }
}
