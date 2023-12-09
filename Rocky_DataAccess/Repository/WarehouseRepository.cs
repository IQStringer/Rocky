using Rocky_DataAccess;
using Rocky_DataAccess.Repository;
using Rocky_DataAccess.Repository.IRepository;
using Rocky_Models;
using System.Collections.Generic;
using System.Linq;

namespace Rocky_DataAccess.Repository
{
    public class WarehouseRepository: Repository<Warehouse>, IWarehouseRepository
    {
    private readonly ApplicationDBContext _db;

    public WarehouseRepository(ApplicationDBContext db): base(db)
    { 
        _db = db;
    }

    // Получение всех складов
    public IEnumerable<Warehouse> GetAll()
    {
        return _db.Set<Warehouse>().ToList();
    }

    // Получение склада по ID
    public Warehouse Get(int id)
    {
        return _db.Set<Warehouse>().FirstOrDefault(w => w.WarehouseId == id);
    }

    // Добавление склада
    public void Add(Warehouse warehouse)
    {
        _db.Set<Warehouse>().Add(warehouse);
        _db.SaveChanges();
    }

    // Обновление данных склада
    public void Update(Warehouse warehouse)
    {
        _db.Set<Warehouse>().Update(warehouse);
        _db.SaveChanges();
    }

    // Удаление склада
    public void Remove(int id)
    {
        Warehouse warehouse = _db.Set<Warehouse>().FirstOrDefault(w => w.WarehouseId == id);
        if (warehouse != null)
        {
            _db.Set<Warehouse>().Remove(warehouse);
            _db.SaveChanges();
        }
    }
}
}
