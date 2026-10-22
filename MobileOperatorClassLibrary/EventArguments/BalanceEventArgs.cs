using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobileOperatorClassLibrary.EventArguments
{
    /// <summary>
    /// Аргументы для событий
    /// </summary>
    public class BalanceEventArgs : EventArgs
    {
        public decimal CurrentBalance { get; set; }
        public string Message { get; set; }
    }
}
