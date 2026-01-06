namespace KeyboardGestures.Core.Settings
{
    public interface IAppSettingsService
    {
        AppSettings Current { get; }
        void Save();
    }
}
