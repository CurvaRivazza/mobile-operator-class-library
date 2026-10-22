using MobileOperatorClassLibrary.EventArguments;
using MobileOperatorClassLibrary.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobileOperatorClassLibrary.Entities
{
    /// <summary>
    /// Класс клиента оператора мобильной связи
    /// </summary>
    public class Client : IEntity
    {
        public int Id { get; set; }
        public string PhoneNumber { get; set; }
        public string FullName { get; set; }
        public decimal Balance { get; set; }
        public Tariff CurrentTariff { get; set; }
        public List<Service> SubscribedServices { get; private set; }

        public event EventHandler<BalanceEventArgs> OnBalanceLow;
        public event EventHandler<BalanceEventArgs> OnBalanceZero;
        public event EventHandler<BalanceEventArgs> OnBalanceChanged;

        public Client()
        {
            SubscribedServices = new List<Service>();
        }

        /// <summary>
        /// Пополняет баланс клиента на указанную сумму
        /// </summary>
        /// <param name="amount">Сумма пополнения</param>
        public void TopUp(decimal amount)
        {
            Balance += amount;
            OnBalanceChanged?.Invoke(this, new BalanceEventArgs { CurrentBalance = Balance });
            CheckBalanceEvents();
        }

        /// <summary>
        /// Списывает с баланса клиента указанную сумму
        /// </summary>
        /// <param name="amount">Сумма списания</param>
        public void Debit(decimal amount)
        {
            Balance -= amount;
            OnBalanceChanged?.Invoke(this, new BalanceEventArgs { CurrentBalance = Balance });
            CheckBalanceEvents();
        }

        /// <summary>
        /// Подключает клиенту дополнительную услугу.
        /// </summary>
        /// <param name="service">Услуга для подключения.</param>
        public void AddService(Service service)
        {
            if (service == null) throw new ArgumentNullException(nameof(service));
            if (SubscribedServices.Any(s => s.Id == service.Id)) return;
            SubscribedServices.Add(service);
        }

        /// <summary>
        /// Отключает услугу по её идентификатору.
        /// </summary>
        /// <param name="serviceId">Идентификатор услуги.</param>
        public void RemoveService(int serviceId)
        {
            var service = SubscribedServices.FirstOrDefault(s => s.Id == serviceId);
            if (service != null) SubscribedServices.Remove(service);
        }

        /// <summary>
        /// Возвращает суммарную стоимость всех подключённых услуг.
        /// </summary>
        public decimal GetTotalServicesCost() => SubscribedServices.Sum(s => s.Price);


        /// <summary>
        /// Метод проверки баланса и генерации события OnBalanceZero / OnBalanceLow
        /// </summary>
        private void CheckBalanceEvents()
        {
            if (Balance <= 0)
            {
                OnBalanceZero?.Invoke(this, new BalanceEventArgs { CurrentBalance = Balance, Message = "Баланс обнулен" });
            }
            else if (Balance < 50)
            {
                OnBalanceLow?.Invoke(this, new BalanceEventArgs { CurrentBalance = Balance, Message = "Баланс на исходе!" });
            }
        }
    }
}
