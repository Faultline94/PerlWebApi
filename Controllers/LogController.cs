using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace PerlWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LogController : ControllerBase
    {
        public LogController () { }

        //GET /Log
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<AppLogItem>))]

        public IEnumerable<AppLogItem> Get()
        {
            return AppLog.Instance.ToArray();
        }
    }
}
