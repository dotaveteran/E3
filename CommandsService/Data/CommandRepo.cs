using System;
using System.Collections.Generic;
using System.Linq;
using CommandsService.Models;

namespace CommandsService.Data
{
    public class CommandRepo : ICommandRepo
    {
        private readonly AppDbContext _context;

        public CommandRepo(AppDbContext context)
        {
            _context = context;
        }

        public void CreateCommand(int shkolaId, Command command)
        {
            if (command == null)
            {
                throw new ArgumentNullException(nameof(command));
            }

            command.ShkolaId = shkolaId;
            _context.Commands.Add(command);
        }

        public void CreateShkola(Shkola plat)
        {
            if(plat == null)
            {
                throw new ArgumentNullException(nameof(plat));
            }
            _context.Shkolas.Add(plat);
        }

        public bool ExternalShkolaExists(int externalShkolaId)
        {
            return _context.Shkolas.Any(p => p.ExternalID == externalShkolaId);
        }

        public IEnumerable<Shkola> GetAllShkolas()
        {
            return _context.Shkolas.ToList();
        }

        public Command GetCommand(int shkolaId, int commandId)
        {
            return _context.Commands
                .Where(c => c.ShkolaId == shkolaId && c.Id == commandId).FirstOrDefault();
        }

        public IEnumerable<Command> GetCommandsForShkola(int shkolaId)
        {
            return _context.Commands
                .Where(c => c.ShkolaId == shkolaId)
                .OrderBy(c => c.Shkola.Name);
        }

        public bool ShkolaExits(int shkolaId)
        {
            return _context.Shkolas.Any(p => p.Id == shkolaId);
        }

        public bool SaveChanges()
        {
            return (_context.SaveChanges() >= 0);
        }
    }
}