using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class Sale : Entity
    {
        public int ProductId { get; set; }
        public virtual Product Product { get; set; }
        public int CustomerId { get; set; }
        public virtual Customer Customer { get; set; }
        public DateTime CreatedAt { get; set; }
        public int? CampaignId { get; set; }
        public virtual Campaign? Campaign { get; set; }
    }
}
