using KeyboardGestures.Core.JsonStorage;
using KeyboardGestures.Core.Settings;

namespace KeyboardGestures.UI.Services
{
    public class AppSettingsService : IAppSettingsService
    {
        private readonly AppSettingsStorage _storage;
        public AppSettings Current { get; }

        public AppSettingsService(AppSettings settings, AppSettingsStorage storage)
        {
            _storage = storage;
            Current = settings;
        }

        public void Save()
        {
            _storage.Save(Current);
        }
    }
}
