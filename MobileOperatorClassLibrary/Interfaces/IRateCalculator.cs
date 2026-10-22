using MobileOperatorClassLibrary.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobileOperatorClassLibrary.Interfaces
{
    /// <summary>
    /// Интерфейс для расчета стоимости услуг
    /// </summary>
    public interface IRateCalculator
    {
        /// <summary>
        /// Метод расчета стоимости звонков
        /// </summary>
        /// <param name="seconds">Длительность звонка в секундах</param>
        /// <param name="tariff">Объект тарифа, по которому рассчитывается стоимость</param>
        /// <returns>Стоимость звонка в денежных единицах</returns>
        decimal CalculateCallCost(int seconds, Tariff tariff);
        /// <summary>
        /// Метод расчета стоимости использования интернета
        /// </summary>
        /// <param name="mb">Объем интернет-трафика в мегабайтах</param>
        /// <param name="tariff">Объект тарифа, по которому рассчитывается стоимость</param>
        /// <returns>Стоимость интернет-трафика в денежных единицах</returns>
        decimal CalculateInternetCost(double mb, Tariff tariff);
    }
}
