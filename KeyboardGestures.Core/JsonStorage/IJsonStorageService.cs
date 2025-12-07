using KeyboardGestures.Core.Commands;

namespace KeyboardGestures.Core.JsonStorage
{
    public interface IJsonStorageService
    {
        List<CommandDefinition> Load();
        void Save(IEnumerable<CommandDefinition> commands);
    }
}
