using Microsoft.Extensions.DependencyInjection;
using System;
using System.Text.Json;
using AutoMapper;
using CommandsService.Dtos;
using CommandsService.Models;
using CommandsService.Data;

namespace CommandsService.EventProcessing
{
    public class EventProcessor : IEventProcessor
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly IMapper _mapper;
        public EventProcessor(IServiceScopeFactory scopeFactory, AutoMapper.IMapper mapper)
        {
            _scopeFactory = scopeFactory;
            _mapper = mapper;
        }
        public void ProcessEvent(string message)
        {
            var eventType = DetermineEvent(message);

            switch(eventType)
            {
                case EventType.ShkolaPublished:
                    addShkola(message);
                    break;
                default:
                    break;
            }
        }    

        private EventType DetermineEvent(string notificationMessage)
        {
            Console.WriteLine("Determining Event");

            var eventType = JsonSerializer.Deserialize<GenericEventDto>(notificationMessage);

            switch(eventType.Event)
            {
                case "Shkola_Published":
                    Console.WriteLine("--> Shkola Published Event Detected");
                    return EventType.ShkolaPublished;
                default:
                    Console.WriteLine("--> Could not determine the event type");
                    return EventType.Undetermined;
            }
        } 

        private void addShkola(string shkolaPublishedMessage)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var repo = scope.ServiceProvider.GetRequiredService<ICommandRepo>();

                var shkolaPublishedDto = JsonSerializer.Deserialize<ShkolaPublishedDto>(shkolaPublishedMessage);

                try
                {
                    var plat = _mapper.Map<Shkola>(shkolaPublishedDto);
                    if(!repo.ExternalShkolaExists(plat.ExternalID))
                    {
                        repo.CreateShkola(plat);
                        repo.SaveChanges();
                        Console.WriteLine("--> Shkola added!");
                    }
                    else
                    {
                        Console.WriteLine("--> Shkola already exisits...");
                    }

                }
                catch (Exception ex)
                {
                    Console.WriteLine($"--> Could not add Shkola to DB {ex.Message}");
                }

            }
        }
    }
    enum EventType
    {
        ShkolaPublished,
        Undetermined
    }   
}