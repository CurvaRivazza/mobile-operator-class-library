using Dapper;
using MobileOperatorClassLibrary.Data.dtos;
using MobileOperatorClassLibrary.Entities;
using MobileOperatorClassLibrary.Interfaces;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobileOperatorClassLibrary.Data
{
    /// <summary>
    /// Реализация репозитория предложений поверх PostgreSQL
    /// </summary>
    public class OfferRepository : IOfferRepository
    {
        private readonly string _connectionString;

        public OfferRepository(string connectionString)
        {
            _connectionString = connectionString
                ?? throw new ArgumentNullException(nameof(connectionString));
        }

        private NpgsqlConnection CreateConnection() => new NpgsqlConnection(_connectionString);

        /// <inheritdoc/>
        public void Save(Offer offer)
        {
            if (offer == null) throw new ArgumentNullException(nameof(offer));

            using var conn = CreateConnection();

            const string sql = @"
                INSERT INTO offers (client_id, text, created_at, offer_type)
                VALUES (@ClientId, @Text, @CreatedAt, @OfferType)
                RETURNING id";

            offer.Id = conn.ExecuteScalar<int>(sql, new
            {
                offer.ClientId,
                offer.Text,
                offer.CreatedAt,
                OfferType = offer.Type.ToString()
            });
        }

        /// <inheritdoc/>
        public IEnumerable<Offer> GetByClient(int clientId)
        {
            using var conn = CreateConnection();

            const string sql = @"
                SELECT id, client_id, text, created_at, offer_type
                FROM offers
                WHERE client_id = @ClientId
                ORDER BY created_at DESC";

            var rows = conn.Query<OfferRow>(sql, new { ClientId = clientId });
            return rows.Select(MapOffer).ToList();
        }

        /// <inheritdoc/>
        public IEnumerable<Offer> GetAll()
        {
            using var conn = CreateConnection();

            const string sql = @"
                SELECT id, client_id, text, created_at, offer_type
                FROM offers
                ORDER BY created_at DESC";

            var rows = conn.Query<OfferRow>(sql);
            return rows.Select(MapOffer).ToList();
        }

        private Offer MapOffer(OfferRow row) => new Offer
        {
            Id = row.Id,
            ClientId = row.ClientId,
            Text = row.Text,
            CreatedAt = row.CreatedAt,
            Type = Enum.TryParse<OfferType>(row.OfferType, out var t) ? t : OfferType.Standard
        };
    }
}
