using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class CampaignCustomer : Entity
    {
        public int CampaignId { get; set; }
        public virtual Campaign Campaign { get; set; }
        public int CustomerId { get; set; }
        public virtual Customer Customer { get; set; }
        public int EmployeeId { get; set; }
        public virtual Employee Employee { get; set; }
        public DateTime CreateadAt { get; set; }
    }
}
