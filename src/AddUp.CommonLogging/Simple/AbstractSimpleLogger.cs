#region License

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
using System.Globalization;
using System.Text;
using AddUp.CommonLogging.Factory;

namespace AddUp.CommonLogging.Simple
{
    /// <summary>
    /// Abstract class providing a standard implementation of simple loggers.
    /// </summary>
    /// <author>Erich Eichinger</author>
    [Serializable]
    public abstract class AbstractSimpleLogger : AbstractLogger
    {
        /// <summary>
        /// Creates and initializes a the simple logger.
        /// </summary>
        /// <param name="logName">The key, usually type key of the calling class, of the logger.</param>
        /// <param name="logLevel">The current logging threshold. Messages recieved that are beneath this threshold will not be logged.</param>
        /// <param name="showlevel">Include level in the log message.</param>
        /// <param name="showDateTime">Include the current time in the log message.</param>
        /// <param name="showLogName">Include the instance key in the log message.</param>
        /// <param name="dateTimeFormat">The date and time format to use in the log message.</param>
        protected AbstractSimpleLogger(string logName, LogLevel logLevel, bool showlevel, bool showDateTime, bool showLogName, string dateTimeFormat)
        {
            Name = logName;
            CurrentLogLevel = logLevel;
            ShowLevel = showlevel;
            ShowDateTime = showDateTime;
            ShowLogName = showLogName;
            DateTimeFormat = dateTimeFormat;
            HasDateTimeFormat = !string.IsNullOrEmpty(DateTimeFormat);
        }

        /// <summary>
        /// The key of the logger.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Include the current log level in the log message.
        /// </summary>
        public bool ShowLevel { get; }

        /// <summary>
        /// Include the current time in the log message.
        /// </summary>
        public bool ShowDateTime { get; }

        /// <summary>
        /// Include the instance key in the log message.
        /// </summary>
        public bool ShowLogName { get; }

        /// <summary>
        /// The current logging threshold. Messages recieved that are beneath this threshold will not be logged.
        /// </summary>
        public LogLevel CurrentLogLevel { get; set; }

        /// <summary>
        /// The date and time format to use in the log message.
        /// </summary>
        public string DateTimeFormat { get; }

        /// <summary>
        /// Determines Whether <see cref="DateTimeFormat"/> is set.
        /// </summary>
        public bool HasDateTimeFormat { get; }

        public override bool IsTraceEnabled => IsLevelEnabled(LogLevel.Trace);
        public override bool IsDebugEnabled => IsLevelEnabled(LogLevel.Debug);
        public override bool IsInfoEnabled => IsLevelEnabled(LogLevel.Info);
        public override bool IsWarnEnabled => IsLevelEnabled(LogLevel.Warn);
        public override bool IsErrorEnabled => IsLevelEnabled(LogLevel.Error);
        public override bool IsFatalEnabled => IsLevelEnabled(LogLevel.Fatal);

        /// <summary>
        /// Appends the formatted message to the specified <see cref="StringBuilder"/>.
        /// </summary>
        /// <param name="stringBuilder">the <see cref="StringBuilder"/> that receíves the formatted message.</param>
        /// <param name="level"></param>
        /// <param name="message"></param>
        /// <param name="exception"></param>
        protected virtual void FormatOutput(StringBuilder stringBuilder, LogLevel level, object message, Exception exception)
        {
            if (stringBuilder == null) throw new ArgumentNullException(nameof(stringBuilder));

            if (ShowDateTime) // Append date-time if so configured
            {
                _ = HasDateTimeFormat
                    ? stringBuilder.Append(DateTime.Now.ToString(DateTimeFormat, CultureInfo.InvariantCulture))
                    : stringBuilder.Append(DateTime.Now);

                _ = stringBuilder.Append(' ');
            }

            if (ShowLevel) // Append a readable representation of the log level
                _ = stringBuilder.Append(("[" + level.ToString().ToUpper() + "]").PadRight(8));

            if (ShowLogName) // Append the key of the log instance if so configured
                _ = stringBuilder.Append(Name).Append(" - ");

            // Append the message
            _ = stringBuilder.Append(message);

            // Append stack trace if not null
            if (exception != null)
                _ = stringBuilder.Append(Environment.NewLine).Append(ExceptionFormatter.Format(exception));
        }

        /// <summary>
        /// Determines if the given log level is currently enabled.
        /// </summary>
        /// <param name="level"></param>
        /// <returns></returns>
        protected virtual bool IsLevelEnabled(LogLevel level)
        {
            var iLevel = (int)level;
            var iCurrentLogLevel = (int)CurrentLogLevel;

            return iLevel >= iCurrentLogLevel;
        }
    }
}
