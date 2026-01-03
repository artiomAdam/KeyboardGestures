namespace KeyboardGestures.Core.JsonStorage
{
    public interface IJsonStorage<T>
    {
        T Load();
        void Save(T data);
    }
}
