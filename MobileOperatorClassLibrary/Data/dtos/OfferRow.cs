using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobileOperatorClassLibrary.Data.dtos
{
    public class OfferRow
    {
        public int Id { get; set; }
        public int ClientId { get; set; }
        public string Text { get; set; }
        public DateTime CreatedAt { get; set; }
        public string OfferType { get; set; }
    }
}
