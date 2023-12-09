using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections;
using System.Collections.Generic;

namespace Rocky_Models.ViewModels
{
    public class ProductVM
    {
        public Product Product { get; set; }
        public IEnumerable<SelectListItem> CategorySelectedList { get; set; }
        public IEnumerable<SelectListItem> ApplicationTypeSelectList { get; set; }
        public IEnumerable<SelectListItem> WarehouseSelectList { get; set; }
    }
}
