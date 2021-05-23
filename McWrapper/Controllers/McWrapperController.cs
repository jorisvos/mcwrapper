using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using McWrapper.Services.McWrapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace McWrapper.Controllers
{
    [ApiController]
    [ApiVersion("1")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class McWrapperController : ControllerBase
    {
        private readonly ILogger<McWrapperController> _logger;
        private readonly IMcWrapperService _mcWrapperService;

        public McWrapperController(IMcWrapperService mcWrapperService, ILogger<McWrapperController> logger)
        {
            _mcWrapperService = mcWrapperService ?? throw new ArgumentNullException(nameof(mcWrapperService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpGet]
        [ProducesResponseType(typeof(List<Models.McWrapper>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<List<Models.McWrapper>>> All()
        {
            var mcWrapper = new List<Models.McWrapper>();
            mcWrapper.AddRange(await _mcWrapperService.GetAll());
            return mcWrapper;
        }
        
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> Add([FromForm] Models.McWrapper mcWrapper, ApiVersion version)
        {
            try
            {
                var createdMcWrapper = await _mcWrapperService.Add(mcWrapper);
            
                return CreatedAtAction("GetById", 
                    "McWrapper",
                    new { key = createdMcWrapper.Key, version = version.ToString() }, 
                    createdMcWrapper);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        
        [HttpPut]
        [ProducesResponseType(typeof(Models.McWrapper), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Models.McWrapper>> Update([FromForm] Models.McWrapper mcWrapper, ApiVersion version)
            => await _mcWrapperService.Update(mcWrapper);
        
        [HttpGet("{key}")]
        [ProducesResponseType(typeof(Models.McWrapper), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Models.McWrapper>> GetById(string key)
        {
            var mcWrapper = await _mcWrapperService.Get(key);

            if (mcWrapper == null)
                return NotFound();
            return mcWrapper;
        }

        [HttpDelete("{key}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> Delete(string key)
        {
            await _mcWrapperService.Remove(key);

            return NoContent();
        }
    }
}