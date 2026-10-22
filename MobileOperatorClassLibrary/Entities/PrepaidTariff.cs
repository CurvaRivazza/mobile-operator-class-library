using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobileOperatorClassLibrary.Entities
{
    /// <summary>
    /// Тариф с предоплатой
    /// </summary>
    public class PrepaidTariff : Tariff
    {
        /// <summary>
        /// Кешбэк за пополнение баланса
        /// </summary>
        public decimal CashbackPercent { get; set; }

        /// <summary>
        /// Переопределение метода расчета штрафа, в связи с тем, что для предоплатных тарифов штраф за перерасход не начисляется
        /// </summary>
        /// <param name="overspent">Сумма перерасхода</param>
        /// <returns>Сумма штрафа (всегда 0)</returns>
        public override decimal CalculatePenalty(decimal overspent)
        {
            return 0;
        }

        /// <summary>
        /// Метод для получения абонентской платы с учетом скидки
        /// Если кешбэк превышает 5%, то абонентская плата снижается на 10%
        /// </summary>
        /// <returns>Итоговая абонентская плата</returns>
        public override decimal GetMonthlyFee()
        {
            if (CashbackPercent > 5)
            {
                return base.GetMonthlyFee() * 0.9m;  // Уменьшение абонентской платы в случае, если кешбэк превышает 5%
            }

            return base.GetMonthlyFee();
        }

        public override string ToString() => $"[Предоплатный] {Name}";
    }
}
