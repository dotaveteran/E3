using SchoolService.Models;
using System.Linq;
using System;
using Microsoft.EntityFrameworkCore;

namespace SchoolService.Data
{
    public static class PrepDb
    {
        public static void PrepPopulation(IApplicationBuilder app, bool isProd)
        {
            using(var serviceScope = app.ApplicationServices.CreateScope())
            {
                SeedData(serviceScope.ServiceProvider.GetService<AppDbContext>(), isProd);
            }
        }
        private static void SeedData(AppDbContext context, bool isProd)
        {
            if(isProd)
            {
                Console.WriteLine("---> Attempting to apply migrations....");
                try
                {
                    context.Database.Migrate();
                }
                catch(Exception ex)
                {
                    Console.WriteLine($"---> Could not run migrations: {ex.Message}");
                }
            }
            if(!context.Shkolas.Any())
            {
                Console.WriteLine("---> Seeding Data...");
                context.Shkolas.AddRange(
                    new Shkola() {Name="Sasha", Publisher="Student", Grade="Excellent"},
                    new Shkola() {Name="Dima", Publisher="Student", Grade="Excellent"},
                    new Shkola() {Name="George", Publisher="Student", Grade="Excellent"}
                ); 
                

                context.SaveChanges();
            }
            else
            {
                Console.WriteLine("---> We already have data");
            }
        }
        
    }
}