using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobileOperatorClassLibrary.Entities
{
    /// <summary>
    /// Запись детализации об использовании интернет-трафика
    /// </summary>
    public class InternetDetail : Detail
    {
        public double Mb { get; set; }

        /// <summary>
        /// Возвращает описание интернет-сессии
        /// </summary>
        public override string GetDescription()
            => $"Интернет, {Mb:F2} МБ, {Cost:C}";
    }
}
