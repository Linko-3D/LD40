﻿using UnityEngine;

/// <summary>
/// Encapsulates Unity Logger class.
/// 
/// Custom logging handlers(.eg log to files) should be encapsulated in this class.
/// More info about logger handlers: https://docs.unity3d.com/ScriptReference/ILogHandler.html
/// </summary>
public class Logger {
    
    private string _globalContext;
    private UnityEngine.Logger _logger;

    public Logger(string globalContext) {
        _globalContext = globalContext;
        _logger = new UnityEngine.Logger(Debug.unityLogger.logHandler);
    }

    public Logger(string globalContext, ILogHandler handler) {
        _globalContext = globalContext;
        _logger = new UnityEngine.Logger(handler);
    }
    
    public void Info(object obj) {
        _logger.Log(_globalContext, obj.ToString());
    }
    
    public void Info(string localContext, object obj) {
        _logger.Log(_globalContext + "::" + localContext, obj.ToString());
    }
    
    public void Warn(object obj) {
        _logger.LogWarning(_globalContext, obj.ToString());
    }
    
    public void Warn(string localContext, object obj) {
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
