using System;
using UnityEngine;
using UnityObject = UnityEngine.Object;

namespace Logging
{
    internal sealed class LogHandlerWithFallbackContext : ILogHandler
    {
        private readonly UnityObject fallbackContext;

        public LogHandlerWithFallbackContext(
            UnityObject fallbackContext)
        {
            this.fallbackContext = fallbackContext;
        }

        public void LogFormat(LogType logType, UnityObject context, string format, params object[] args)
        {
            Debug.unityLogger.logHandler.LogFormat(logType, context ?? fallbackContext, format, args);
        }

        public void LogException(Exception exception, UnityObject context)
        {
            Debug.unityLogger.LogException(exception, context ?? fallbackContext);
        }
    }
}