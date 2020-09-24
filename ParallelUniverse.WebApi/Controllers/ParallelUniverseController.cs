using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParallelUniverse.WebApi.Controllers
{
    [Route("api")]
    [ApiController]
    public class ParallelUniverseController : ControllerBase
    {
        private readonly UniverseService _universeService;

        public ParallelUniverseController(UniverseService universeService)
        {
            _universeService = universeService;
        }

        [HttpGet("pulink")]
        public string GeneratePortalLink(string link, TimeSpan duration)
        {
            return _universeService.GeneratePortalLink(link, duration);
        }
    }
}
