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

    
    public void Update(Warehouse warehouse)
    {
        _db.Set<Warehouse>().Update(warehouse);
    }
}
}
