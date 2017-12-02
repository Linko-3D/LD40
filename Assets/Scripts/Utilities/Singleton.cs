public class Singleton<T> where T : class, new() {

    static T _instance = null;

    public T Instance {
        get {
            if (_instance == null) {
                _instance = new T();
            }

            return _instance;
        }
    }
}