using MobileOperatorClassLibrary.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobileOperatorClassLibrary.Interfaces
{
    public interface IOfferRepository
    {
        /// <summary>Сохраняет предложение в БД и возвращает его Id</summary>
        void Save(Offer offer);

        /// <summary>Возвращает все предложения по клиенту</summary>
        IEnumerable<Offer> GetByClient(int clientId);

        /// <summary>Возвращает все предложения</summary>
        IEnumerable<Offer> GetAll();
    }
}
