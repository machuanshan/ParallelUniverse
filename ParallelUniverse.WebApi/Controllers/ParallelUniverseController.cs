using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParallelUniverse.WebApi.Controllers
{
    [Route("api/pu")]
    [ApiController]
    public class ParallelUniverseController : ControllerBase
    {
        private readonly UniverseService _universeService;

        public ParallelUniverseController(UniverseService universeService)
        {
            _universeService = universeService;
        }

        [HttpPost("link")]
        public string GeneratePortalLink(string target, TimeSpan duration)
        {
            return _universeService.GeneratePortalLink(target, duration);
        }
    }
}
