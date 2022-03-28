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

        }
    }
}
