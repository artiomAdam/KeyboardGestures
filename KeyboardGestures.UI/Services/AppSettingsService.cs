using KeyboardGestures.Core.JsonStorage;
using KeyboardGestures.Core.Settings;

namespace KeyboardGestures.UI.Services
{
    public class AppSettingsService : IAppSettingsService
    {
        private readonly AppSettingsStorage _storage;
        public AppSettings Current { get; }

        public AppSettingsService(AppSettingsStorage storage)
        {
            _storage = storage;
            Current = _storage.Load() ?? new AppSettings();
        }

        public void Save()
        {
            _storage.Save(Current);
        }
    }
}
