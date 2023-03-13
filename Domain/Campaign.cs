using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class Campaign : Entity
    {
        public string Name { get; set; }
        public string? Description { get; set; }
        public DateTime StartDateTime { get; set; }
        public DateTime? EndDateDime { get; set; }
        public decimal Discount { get; set; }
        public virtual IEnumerable<CampaignCustomer> CampaignCustomers { get; set; }
        public virtual IEnumerable<Sale>? Sales { get; set; }
    }
}
