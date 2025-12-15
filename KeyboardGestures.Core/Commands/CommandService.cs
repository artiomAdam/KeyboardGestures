
using KeyboardGestures.Core.JsonStorage;
using System.Collections.ObjectModel;

namespace KeyboardGestures.Core.Commands
{
    /* Command uniqueness is */
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
    => _registry.GetAll()
        .OrderBy(c => CommandRules.GetOrder(c.CommandType))
        .ThenBy(c => c.CommandType.ToString());

        public void AddNew(CommandDefinition cmd)
        {
            if (_registry.ContainsSequence(cmd.Sequence))
                throw new InvalidOperationException("Key sequence already exists.");

            if (CommandRules.IsSingleInstance(cmd.CommandType) &&
                _registry.GetAll().Any(c => c.CommandType == cmd.CommandType))
                throw new InvalidOperationException($"{cmd.CommandType} already exists.");

            if(cmd.Sequence.Count == 0) throw new InvalidOperationException("Empty key unacceptable.");
            _registry.Register(cmd);
            SaveAll();
        }


        public void UpdateCommand(CommandDefinition cmd, List<int>? oldSequence)
        {
            var newKey = cmd.Sequence;
            if(newKey.Count == 0) throw new InvalidOperationException("Empty key unacceptable.");

            if (!oldSequence.SequenceEqual(newKey))
            {
                if (_registry.ContainsSequence(newKey))
                    throw new InvalidOperationException("Key sequence already exists.");
            }

            if (CommandRules.IsSingleInstance(cmd.CommandType))
            {
                var existing = _registry.GetAll()
                    .FirstOrDefault(c => c.CommandType == cmd.CommandType);

                if (existing != null && !existing.Sequence.SequenceEqual(oldSequence))
                    throw new InvalidOperationException($"{cmd.CommandType} already exists.");
            }

            if (!oldSequence.SequenceEqual(newKey))
                _registry.UpdateSequence(cmd, oldSequence);
            else
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
