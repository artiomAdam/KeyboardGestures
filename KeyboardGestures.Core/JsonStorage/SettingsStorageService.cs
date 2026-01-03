using KeyboardGestures.Core.Settings;

namespace KeyboardGestures.Core.JsonStorage
{
    public class SettingsStorageService
    {
        private readonly IJsonStorage<AppSettings> _storage;

        public SettingsStorageService(IJsonStorage<AppSettings> storage)
        {
            _storage = storage;
        }

        public AppSettings Load() => _storage.Load();
        public void Save(AppSettings appSettings) => _storage.Save(appSettings);
    }
}
