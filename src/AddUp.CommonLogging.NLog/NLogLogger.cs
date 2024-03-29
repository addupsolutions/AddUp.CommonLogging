#region License

/*
 * Copyright © 2002-2007 the original author or authors.
 * 
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 * 
 *      http://www.apache.org/licenses/LICENSE-2.0
 * 
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

#endregion

using System;
using AddUp.CommonLogging.Factory;
using NLog;
using LoggerNLog = NLog.Logger;
using LogLevelNLog = NLog.LogLevel;

namespace AddUp.CommonLogging.NLog
{
    /// <summary>
    /// Concrete implementation of <see cref="ILog"/> interface specific to NLog 1.0.0.505-2.0.
    /// </summary>
    /// <remarks>
    /// NLog is a .NET logging library designed with simplicity and flexibility in mind.
    /// http://www.nlog-project.org/
    /// </remarks>
    /// <author>Bruno Baia</author>
    public sealed partial class NLogLogger : AbstractLogger
    {
        // Stack unwinding algorithm was changed in NLog2 (now it checks for system assemblies and logger type)
        // so we need this workaround to make it display correct stack trace.
        private readonly static Type declaringType = typeof(NLogLogger);
        private readonly LoggerNLog logger;

        internal NLogLogger(LoggerNLog nlogLogger) => logger = nlogLogger;

        /// <summary>
        /// Actually sends the message to the underlying log system.
        /// </summary>
        /// <param name="level">the level of this log event.</param>
        /// <param name="message">the message to log</param>
        /// <param name="exception">the exception to log (may be null)</param>
        protected override void WriteInternal(LogLevel level, object message, Exception exception)
        {
            LogLevelNLog nlogLevel = GetLevel(level);
            LogEventInfo logEvent = new LogEventInfo(nlogLevel, logger.Name, null, "{0}", new object[] { message }, exception);

            logger.Log(declaringType, logEvent);
        }

        private static LogLevelNLog GetLevel(LogLevel logLevel)
        {
            switch (logLevel)
            {
                case LogLevel.All: return LogLevelNLog.Trace;
                case LogLevel.Trace: return LogLevelNLog.Trace;
                case LogLevel.Debug: return LogLevelNLog.Debug;
                case LogLevel.Info: return LogLevelNLog.Info;
                case LogLevel.Warn: return LogLevelNLog.Warn;
                case LogLevel.Error: return LogLevelNLog.Error;
                case LogLevel.Fatal: return LogLevelNLog.Fatal;
                case LogLevel.Off: return LogLevelNLog.Off;
                default:
                    throw new ArgumentOutOfRangeException(nameof(logLevel), logLevel, "unknown log level");
            }
        }
    }
}
