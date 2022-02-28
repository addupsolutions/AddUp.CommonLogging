﻿#region License

/*
 * Copyright © 2002-2009 the original author or authors.
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
using AddUp.CommonLogging.Configuration;
using AddUp.CommonLogging.Factory;

namespace AddUp.CommonLogging.Simple
{
    /// <summary>
    /// Base factory implementation for creating simple <see cref="ILog" /> instances.
    /// </summary>
    /// <remarks>Default settings are LogLevel.All, showDateTime = true, showLogName = true, and no DateTimeFormat.
    /// The keys in the NameValueCollection to configure this adapter are the following
    /// <list type="bullet">
    ///     <item>level</item>
    ///     <item>showDateTime</item>
    ///     <item>showLogName</item>
    ///     <item>dateTimeFormat</item>
    /// </list>
    /// <example>
    /// Here is an example how to implement your own logging adapter:
    /// <code>
    /// public class ConsoleOutLogger : AbstractSimpleLogger
    /// {
    ///   public ConsoleOutLogger(string logName, LogLevel logLevel, bool showLevel, bool showDateTime, 
    /// bool showLogName, string dateTimeFormat)
    ///       : base(logName, logLevel, showLevel, showDateTime, showLogName, dateTimeFormat)
    ///   {
    ///   }
    /// 
    ///   protected override void WriteInternal(LogLevel level, object message, Exception e)
    ///   {
    ///       // Use a StringBuilder for better performance
    ///       StringBuilder sb = new StringBuilder();
    ///       FormatOutput(sb, level, message, e);
    /// 
    ///       // Print to the appropriate destination
    ///       Console.Out.WriteLine(sb.ToString());
    ///   }
    /// }
    /// 
    /// public class ConsoleOutLoggerFactoryAdapter : AbstractSimpleLoggerFactoryAdapter
    /// {
    ///   public ConsoleOutLoggerFactoryAdapter(NameValueCollection properties)
    ///       : base(properties)
    ///   { }
    /// 
    ///   protected override ILog CreateLogger(string key, LogLevel level, bool showLevel, bool 
    /// showDateTime, bool showLogName, string dateTimeFormat)
    ///   {
    ///       ILog log = new ConsoleOutLogger(key, level, showLevel, showDateTime, showLogName, 
    /// dateTimeFormat);
    ///       return log;
    ///   }
    /// }
    /// </code>
    /// </example>
    /// </remarks>
    /// <author>Gilles Bayon</author>
    /// <author>Mark Pollack</author>
    /// <author>Erich Eichinger</author>
    [Serializable]
    public abstract class AbstractSimpleLoggerFactoryAdapter : AbstractCachingLoggerFactoryAdapter
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AbstractSimpleLoggerFactoryAdapter"/> class.
        /// </summary>
        /// <remarks>
        /// Looks for level, showDateTime, showLogName, dateTimeFormat items from 
        /// <paramref key="properties" /> for use when the GetLogger methods are called.
        /// <c>System.Configuration.ConfigurationManager</c> for more information on how to use the 
        /// standard .NET application configuration file (App.config/Web.config) 
        /// to configure this adapter.
        /// </remarks>
        /// <param name="properties">The key value collection, typically specified by the user in 
        /// a configuration section named common/logging.</param>
        protected AbstractSimpleLoggerFactoryAdapter(NameValueCollection properties) : this(
            ArgUtils.TryParseEnum(LogLevel.All, ArgUtils.GetValue(properties, "level")),
            ArgUtils.TryParse(true, ArgUtils.GetValue(properties, "showDateTime")),
            ArgUtils.TryParse(true, ArgUtils.GetValue(properties, "showLogName")),
            ArgUtils.TryParse(true, ArgUtils.GetValue(properties, "showLevel")),
            ArgUtils.GetValue(properties, "dateTimeFormat", string.Empty))
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="AbstractSimpleLoggerFactoryAdapter"/> class with 
        /// default settings for the loggers created by this factory.
        /// </summary>
        protected AbstractSimpleLoggerFactoryAdapter(LogLevel level, bool showDateTime, bool showLogName, bool showLevel, string dateTimeFormat)
            : base(true)
        {
            Level = level;
            ShowDateTime = showDateTime;
            ShowLogName = showLogName;
            ShowLevel = showLevel;
            DateTimeFormat = dateTimeFormat ?? string.Empty;
        }

        /// <summary>
        /// The default <see cref="LogLevel"/> to use when creating new <see cref="ILog"/> instances.
        /// </summary>
        public LogLevel Level { get; set; }

        /// <summary>
        /// The default setting to use when creating new <see cref="ILog"/> instances.
        /// </summary>
        public bool ShowLevel { get; set; }

        /// <summary>
        /// The default setting to use when creating new <see cref="ILog"/> instances.
        /// </summary>
        public bool ShowDateTime { get; set; }

        /// <summary>
        /// The default setting to use when creating new <see cref="ILog"/> instances.
        /// </summary>
        public bool ShowLogName { get; set; }

        /// <summary>
        /// The default setting to use when creating new <see cref="ILog"/> instances.
        /// </summary>
        public string DateTimeFormat { get; set; }

        /// <summary>
        /// Create the specified logger instance
        /// </summary>
        protected override ILog CreateLogger(string name) => CreateLogger(name, Level, ShowLevel, ShowDateTime, ShowLogName, DateTimeFormat);

        /// <summary>
        /// Derived factories need to implement this method to create the
        /// actual logger instance.
        /// </summary>
        /// <returns>a new logger instance. Must never be <c>null</c>!</returns>
        protected abstract ILog CreateLogger(string name, LogLevel level, bool showLevel, bool showDateTime, bool showLogName, string dateTimeFormat);
    }
}
