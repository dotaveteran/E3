using System;
using System.Collections.Generic;
using CommandsService.Models;
using CommandsService.SyncDataServices.Grpc;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace CommandsService.Data
{
    public static class PrepDb
    {
        public static void PrepPopulation(IApplicationBuilder applicationBuilder)
        {
            using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
            {
                var grpcClient = serviceScope.ServiceProvider.GetService<IShkolaDataClient>();

                var shkolas = grpcClient.ReturnAllShkolas();

                SeedData(serviceScope.ServiceProvider.GetService<ICommandRepo>(), shkolas);
            }
        }
        
        private static void SeedData(ICommandRepo repo, IEnumerable<Shkola> shkolas)
        {
            Console.WriteLine("Seeding new shkolas...");

            foreach (var plat in shkolas)
            {
                if(!repo.ExternalShkolaExists(plat.ExternalID))
                {
                    repo.CreateShkola(plat);
                }
                repo.SaveChanges();
            }
        }
    }
}