using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobileOperatorClassLibrary.Interfaces
{
    /// <summary>
    /// Интерфейс для сущностей, которые могут быть сохранены в БД
    /// </summary>
    public interface IEntity
    {
        int Id { get; set; }
    }
}
