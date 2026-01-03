using System.Text.Json;
using System.Text.Json.Serialization;

namespace KeyboardGestures.Core.JsonStorage
{
    public class JsonFileStorage<T> : IJsonStorage<T> where T : new()
    {
        private readonly string _path;
        private readonly JsonSerializerOptions _options;

        public JsonFileStorage(string path, JsonSerializerOptions? options = null)
        {
            _path = path;
            _options = options ?? new JsonSerializerOptions
            {
                WriteIndented = true,
                PropertyNameCaseInsensitive = true,
                Converters = { new JsonStringEnumConverter() }
            };
        }

        public T Load()
        {
            if (!File.Exists(_path)) return new T();

            var json = File.ReadAllText(_path);
            return JsonSerializer.Deserialize<T>(json, _options) ?? new T();
        }

        public void Save(T data)
        {
            var json = JsonSerializer.Serialize(data, _options);
            File.WriteAllText(_path, json);
        }
    }
}
