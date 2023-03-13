using Application;
using Application.Commands;
using Application.DataTransfer;
using Application.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Api.Controllers
{ 
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CampaignCustomerController : ControllerBase
    {
        private readonly IApplicationActor actor;
        private readonly IGetCampaignsQuery query;

        public CampaignCustomerController(IApplicationActor actor, IGetCampaignsQuery query)
        {
            this.actor = actor;
            this.query = query;
        }

        // GET: api/<CampaignCustomerController>
        [HttpGet]
        public IActionResult Get([FromServices] IGetCampaignsQuery query)
        {
            return Ok(query.Execute());
        }

        // POST api/<CampaignCustomerController>
        [HttpPost]
        public IActionResult Post([FromBody] CampaignCustomerDto dto,
            [FromServices] ICreateCampaignCustomerCommand command)
        {
            dto.EmployeeId = actor.Id;
            command.Execute(dto);
            return StatusCode(200, "You added customer to the campaign successfully!");
        }
    }
}
