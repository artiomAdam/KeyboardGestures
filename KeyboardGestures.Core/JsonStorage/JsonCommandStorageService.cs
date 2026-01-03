using KeyboardGestures.Core.Commands;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace KeyboardGestures.Core.JsonStorage
{
    public class JsonCommandStorageService
    {
        private readonly IJsonStorage<List<CommandDefinition>> _storage;

        public JsonCommandStorageService(IJsonStorage<List<CommandDefinition>> storage)
        {
            _storage = storage;
        }

        public List<CommandDefinition> Load() => _storage.Load();

        public void Save(IEnumerable<CommandDefinition> commands) => _storage.Save(commands.ToList());
    }
}
