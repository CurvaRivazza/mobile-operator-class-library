using MobileOperatorClassLibrary.Entities;
using MobileOperatorClassLibrary.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobileOperatorClassLibrary.Domain
{
    /// <summary>
    /// Сервис прогнозирования
    /// </summary>
    public class BalancePredictor : IPredictable
    {
        private readonly IClientRepository _repository;
        public BalancePredictor(IClientRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        /// <summary>
        /// Прогнозирует дату обнуления баланса на основе анализа за 7 дней
        /// </summary>
        /// <param name="client">Клиент для прогноза</param>
        /// <returns>Дата, когда баланс станет нулевым</returns>
        public DateTime PredictZeroDate(Client client) => PredictZeroDate(client, 7);

        /// <summary>
        /// Прогнозирует дату обнуления баланса на основе анализа расходов за указанное количество дней
        /// Для постоплатных тарифов возвращает дату следующего расчётного периода
        /// </summary>
        /// <param name="client">Клиент для прогноза</param>
        /// <param name="daysForAnalyze">Глубина анализа истории</param>
        /// <returns>Дата, когда баланс станет нулевым</returns>
        public DateTime PredictZeroDate(Client client, int daysForAnalyze)
        {
            if (client == null) throw new ArgumentNullException(nameof(client));

            // Для постоплатных тарифов баланс не обнуляется
            // клиент платит по факту, поэтому возвращаем дату списания
            if (client.CurrentTariff is PostpaidTariff)
            {
                return DateTime.Now.AddDays(30);
            }

            // Получаем детализацию за указанный период
            var from = DateTime.Now.AddDays(-daysForAnalyze);
            var details = _repository
                .GetDetailsByClient(client.Id, from, DateTime.Now)
                .ToList();

            if (details.Count == 0)
            {
                // если нет истории, считаем, что баланса хватит на 30 дней
                return DateTime.Now.AddDays(30);
            }

            // Средний расход в день
            decimal totalCost = details.Sum(d => d.Cost);
            decimal avgDailyCost = totalCost / daysForAnalyze;

            if (avgDailyCost <= 0)
                return DateTime.Now.AddDays(30);

            // Сколько дней осталось до нуля
            int daysLeft = (int)Math.Floor(client.Balance / avgDailyCost);

            // Учитываем абонентскую плату и услуги
            decimal monthlyExpenses = client.CurrentTariff.GetMonthlyFee()
                                    + client.GetTotalServicesCost();
            decimal dailyExtra = monthlyExpenses / 30m;
            if (dailyExtra > 0)
            {
                decimal totalDailyCost = avgDailyCost + dailyExtra;
                daysLeft = (int)Math.Floor(client.Balance / totalDailyCost);
            }

            return DateTime.Now.AddDays(Math.Max(daysLeft, 0));
        }
    }
}
