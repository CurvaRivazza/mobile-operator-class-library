using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobileOperatorClassLibrary.Entities
{
    /// <summary>
    /// Запись детализации об отправке SMS
    /// </summary>
    public class SmsDetail : Detail
    {
        public string DestinationNumber { get; set; }

        /// <summary>
        /// Возвращает описание SMS
        /// </summary>
        public override string GetDescription()
            => $"SMS на {DestinationNumber}, {Cost:C}";
    }
}
