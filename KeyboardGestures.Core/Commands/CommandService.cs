
using KeyboardGestures.Core.JsonStorage;
using System.Collections.ObjectModel;

namespace KeyboardGestures.Core.Commands
{
    public class CommandService : ICommandService
    {
        private readonly CommandRegistry _registry;
        private readonly IJsonStorageService _storage;

        public CommandService(CommandRegistry registry, IJsonStorageService storage)
        {
            _registry = registry;
            _storage = storage;
            foreach (var cmd in _storage.Load())
                _registry.Register(cmd);
        }

        public IEnumerable<CommandDefinition> LoadAll()
            => _registry.GetAll();

        public void AddNew(CommandDefinition cmd)
        {
            _registry.Register(cmd);
            SaveAll();
        }


        public void UpdateCommand(CommandDefinition cmd, List<int>? oldSequence = null)
        {
            if (oldSequence != null && _registry.ContainsSequence(oldSequence))
            {
                _registry.UpdateSequence(cmd, oldSequence);
                SaveAll();
                return;
            }
            _registry.Register(cmd);

            SaveAll();
        }

        public void Delete(CommandDefinition cmd)
        {
            _registry.Remove(cmd);
            SaveAll();
        }

        private void SaveAll()
        {
            var all = _registry.GetAll().ToList();
            _storage.Save(all);
        }
    }
}
