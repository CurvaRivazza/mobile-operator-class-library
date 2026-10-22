using MobileOperatorClassLibrary.Entities;
using MobileOperatorClassLibrary.EventArguments;
using MobileOperatorClassLibrary.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobileOperatorClassLibrary.Domain
{
    /// <summary>
    /// Генератор предложений для рассылки
    /// </summary>
    public class OfferGenerator
    {
        private readonly IOfferRepository _offerRepository;
        public List<Offer> GeneratedOffers { get; private set; } = new List<Offer>();

        /// <summary>
        /// Создает генератор без сохранения в БД
        /// </summary>
        public OfferGenerator() { }

        public OfferGenerator(IOfferRepository offerRepository)
        {
            _offerRepository = offerRepository;
        }


        /// <summary>
        /// Обработчик события «баланс на исходе»
        /// Генерирует предложение и сохраняет его в коллекцию
        /// </summary>
        /// <param name="sender">Источник события (клиент)</param>
        /// <param name="e">Аргументы события</param>
        public void OnClientBalanceLow(object sender, BalanceEventArgs e)
        {
            if (sender is not Client client) return;

            string text = GenerateOffer(client);
            SaveOffer(new Offer
            {
                ClientId = client.Id,
                Text = text,
                CreatedAt = DateTime.Now,
                Type = OfferType.LowBalance
            });
        }

        /// <summary>
        /// Обработчик события «баланс обнулён»
        /// Генерирует срочное предложение о пополнении
        /// </summary>
        public void OnClientBalanceZero(object sender, BalanceEventArgs e)
        {
            if (sender is not Client client) return;

            string text = GenerateOffer(client, 300m);
            SaveOffer(new Offer
            {
                ClientId = client.Id,
                Text = text,
                CreatedAt = DateTime.Now,
                Type = OfferType.ZeroBalance
            });
        }

        private void SaveOffer(Offer offer)
        {
            GeneratedOffers.Add(offer);
            _offerRepository?.Save(offer);
        }

        /// <summary>
        /// Генерирует стандартное предложение для клиента в зависимости от типа его тарифа
        /// </summary>
        /// <param name="client">Клиент</param>
        /// <returns>Текст предложения</returns>
        public string GenerateOffer(Client client)
        {
            if (client.CurrentTariff is PrepaidTariff)
            {
                return $"Пополните баланс сейчас и получите +10% к сумме! Ваш текущий баланс: {client.Balance:C}";
            }

            return "Перейдите на новый тариф с большим пакетом минут!";
        }

        /// <summary>
        /// Генерирует предложение с индивидуальным порогом пополнения
        /// </summary>
        /// <param name="client">Клиент</param>
        /// <param name="customBonus">Пороговая сумма пополнения</param>
        /// <returns>Текст предложения</returns>
        public string GenerateOffer(Client client, decimal customBonus)
        {
            return $"Пополните на сумму более {customBonus:C} и получите скидку на следующий месяц!";
        }
    }
}
