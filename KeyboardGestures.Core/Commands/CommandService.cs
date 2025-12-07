
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

        public void UpdateSequence(CommandDefinition cmd, List<int> newSeq)
        {
            var oldSeq = cmd.Sequence.ToList();

            cmd.Sequence = newSeq;
            _registry.UpdateSequence(cmd, oldSeq);

            SaveAll();
        }

        public void UpdateCommand(CommandDefinition cmd)
        {
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
