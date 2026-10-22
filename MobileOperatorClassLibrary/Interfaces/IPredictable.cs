using MobileOperatorClassLibrary.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobileOperatorClassLibrary.Interfaces
{
    /// <summary>
    /// Интерфейс для прогнозирования. Определяет контракт расчета даты, когда баланс клиента достигнет нуля
    /// </summary>
    public interface IPredictable
    {
        /// <summary>
        /// Прогнозирует дату обнуления баланса клиента
        /// </summary>
        /// <param name="client">Клиент, для которого строится прогноз</param>
        /// <returns>Предполагаемая дата, когда баланс станет равным нулю</returns>
        DateTime PredictZeroDate(Client client);
    }
}
