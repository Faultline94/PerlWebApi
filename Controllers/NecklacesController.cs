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

        //GET: api/necklaces/
        //Below are good practice decorators to use for a GET request
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Necklace>))]
        public async Task<IEnumerable<Necklace>> GetNecklaces()
        {
            var neck = await _repo.ReadAllAsyncWithPearls();
            return neck;
        }

        //GET: api/necklaces/id
        [HttpGet("{neckId}", Name = nameof(GetNecklace))]
        [ProducesResponseType(200, Type = typeof(Necklace))]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]

        public async Task<IActionResult> GetNecklace(string neckId)
        {
            if (!int.TryParse(neckId, out int necklaceId))
            {
                return BadRequest("ID format error");
            }
            var neck = await _repo.ReadAsync(necklaceId);
            if (neck != null)
            {
                return Ok(neck);
            }
            else
            {
                return NotFound();
            }
        }

        //Body: Necklaces in Json
        //Note: Input ID has to be same as Changed ID
        [HttpPut("{neckIdString}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> UpdateNecklace(string neckIdString, [FromBody] Necklace neck)
        {
            if (!int.TryParse(neckIdString, out int neckId))
            {
                return BadRequest("ID format error");
            }
            if (neckId != neck.NecklaceID)
            {
                return BadRequest("Necklace ID mismatch");
            }

            await _repo.UpdateAsync(neck);
            if (neck != null)
            {
                //_logger.LogInformation("Updated necklace {neckIdString}", neckIdString);
                //Send an empty body response to confirm
                return new NoContentResult();
            }
            else
            {
                return NotFound();
            }
        }

        //DELETE: api/necklaces/id
        [HttpDelete("{neckIdString}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> DeleteNecklace(string neckIdString)
        {

            if (!int.TryParse(neckIdString, out int neckId))
            {
                return BadRequest("Id format error");
            }

            Necklace neck = await _repo.ReadAsync(neckId);
            if (neck == null)
            {
                return NotFound();
            }

            neck = await _repo.DeleteAsync(neckId);
            if (neck != null)
            {
                //_logger.LogInformation("Deleted necklace {neckIdString}", neckId);

                //Send an empty body response to confirm
                return new NoContentResult();
            }
            else
            {
                return BadRequest("Necklace found but could not be deleted");
            }
        }


        //POST: api/neklaces
        //Body: Necklaces in Json
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
                return BadRequest("Necklace ID already exists");
            }

            neck = await _repo.CreateAsync(neck);
            if (neck != null)
            {
                //201 created ok with url details to read newlys created necklace
                return CreatedAtRoute(

                    //Named Route in the HttpGet request
                    routeName: nameof(GetNecklace),

                    //neckId is the parameter in HttpGet
                    routeValues: new { neckId = neck.NecklaceID.ToString().ToLower() },

                    //Necklace detail in the Body
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
