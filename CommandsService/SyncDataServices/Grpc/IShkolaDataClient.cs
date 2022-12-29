using System.Collections.Generic;
using CommandsService.Models;

namespace CommandsService.SyncDataServices.Grpc
{
    public interface IShkolaDataClient
    {
        IEnumerable<Shkola> ReturnAllShkolas();
    }
}