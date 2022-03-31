using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using DbCRUDRepos;
using PearlNecklace;

namespace PerlWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NecklacesController : ControllerBase
    {
        private INecklaceRepository _repo;
        private ILogger<NecklacesController> _logger;

        //GET: api/customers/?country={country}
        //Below are good practice decorators to use for a GET request
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Necklace>))]
        public async Task<IEnumerable<Necklace>> GetNecklaces(string neckParam)
        {
            _logger.LogInformation("GetCustomers initiated");
            if (string.IsNullOrWhiteSpace(neckParam))
            {
                var neck = await _repo.ReadAllAsync();

                _logger.LogInformation("GetCustomers returned {count} customers", neck.Count());
                return neck;
            }
            else
            {
                var neck = await _repo.ReadAllAsync();
                //neck = neck.Where(necklace => necklace.Country == neckParam);

                _logger.LogInformation("GetCustomers returned {count} customers in country {country}", neck.Count(), neckParam);
                return neck;
            }
        }

        //GET: api/customers/id
        [HttpGet("{custId}", Name = nameof(GetNecklace))]
        [ProducesResponseType(200, Type = typeof(Necklace))]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]

        public async Task<IEnumerable<Necklace>> GetNecklace(string neckId)
        {
            if (string.IsNullOrWhiteSpace(neckId))
            {
                var neck = await _repo.ReadAllAsyncWithPearls();
                return neck;
            }
            else
            {
                var list = await _repo.ReadAllAsyncWithPearls();
                return list.Where(neck => neck.NecklaceID.ToString() == neckId);
            }
        }
        //DELETE: api/customers/id
        [HttpDelete("{neckIdString}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> DeleteNecklace(string neckIdString)
        {

            if (!int.TryParse(neckIdString, out int neckId))
            {
                return BadRequest("Guid format error");
            }

            Necklace neck = await _repo.ReadAsync(neckId);
            if (neck == null)
            {
                return NotFound();
            }

            neck = await _repo.DeleteAsync(neckId);
            if (neck != null)
            {
                //_logger.LogInformation("Deleted necklace {neckId}", neckId);

                //Send an empty body response to confirm
                return new NoContentResult();
            }
            else
            {
                return BadRequest("Customer found but could not be deleted");
            }
        }


        //POST: api/customers
        //Body: Customer in Json
        //Note: ID has to be zero for necklace, auto creates correct ID
        [HttpPost]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> CreateNecklace([FromBody] Necklace neck)
        {
            if (neck == null)
            {
                return BadRequest("No Necklace");
            }
            if (await _repo.ReadAsync(neck.NecklaceID) != null)
            {
                return BadRequest("Necklace ID already existing");
            }

            neck = await _repo.CreateAsync(neck);
            if (neck != null)
            {
                //201 created ok with url details to read newlys created customer
                return CreatedAtRoute(

                    //Named Route in the HttpGet request
                    routeName: nameof(GetNecklace),

                    //custId is the parameter in HttpGet
                    routeValues: new { custId = neck.NecklaceID.ToString().ToLower() },

                    //Customer detail in the Body
                    value: neck);
            }
            else
            {
                return BadRequest("Could not create Necklace");
            }
        }
        public NecklacesController(INecklaceRepository repo, ILogger<NecklacesController> logger)
        {
            _repo = repo;
            AppLog.Instance.LogInformation("Controller started");
        }

    }
}
