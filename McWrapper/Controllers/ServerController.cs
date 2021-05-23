using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using McWrapper.Models;
using McWrapper.Services.Servers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace McWrapper.Controllers
{
    [ApiController]
    [ApiVersion("1")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class ServerController : ControllerBase
    {
        private readonly ILogger<ServerController> _logger;
        private readonly IServerService _serverService;

        public ServerController(IServerService serverService, ILogger<ServerController> logger)
        {
            _serverService = serverService ?? throw new ArgumentNullException(nameof(serverService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
        
        #region ServerManager operations
        [HttpGet("{id:guid}/start")]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<bool> Start(Guid id)
            => ServerService.ServerManager.StartServer(id);
        
        [HttpGet("{id:guid}/stop")]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<bool> Stop(Guid id)
            => ServerService.ServerManager.StopServer(id);
        
        [HttpGet("{id:guid}/accepteula")]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<bool> AcceptEula(Guid id)
            => ServerService.ServerManager.AcceptEula(id);
        
        [HttpGet("{id:guid}/info")]
        [ProducesResponseType(typeof(McWrapperLib.Server), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<McWrapperLib.Server> Info(Guid id)
            => ServerService.ServerManager.GetInfo(id);

        [HttpGet("stopall")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult StopAll()
        {
            ServerService.ServerManager.StopAll();
            return NoContent();
        }
        
        [HttpGet("wait")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult Wait()
        {
            ServerService.ServerManager.WaitForAllToStop();
            return NoContent();
        }
        
        [HttpPost("{id:guid}/command")]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<bool> Command(Guid id, [FromQuery] string command)
            => ServerService.ServerManager.ExecuteCommand(id, command);

        [HttpGet("{id:guid}/log")]
        [ProducesResponseType(typeof(FileStreamResult), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public FileStreamResult Log(Guid id)
            =>new FileStreamResult(System.IO.File.OpenRead(ServerService.ServerManager.GetLog(id)), "application/octet-stream");

        [HttpGet("{id:guid}/minecraftlog")]
        [ProducesResponseType(typeof(FileStreamResult), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public FileStreamResult MinecraftLog(Guid id)
            =>new FileStreamResult(System.IO.File.OpenRead(ServerService.ServerManager.GetMinecraftLog(id)), "application/octet-stream");
        
        [HttpGet("{id:guid}/running")]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<bool> Running(Guid id)
            => ServerService.ServerManager.IsRunning(id);

        [HttpGet("{id:guid}/wait")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult WaitForStop(Guid id)
        {
            ServerService.ServerManager.WaitForStop(id);
            return NoContent();
        }
        
        [HttpGet("{id:guid}/plugins")]
        [ProducesResponseType(typeof(string[]), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<string[]> Plugins(Guid id)
            => ServerService.ServerManager.GetPlugins(id);
        #endregion

        #region Database operations
        [HttpGet]
        [ProducesResponseType(typeof(List<Server>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<List<Server>>> All()
        {
            var servers = new List<Server>();
            servers.AddRange(await _serverService.GetAll());
            return servers;
        }
        
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> Add([FromForm] Server server, ApiVersion version)
        {
            try
            {
                var createdServer = await _serverService.Add(server);
            
                return CreatedAtAction("GetById", 
                    "Server",
                    new { id = createdServer.Id, version = version.ToString() }, 
                    createdServer);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        
        [HttpGet("{id:guid}")]
        [ProducesResponseType(typeof(Server), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Server>> GetById(Guid id, [FromQuery] bool enrichEnabledPlugins = true)
        {
            var server = await _serverService.Get(id, enrichEnabledPlugins);

            if (server == null)
                return NotFound();
            return server;
        }

        [HttpDelete("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> Delete(Guid id)
        {
            try
            {
                await _serverService.Remove(id);

                return NoContent();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        #endregion
    }
}