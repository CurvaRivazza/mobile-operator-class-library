using Dapper;
using MobileOperatorClassLibrary.Data.dtos;
using MobileOperatorClassLibrary.Entities;
using MobileOperatorClassLibrary.Interfaces;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MobileOperatorClassLibrary.Data
{
    /// <summary>
    /// Реализация репозитория клиентов поверх PostgreSQL с использованием Dapper.
    /// </summary>
    public class ClientRepository : IClientRepository
    {
        private readonly string _connectionString;

        public ClientRepository(string connectionString)
        {
            _connectionString = connectionString
                ?? throw new ArgumentNullException(nameof(connectionString));
        }

        private NpgsqlConnection CreateConnection() => new NpgsqlConnection(_connectionString);
        /// <summary>
        /// Возвращает клиента по Id вместе с тарифом и подключёнными услугами.
        /// </summary>
        public Client GetById(int id)
        {
            using var conn = CreateConnection();
            conn.Open();

            const string clientSql = @"
                SELECT c.id, c.phone_number, c.full_name, c.balance,
                       t.id                  AS TariffId,
                       t.name                AS TariffName,
                       t.monthly_fee,
                       t.call_cost_per_minute,
                       t.mb_cost,
                       t.tariff_type,
                       t.cashback_percent,
                       t.credit_limit,
                       t.penalty_percent
                FROM clients c
                JOIN tariffs t ON t.id = c.tariff_id
                WHERE c.id = @Id";

            var row = conn.QueryFirstOrDefault<ClientRow>(clientSql, new { Id = id });
            if (row == null) return null;

            var client = MapClient(row);

            const string servicesSql = @"
                SELECT s.id, s.name, s.price, s.description
                FROM services s
                JOIN client_services cs ON cs.service_id = s.id
                WHERE cs.client_id = @Id";

            var services = conn.Query<Service>(servicesSql, new { Id = id }).ToList();
            foreach (var s in services)
                client.AddService(s);

            return client;
        }

        /// <summary>
        /// Возвращает всех клиентов (с тарифом, без услуг).
        /// </summary>
        public IEnumerable<Client> GetAll()
        {
            using var conn = CreateConnection();

            const string sql = @"
                SELECT c.id, c.phone_number, c.full_name, c.balance,
                       t.id                  AS TariffId,
                       t.name                AS TariffName,
                       t.monthly_fee,
                       t.call_cost_per_minute,
                       t.mb_cost,
                       t.tariff_type         AS TariffType,
                       t.cashback_percent,
                       t.credit_limit,
                       t.penalty_percent
                FROM clients c
                JOIN tariffs t ON t.id = c.tariff_id
                ORDER BY c.id";

            var rows = conn.Query<ClientRow>(sql);
            return rows.Select(MapClient).ToList();
        }

        /// <summary>
        /// Сохраняет клиента: INSERT (если Id = 0) или UPDATE.
        /// </summary>
        public void Save(Client client)
        {
            using var conn = CreateConnection();

            if (client.Id == 0)
            {
                const string insert = @"
                    INSERT INTO clients (phone_number, full_name, balance, tariff_id)
                    VALUES (@PhoneNumber, @FullName, @Balance, @TariffId)
                    RETURNING id";

                client.Id = conn.ExecuteScalar<int>(insert, new
                {
                    client.PhoneNumber,
                    client.FullName,
                    client.Balance,
                    TariffId = client.CurrentTariff.Id
                });
            }
            else
            {
                const string update = @"
                    UPDATE clients
                    SET phone_number = @PhoneNumber,
                        full_name    = @FullName,
                        balance      = @Balance,
                        tariff_id    = @TariffId
                    WHERE id = @Id";

                conn.Execute(update, new
                {
                    client.Id,
                    client.PhoneNumber,
                    client.FullName,
                    client.Balance,
                    TariffId = client.CurrentTariff.Id
                });
            }
        }


        /// <summary>
        /// Добавляет запись детализации о звонке в БД.
        /// </summary>
        public void AddCallDetail(CallDetail detail)
        {
            if (detail == null)
                throw new ArgumentNullException(nameof(detail));
            if (detail.ClientOwner == null)
                throw new InvalidOperationException("У записи детализации не указан клиент.");
            if (detail.UsedTariff == null)
                throw new InvalidOperationException("У записи детализации не указан тариф.");

            using var conn = CreateConnection();

            const string sql = @"
                INSERT INTO call_details
                    (client_id, tariff_id, date_time, duration_seconds,
                     destination_number, cost)
                VALUES (@ClientId, @TariffId, @DateTime, @DurationSeconds,
                        @DestinationNumber, @Cost)
                RETURNING id";

            detail.Id = conn.ExecuteScalar<int>(sql, new
            {
                ClientId = detail.ClientOwner.Id,
                TariffId = detail.UsedTariff.Id,
                detail.DateTime,
                detail.DurationSeconds,
                detail.DestinationNumber,
                detail.Cost
            });
        }

        /// <summary>
        /// Возвращает детализацию звонков клиента за период.
        /// Тариф тянется из БД через JOIN.
        /// </summary>
        public IEnumerable<CallDetail> GetDetailsByClient(int clientId, DateTime from, DateTime to)
        {
            using var conn = CreateConnection();

            const string sql = @"
                SELECT d.id,
                       d.date_time,
                       d.duration_seconds,
                       d.destination_number,
                       d.cost,
                       d.client_id,
                       t.id                  AS TariffId,
                       t.name                AS TariffName,
                       t.monthly_fee,
                       t.call_cost_per_minute,
                       t.mb_cost,
                       t.tariff_type,
                       t.cashback_percent,
                       t.credit_limit,
                       t.penalty_percent
                FROM call_details d
                JOIN tariffs t ON t.id = d.tariff_id
                WHERE d.client_id = @ClientId
                  AND d.date_time BETWEEN @From AND @To
                ORDER BY d.date_time DESC";

            var rows = conn.Query<CallDetailRow>(sql, new
            {
                ClientId = clientId,
                From = from,
                To = to
            });

            return rows.Select(MapCallDetail).ToList();
        }


        /// <summary>
        /// Маппит строку JOIN-запроса в объект <see cref="Client"/>.
        /// </summary>
        private Client MapClient(ClientRow row)
        {
            return new Client
            {
                Id = row.Id,
                PhoneNumber = row.PhoneNumber,
                FullName = row.FullName,
                Balance = row.Balance,
                CurrentTariff = MapTariff(row)
            };
        }

        /// <summary>
        /// Маппит строку JOIN-запроса (call_details + tariffs) в объект <see cref="CallDetail"/>.
        /// </summary>
        private CallDetail MapCallDetail(CallDetailRow row)
        {
            return new CallDetail
            {
                Id = row.Id,
                DateTime = row.DateTime,
                DurationSeconds = row.DurationSeconds,
                DestinationNumber = row.DestinationNumber,
                Cost = row.Cost,
                ClientOwner = new Client { Id = row.ClientId },
                UsedTariff = MapTariff(row)
            };
        }

        /// <summary>
        /// Маппит поля тарифа в конкретный класс-наследник по полю tariff_type.
        /// </summary>
        private Tariff MapTariff(ITariffRow row)
        {
            switch (row.TariffType)
            {
                case "prepaid":
                    return new PrepaidTariff
                    {
                        Id = row.TariffId,
                        Name = row.TariffName,
                        MonthlyFee = row.MonthlyFee,
                        CallCostPerMinute = row.CallCostPerMinute,
                        MbCost = row.MbCost,
                        CashbackPercent = row.CashbackPercent ?? 0m
                    };

                case "postpaid":
                    return new PostpaidTariff
                    {
                        Id = row.TariffId,
                        Name = row.TariffName,
                        MonthlyFee = row.MonthlyFee,
                        CallCostPerMinute = row.CallCostPerMinute,
                        MbCost = row.MbCost,
                        CreditLimit = row.CreditLimit ?? 0m,
                        PenaltyPercent = row.PenaltyPercent ?? 0m
                    };

                default:
                    throw new InvalidOperationException(
                        $"Неизвестный тип тарифа в БД: '{row.TariffType}'. " +
                        "Ожидается 'prepaid' или 'postpaid'.");
            }
        }
    }
}