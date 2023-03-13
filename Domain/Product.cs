using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class Product : Entity
    {
        public string ProductName { get; set; }
        public decimal Price { get; set; }
        public virtual ICollection<Sale> Sales { get; set; }
    }
}
