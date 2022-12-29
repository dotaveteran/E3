using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SchoolService.Data;
using SchoolService.Dtos;
using SchoolService.Models;
using SchoolService.SyncDataServices.Http;
using SchoolService.AsyncDataServices;

namespace SchoolService.Controllers
{
    [Route("api/[controller]") ]
    [ApiController]    
    public class ShkolasController : ControllerBase
    {
        
        private readonly IShkolaRepo _repository;
        private readonly IMapper _mapper;
        private readonly ICommandDataClient _commandDataClient;
        private readonly IMessageBusClient _messageBusClient;


        public ShkolasController(
            IShkolaRepo repository,
            IMapper mapper,
            ICommandDataClient commandDataClient,
            IMessageBusClient  messageBusClient)
        {
            _repository = repository;
            _mapper = mapper;
            _commandDataClient = commandDataClient;
            _messageBusClient = messageBusClient;
        }

        [HttpGet]
        public ActionResult<IEnumerable<ShkolaReadDto>> GetShkolas()
        {
            Console.WriteLine("--> Getting Shkolas....");
            
            var shkolaItem = _repository.GetAllShkolas();

            return Ok(_mapper.Map<IEnumerable<ShkolaReadDto>>(shkolaItem));
        }
    

        [HttpGet("{id}", Name = "GetShkolaById")]
        public ActionResult<ShkolaReadDto> GetShkolaById(int id)
        {
            var shkolaItem = _repository.GetShkolaById(id);
            if(shkolaItem != null)
            {
                return Ok(_mapper.Map<ShkolaReadDto>(shkolaItem));    
            }

            return NotFound();
        }

        [HttpPost]
        public async Task<ActionResult<ShkolaReadDto>> CreateShkola(ShkolaCreateDto shkolaCreateDto)
        {
            var shkolaModel = _mapper.Map<Shkola>(shkolaCreateDto);
            _repository.CreateShkola(shkolaModel);
            _repository.SaveChanges();

            var shkolaReadDto = _mapper.Map<ShkolaReadDto>(shkolaModel);


            // Send Sync Message
            try
            {
                await _commandDataClient.SendShkolaToCommand(shkolaReadDto);
            }
            catch(Exception ex)
            {
                Console.WriteLine($"--> Could not send synchronously: {ex.Message}");
            }

            // Send Async Message
            try
            {
                 var shkolaPublishedDto = _mapper.Map<ShkolaPublishedDto>(shkolaReadDto);
                shkolaPublishedDto.Event = "Shkola_Published";
                _messageBusClient.PublishNewShkola(shkolaPublishedDto);
            }
            catch(Exception ex)
            {
                Console.WriteLine($"--> Could not send asynchronously: {ex.Message}");
            }

            return CreatedAtRoute(nameof(GetShkolaById), new { Id = shkolaReadDto.Id}, shkolaReadDto);
        }
    }
}