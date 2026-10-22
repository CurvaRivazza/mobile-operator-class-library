using MobileOperatorClassLibrary.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobileOperatorClassLibrary.Interfaces
{
    /// <summary>
    /// Репозиторий для работы с тарифами
    /// </summary>
    public interface ITariffRepository
    {
        /// <summary>Возвращает тариф по идентификатору</summary>
        Tariff GetById(int id);

        /// <summary>Возвращает все доступные тарифы</summary>
        IEnumerable<Tariff> GetAll();

        /// <summary>Сохраняет тариф (вставка или обновление)</summary>
        void Save(Tariff tariff);

        /// <summary>Удаляет тариф по идентификатору</summary>
        void Delete(int id);
    }
}
