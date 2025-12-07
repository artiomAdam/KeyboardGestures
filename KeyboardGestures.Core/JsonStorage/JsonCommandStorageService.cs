using KeyboardGestures.Core.Commands;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace KeyboardGestures.Core.JsonStorage
{
    public class JsonCommandStorageService : IJsonStorageService
    {
        private readonly string _path;
        private readonly JsonSerializerOptions _options;

        public JsonCommandStorageService(string path)
        {
            _path = path;
            _options = new JsonSerializerOptions
            {
                WriteIndented = true,
                PropertyNameCaseInsensitive = true,
                Converters = { new JsonStringEnumConverter() }
            };
        }

        public List<CommandDefinition> Load()
        {
            if (!File.Exists(_path)) return new();
            var json = File.ReadAllText(_path);
            return JsonSerializer.Deserialize<List<CommandDefinition>>(json, _options) ?? new();
        }

        public void Save(IEnumerable<CommandDefinition> commands)
        {
            var json = JsonSerializer.Serialize(commands, _options);
            File.WriteAllText(_path, json);
        }
    }
}
