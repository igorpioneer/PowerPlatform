using Application.DataTransfer;
using Application.Queries;
using DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Implementation.Queries
{
    public class GetCampaignsQuery : IGetCampaignsQuery
    {
        private readonly PowerPlatformTaskContext _context;

        public GetCampaignsQuery(PowerPlatformTaskContext context)
        {
            _context = context;
        }

        public IEnumerable<CampaignDto> Execute()
        {
            var campaigns = _context.Campaigns.Select(c => new CampaignDto
            {
                CampaignId = c.Id,
                Name = c.Name
            }).ToList();

            return campaigns;
        }
    }
}
