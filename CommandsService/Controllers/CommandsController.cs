using System;
using System.Collections.Generic;
using AutoMapper;
using CommandsService.Data;
using CommandsService.Dtos;
using CommandsService.Models;
using Microsoft.AspNetCore.Mvc;

namespace CommandsService.Controllers
{
    [Route("api/c/shkolas/{shkolaId}/[controller]")]
    [ApiController]
    public class CommandsController : ControllerBase
    {
        private readonly ICommandRepo _repository;
        private readonly IMapper _mapper;
       
        public CommandsController(ICommandRepo repository, IMapper mapper)
 
        {
            _repository = repository;
            _mapper = mapper;       
        }

        [HttpGet]
        public ActionResult<IEnumerable<CommandReadDto>> GetCommandsForShkola(int shkolaId)
        {
            Console.WriteLine($"--> Hit GetCommandsForShkola: {shkolaId}");

            if(!_repository.ShkolaExits(shkolaId))
            {
                return NotFound();
            }

            var commands = _repository.GetCommandsForShkola(shkolaId);

            return Ok(_mapper.Map<IEnumerable<CommandReadDto>>(commands));
        }

        [HttpGet("{commandId}", Name = "GetCommandForShkola")]
        public ActionResult<CommandReadDto> GetCommandForShkola(int shkolaId, int commandId)
        {
                        Console.WriteLine($"--> Hit GetCommandForShkola: {shkolaId} / {commandId}");

            if(!_repository.ShkolaExits(shkolaId))
            {
                return NotFound();
            }

            var command = _repository.GetCommand(shkolaId, commandId);

            if(command == null)
            {
                return NotFound();
            
            }

            return Ok(_mapper.Map<CommandReadDto>(command));
        }

        [HttpPost]
        public ActionResult<CommandReadDto> CreateCommandForShkola(int shkolaId, CommandCreateDto commandDto)
        {
             Console.WriteLine($"--> Hit CreateCommandForShkola: {shkolaId}");

            if (!_repository.ShkolaExits(shkolaId))
            {
                return NotFound();
            }

            var command = _mapper.Map<Command>(commandDto);

            _repository.CreateCommand(shkolaId, command);
            _repository.SaveChanges();

            var commandReadDto = _mapper.Map<CommandReadDto>(command);

            return CreatedAtRoute(nameof(GetCommandForShkola),
                new {shkolaId = shkolaId, commandId = commandReadDto.Id}, commandReadDto);
        }
    }
}