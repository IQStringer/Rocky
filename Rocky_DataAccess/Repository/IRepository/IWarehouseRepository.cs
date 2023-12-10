using Rocky_Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rocky_DataAccess.Repository.IRepository
{
    public interface IWarehouseRepository: IRepository<Warehouse>
    {
        void Update(Warehouse obj);
        /*
        // Получение всех складов
        IEnumerable<Warehouse> GetAll();

        // Получение склада по ID
        Warehouse Get(int id);

        // Добавление склада
        void Add(Warehouse warehouse);

        // Обновление данных склада
        void Save(Warehouse warehouse);

        // Удаление склада
        void Remove(int id);*/
    }
}
