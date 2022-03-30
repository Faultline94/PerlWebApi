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
    public class NecklaceController : ControllerBase
    {
        private INecklaceRepository _repo;

        //GET: api/customers
        //GET: api/customers/?country={country}
        //Below are good practice decorators to use for a GET request
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

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Necklace>))]
        public async Task<IEnumerable<Necklace>> GetAllNecklaces()
        {
                var neck = await _repo.ReadAllAsyncWithPearls();
                return neck;
        }

        public NecklaceController(INecklaceRepository repo, ILogger<NecklaceController> logger)
        {
            _repo = repo;
            AppLog.Instance.LogInformation("Controller started");
        }

    }
}
