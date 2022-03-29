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
        private NecklaceRepository _repo;

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Necklace>))]

        public async Task<IEnumerable<Necklace>> GetNecklaces()
        {
            var neck = await _repo.ReadAllAsync();
            return neck;
        }

        [HttpGet("{necklaceId}", Name = nameof(GetNecklace))]
        [ProducesResponseType(200, Type = typeof(Necklace))]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetNecklace(string necklaceId)
        {
            if (int.TryParse(necklaceId, out int neckVar))
            {
                return BadRequest("Guid format error");
            }
            Necklace neck = await _repo.ReadAsync(neckVar);
            if (neck != null)
            {
                //cust is returned in the body
                return Ok(neck);
            }
            else
            {
                return NotFound();
            }
        }
    }
}
