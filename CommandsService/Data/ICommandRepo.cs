using CommandsService.Models;

namespace CommandsService.Data
{
    public interface ICommandRepo
    {
        bool SaveChanges();

        // Shkolas
        IEnumerable<Shkola> GetAllShkolas();
        void CreateShkola(Shkola plat);
        bool ShkolaExits(int shkolaId);
        bool ExternalShkolaExists(int externalShkolaId);

        // Commands
        IEnumerable<Command> GetCommandsForShkola(int shkolaId);
        Command GetCommand(int shkolaId, int commandId);
        void CreateCommand(int shkolaId, Command command);
    }
}