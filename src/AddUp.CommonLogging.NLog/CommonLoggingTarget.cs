#region License

/*
 * Copyright 2002-2009 the original author or authors.
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
using System.Collections.Generic;
using System.Configuration;
using AddUp.CommonLogging.Configuration;
using NLog;
using NLog.Layouts;
using NLog.Targets;

namespace AddUp.CommonLogging.NLog
{
    /// <summary>
    /// Routes all log events logged through NLog into the AddUp.CommonLogging infrastructure.
    /// </summary>
    /// <remarks>
    /// <example>
    /// To route all NLog events to AddUp.CommonLogging, you must add this target to your configuration:
    /// <code>
    /// LoggingConfiguration cfg = new LoggingConfiguration();
    /// CommonLoggingTarget target = new CommonLoggingTarget(&quot;${level:uppercase=true}|${logger}|${message}&quot;);
    /// cfg.LoggingRules.Add(new LoggingRule(&quot;*&quot;, LogLevel.Trace, target));
    /// 
    /// LogManager.Configuration = cfg;
    /// 
    /// Logger log = LogManager.GetLogger(&quot;mylogger&quot;);
    /// log.Debug(&quot;some message&quot;);
    /// </code>
    /// </example>
    /// </remarks>
    /// <author>Erich Eichinger</author>
    public class CommonLoggingTarget : TargetWithLayout
    {
        private delegate string MessageFormatter();
        private delegate void LogMethod(ILog logger, MessageFormatter fmtr, Exception exception);

        private static readonly Dictionary<global::NLog.LogLevel, LogMethod> logMethods = new Dictionary<global::NLog.LogLevel, LogMethod>
        {
            [global::NLog.LogLevel.Trace] = (log, msg, ex) => log.Trace(m => m(msg()), ex),
            [global::NLog.LogLevel.Debug] = (log, msg, ex) => log.Debug(m => m(msg()), ex),
            [global::NLog.LogLevel.Info] = (log, msg, ex) => log.Info(m => m(msg()), ex),
            [global::NLog.LogLevel.Warn] = (log, msg, ex) => log.Warn(m => m(msg()), ex),
            [global::NLog.LogLevel.Error] = (log, msg, ex) => log.Error(m => m(msg()), ex),
            [global::NLog.LogLevel.Fatal] = (log, msg, ex) => log.Fatal(m => m(msg()), ex),
            [global::NLog.LogLevel.Off] = (log, msg, ex) => { }
        };

        public CommonLoggingTarget() { }

        public CommonLoggingTarget(string layout)
        {
            _ = ArgUtils.AssertNotNull(nameof(layout), layout);
            Layout = layout;
        }

        /// <summary>
        /// Writes the event to the AddUp.CommonLogging infrastructure
        /// </summary>
        protected override void Write(LogEventInfo logEvent)
        {
            if (LogManager.Adapter is NLogLoggerFactoryAdapter)
                throw new ConfigurationErrorsException("routing NLog events to AddUp.CommonLogging configured with NLogLoggerFactoryAdapter results in an endless recursion");

            var logger = LogManager.GetLogger(logEvent.LoggerName);
            var log = logMethods[logEvent.Level];
            log(logger, () => Layout.Render(logEvent), logEvent.Exception);
        }
    }
}