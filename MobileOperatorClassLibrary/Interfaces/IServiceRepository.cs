using MobileOperatorClassLibrary.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobileOperatorClassLibrary.Interfaces
{
    public interface IServiceRepository
    {
        /// <summary>Возвращает услугу по id</summary>
        Service GetById(int id);
        /// <summary>Возвращает все услуги</summary>
        IEnumerable<Service> GetAll();
        /// <summary>Сохраняет услугу (вставка или обновление)</summary>
        void Save(Service client);
        /// <summary> Удаляет услугу </summary>
        void Delete(Service service);
    }
}
