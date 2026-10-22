using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobileOperatorClassLibrary.Entities
{
    /// <summary>
    /// Тариф с постоплатой
    /// </summary>
    public class PostpaidTariff : Tariff
    {
        public decimal CreditLimit { get; set; }
        public decimal PenaltyPercent { get; set; }

        /// <summary>
        /// метод расчета штрафа за перерасход
        /// </summary>
        /// <param name="overspent">Сумма перерасхода</param>
        /// <returns>Сумма штрафа</returns>
        public override decimal CalculatePenalty(decimal overspent)
        {
            return overspent * (PenaltyPercent / 100);
        }

        /// <summary>
        /// Перегруженный метод расчета штрафа с учетом лояльности клиента
        /// </summary>
        /// <param name="overspent">Сумма перерасхода</param>
        /// <param name="isLoyalClient">Является ли клиент лояльным</param>
        /// <returns>Размер штрафа</returns>
        public decimal CalculatePenalty(decimal overspent, bool isLoyalClient)
        {
            if (isLoyalClient)
            {
                return CalculatePenalty(overspent) * 0.5m;
            }

            return CalculatePenalty(overspent);
        }

        public override string ToString() => $"[Постоплатный] {Name}";
    }
}
