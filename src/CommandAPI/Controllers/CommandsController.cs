using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using CommandAPI.Data;
using CommandAPI.Models;

namespace CommandAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommandsController : ControllerBase
    {
        private readonly ICommandAPIRepo _repository;
        public CommandsController(ICommandAPIRepo respository)
        {
            _repository = respository;
        }
        [HttpGet]
        public ActionResult<IEnumerable<Command>> Get(){
            return Ok(_repository.GetAllCommands());
        }

        [HttpGet("{id}")]
        public ActionResult<Command> GetCommandById(int id){
            var commandItem = _repository.GetCommandById(id);
            if(commandItem == null)
                return NotFound();

            return Ok(commandItem);
        }
    }
}