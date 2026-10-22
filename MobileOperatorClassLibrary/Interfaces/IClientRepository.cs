using MobileOperatorClassLibrary.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobileOperatorClassLibrary.Interfaces
{
    /// <summary>
    /// Репозиторий для работы с клиентами и их детализацией
    /// </summary>
    public interface IClientRepository
    {
        /// <summary>Возвращает клиента по id</summary>
        Client GetById(int id);
        /// <summary>Возвращает всех клиентов</summary>
        IEnumerable<Client> GetAll();
        /// <summary>Сохраняет клиента (вставка или обновление)</summary>
        void Save(Client client);
        /// <summary>Добавляет запись детализации</summary>
        void AddCallDetail(CallDetail detail);
        /// <summary>Возвращает детализацию клиента за период</summary>
        IEnumerable<CallDetail> GetDetailsByClient(int clientId, DateTime from, DateTime to);
    }
}
