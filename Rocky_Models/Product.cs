using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Rocky_Models
{
    public class Product
    {
        public Product() { DeliveryTime = 1; }
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }
        public string ShortDesc { get; set; }
        public string Description { get; set; }
        [Range(1, int.MaxValue)]
        public double Price { get; set; }
        public string Image { get; set; }

        [Display(Name = "Category Type")]
        public int CategoryId { get; set; }

        [ForeignKey("CategoryId")]
        public virtual Category Category { get; set; }

        [Display(Name = "Application Type")]
        public int ApplicationTypeId { get; set; }

        [ForeignKey("ApplicationTypeId")]
        public virtual ApplicationType ApplicationType { get; set; }

        [Range(1, 100)]
        public int DeliveryTime { get; set; }
        // Внешние ключи для связи с Warehouse
        [Display(Name = "Loading Warehouse")]
        public int LoadingWarehouseId { get; set; }
        [Display(Name = "Unloading Warehouse")]
        public int UnloadingWarehouseId { get; set; }

        // Навигационные свойства
        [ForeignKey("LoadingWarehouseId")]
        public virtual Warehouse LoadingWarehouse { get; set; }
        [ForeignKey("UnloadingWarehouseId")]
        public virtual Warehouse UnloadingWarehouse { get; set; }
    }
}
