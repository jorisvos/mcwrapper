using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using McWrapper.Models;
using McWrapper.Services.Plugins;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace McWrapper.Controllers
{
    [ApiController]
    [ApiVersion("1")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class PluginController : ControllerBase
    {
        private readonly ILogger<PluginController> _logger;
        private readonly IPluginService _pluginService;

        public PluginController(IPluginService pluginService, ILogger<PluginController> logger)
        {
            _pluginService = pluginService ?? throw new ArgumentNullException(nameof(pluginService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpGet]
        [ProducesResponseType(typeof(List<Plugin>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<List<Plugin>>> All()
        {
            var plugins = new List<Plugin>();
            plugins.AddRange(await _pluginService.GetAll());
            return plugins;
        }
        
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> Upload([FromForm] Plugin plugin, [FromForm] IFormFile file, ApiVersion version)
        {
            if (file == null)
                return BadRequest("The plugin has to be uploaded under the name 'file'.");
            
            try
            {
                var uploadedPlugin = await _pluginService.Add(plugin, file);
            
                return CreatedAtAction("GetById", 
                    "Plugin",
                    new { id = uploadedPlugin.Id, version = version.ToString() }, 
                    uploadedPlugin);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        
        [HttpGet("{id:guid}")]
        [ProducesResponseType(typeof(Plugin), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Plugin>> GetById(Guid id)
        {
            var plugin = await _pluginService.Get(id);

            if (plugin == null)
                return NotFound();
            return plugin;
        }

        [HttpDelete("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> Delete(Guid id)
        {
            await _pluginService.Remove(id);

            return NoContent();
        }
    }
}