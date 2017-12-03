using UnityEngine;

/// <summary>
/// Encapsulates Unity Logger class.
/// 
/// Custom logging handlers(.eg log to files) should be encapsulated in this class.
/// More info about logger handlers: https://docs.unity3d.com/ScriptReference/ILogHandler.html
/// </summary>
public class Logger {
    
    public enum Level {
        Info,
        Warn,
        Error
    }

    private string _globalContext;
    private Level _level;
    private UnityEngine.Logger _logger;

    public Logger(string globalContext, Level lvl) {
        _level = lvl;
        _globalContext = globalContext;
        _logger = new UnityEngine.Logger(Debug.unityLogger.logHandler);
    }

    public void Info(object obj) {
        if (_level != Level.Info) return;
        _logger.Log(_globalContext, obj.ToString());
    }
    
    public void Info(string localContext, object obj) {
        if (_level != Level.Info) return;
        _logger.Log(_globalContext + "::" + localContext, obj.ToString());
    }
    
    public void Warn(object obj) {
        if (_level < Level.Warn) return;
        _logger.LogWarning(_globalContext, obj.ToString());
    }
    
    public void Warn(string localContext, object obj) {
        if (_level < Level.Warn) return;
        _logger.LogWarning(_globalContext + "::" + localContext, obj.ToString());
    }
    
    public void Error(object obj) {
        _logger.LogError(_globalContext, obj.ToString());
    }
    
    public void Error(string localContext, object obj) {
        _logger.LogError(_globalContext + "::" + localContext, obj.ToString());
    }
    
    public void Assert(bool shouldSucceed, object obj) {
        if (!shouldSucceed) {
            _logger.LogError(_globalContext + "::", obj.ToString());
        }
    }
    
    public void Assert(bool shouldSucceed, string localContext, object obj) {
        if (!shouldSucceed) {
            _logger.LogError(_globalContext + "::" + localContext, obj.ToString());
        }
    }
}
