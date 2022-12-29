using System;
using System.Collections.Generic;
using AutoMapper;
using CommandsService.Models;
using Grpc.Net.Client;
using Microsoft.Extensions.Configuration;
using SchoolService;

namespace CommandsService.SyncDataServices.Grpc
{
    public class ShkolaDataClient : IShkolaDataClient
    {
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;

        public ShkolaDataClient(IConfiguration configuration, IMapper mapper)
        {
            _configuration = configuration;
            _mapper = mapper;
        }

        public IEnumerable<Shkola> ReturnAllShkolas()
        {
            Console.WriteLine($"--> Calling GRPC Service {_configuration["GrpcShkola"]}");
            var channel = GrpcChannel.ForAddress(_configuration["GrpcShkola"]);
            var client = new GrpcShkola.GrpcShkolaClient(channel);
            var request = new GetAllRequest();

            try
            {
                var reply = client.GetAllShkolas(request);
                return _mapper.Map<IEnumerable<Shkola>>(reply.Shkola);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"--> Couldnot call GRPC Server {ex.Message}");
                return null;
            }
        }
    }
}