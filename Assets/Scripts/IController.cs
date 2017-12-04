public interface IController {
    string Name { get; }
    void OnResetEvent();
    void OnDisableEvent();
}
