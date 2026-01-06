using KeyboardGestures.Core.Commands;

namespace KeyboardGestures.Core.JsonStorage
{
    public class CommandStorage
    {
        private readonly IJsonStorage<List<CommandDefinition>> _storage;

        public CommandStorage(IJsonStorage<List<CommandDefinition>> storage)
        {
            _storage = storage;
        }

        public List<CommandDefinition> Load() => _storage.Load();

        public void Save(IEnumerable<CommandDefinition> commands) => _storage.Save(commands.ToList());
    }
}
