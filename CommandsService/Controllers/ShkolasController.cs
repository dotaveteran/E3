using AutoMapper;
using CommandsService.Data;
using CommandsService.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace CommandsService.Controllers
{
    [Route("api/c/[controller]")]
    [ApiController]
    public class ShkolasController : ControllerBase
    {
        private readonly ICommandRepo _repository;
        private readonly IMapper _mapper;
        
        public ShkolasController(ICommandRepo repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;  
        }
        [HttpGet]
        public ActionResult<IEnumerable<ShkolareadDto>> GetShkolas()
        {
            Console.WriteLine("--> Getting Shkolas from CommandService");
            var shkolaItems = _repository.GetAllShkolas();

            return Ok(_mapper.Map<IEnumerable<ShkolareadDto>>(shkolaItems));
        }

        [HttpPost]
        public ActionResult TestInboundConnection()
        {
            Console.WriteLine("--> Inbound POST # Command Service");

            return Ok("Inbound test of from Shkolas Controler");
        }
    }
}