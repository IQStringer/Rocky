using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Rocky_DataAccess;
using Rocky_DataAccess.Repository.IRepository;
using Rocky_Models;
using Rocky_Utility;
using System.Collections;
using System.Collections.Generic;

namespace Rocky.Controllers
{
    [Authorize(Roles = (WC.AdminRole))]
    public class WarehouseController : Controller
    {
        private readonly IProductRepository _proRepo;
        private readonly IWarehouseRepository _warRepo;
        public WarehouseController(IWarehouseRepository warRepo, IProductRepository proRepo)
        {
            _warRepo = warRepo;
            _proRepo = proRepo;
        }
        public IActionResult Index()
        {
            IEnumerable<Warehouse> objList = _warRepo.GetAll();
            return View(objList);
        }
        public IActionResult Create() 
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Warehouse obj)
        {
            _warRepo.Add(obj);
            _warRepo.Save();
            return RedirectToAction("Index");
        }
        //Get для редактирования
        public IActionResult Edit(int id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var obj = _warRepo.Find(id);
            if (obj == null)
            {
                return NotFound();
            }
            return View(obj);
        }
        //Post для edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Warehouse obj)
        {
            if (ModelState.IsValid)
            {
                _warRepo.Update(obj);
                _warRepo.Save();
                return RedirectToAction("Index");
            }
            return View(obj);

        }
        //Get для удаления
        public IActionResult Delete(int id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var obj = _warRepo.Find(id);
            if (obj == null)
            {
                return NotFound();
            }
            return View(obj);
        }
        //Post для удаления
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePost(int? id)
        {
            var obj = _warRepo.Find(id.GetValueOrDefault());
            if (obj == null)
            {
                return NotFound();
            }
            //Каскадка
            IEnumerable<Product> list = _proRepo.GetAll(a => a.LoadingWarehouse == obj || a.UnloadingWarehouse == obj);
            foreach (Product product in list) 
            {
                product.UnloadingWarehouse = product.UnloadingWarehouse == obj ? null : product.UnloadingWarehouse;
                product.LoadingWarehouse = product.LoadingWarehouse == obj ? null : product.LoadingWarehouse;

            }
            _proRepo.Save();
            _warRepo.Remove(obj);
            _warRepo.Save();
            return RedirectToAction("Index");
        }
    }
}
