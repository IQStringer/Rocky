using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rocky_Models
{
    public class Warehouse
    {
        public int Id { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Number { get; set; }
        public string ZipCode { get; set; }
        [NotMapped]
        public string Name{ get { return $"{City}, {Street}, {Number}, {ZipCode}"; } }
    }

}
