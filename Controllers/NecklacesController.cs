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

        //GET: api/customers
        //GET: api/customers/?country={country}
        //Below are good practice decorators to use for a GET request
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Necklace>))]
        public async Task<IEnumerable<Necklace>> GetNecklaces(string neckId)
        {
            if (string.IsNullOrWhiteSpace(neckId))
            {
                var neck = await _repo.ReadAllAsync();
                return neck;
            }
            else
            {
                var list = await _repo.ReadAllAsync();
                return list.Where(neck => neck.NecklaceID.ToString() == neckId);
            }
        }
        public NecklacesController(INecklaceRepository repo, ILogger<NecklacesController> logger)
        {
            _repo = repo;
            AppLog.Instance.LogInformation("Controller started");
        }

    }
}
