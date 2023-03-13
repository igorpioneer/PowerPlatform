using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DataTransfer
{
    public class CampaignCustomerDto
    {
        public int CampaignId { get; set; }
        public int CustomerId { get; set; }
        public int EmployeeId { get; set; }
        public DateTime CreateadAt { get; set; } = DateTime.Now;
    }
}
