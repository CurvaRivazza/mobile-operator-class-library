using MobileOperatorClassLibrary.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobileOperatorClassLibrary.Entities
{
    public class Offer : IEntity
    {
        public int Id { get; set; }

        public int ClientId { get; set; }

        public string Text { get; set; }

        public DateTime CreatedAt { get; set; }

        public OfferType Type { get; set; }
    }
}
