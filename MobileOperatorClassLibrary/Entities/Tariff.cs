using MobileOperatorClassLibrary.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobileOperatorClassLibrary.Entities
{
    /// <summary>
    /// Базовый класс тарифа
    /// </summary>
    public abstract class Tariff : IEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal MonthlyFee { get; set; }
        public decimal CallCostPerMinute { get; set; }
        public decimal MbCost { get; set; }

        /// <summary>
        /// геттер для получения итоговой абонентской платы
        /// </summary>
        /// <returns>размер абонентской платы</returns>
        public virtual decimal GetMonthlyFee() => MonthlyFee;

        /// <summary>
        /// Метод для расчета штрафов, если баланс ушел в минус
        /// </summary>
        /// <param name="overspent"></param>
        /// <returns>Размер штрафа в денежных единицах</returns>
        public abstract decimal CalculatePenalty(decimal overspent);
    }
}
