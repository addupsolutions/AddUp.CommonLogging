#region License

/*
 * Copyright � 2002-2009 the original author or authors.
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
using System.Text;

namespace AddUp.CommonLogging.Simple
{
    /// <summary>
    /// Sends log messages to <see cref="Console.Out" />.
    /// </summary>
    /// <author>Gilles Bayon</author>
    [Serializable]
    public class ConsoleOutLogger : AbstractSimpleLogger
    {
        private static readonly Dictionary<LogLevel, ConsoleColor> colors = new Dictionary<LogLevel, ConsoleColor>
        {
            { LogLevel.Fatal, ConsoleColor.Red },
            { LogLevel.Error, ConsoleColor.Yellow },
            { LogLevel.Warn, ConsoleColor.Magenta },
            { LogLevel.Info, ConsoleColor.White },
            { LogLevel.Debug, ConsoleColor.Gray },
            { LogLevel.Trace, ConsoleColor.DarkGray },
        };

        /// <summary>
        /// Creates and initializes a logger that writes messages to <see cref="Console.Out" />.
        /// </summary>
        /// <param name="logName">The name, usually type name of the calling class, of the logger.</param>
        /// <param name="logLevel">The current logging threshold. Messages recieved that are beneath this threshold will not be logged.</param>
        /// <param name="showLevel">Include the current log level in the log message.</param>
        /// <param name="showDateTime">Include the current time in the log message.</param>
        /// <param name="showLogName">Include the instance name in the log message.</param>
        /// <param name="dateTimeFormat">The date and time format to use in the log message.</param>
        /// <param name="useColor">Use color when writing the log message.</param>
        public ConsoleOutLogger(string logName, LogLevel logLevel, bool showLevel, bool showDateTime, bool showLogName, string dateTimeFormat, bool useColor)
            : this(logName, logLevel, showLevel, showDateTime, showLogName, dateTimeFormat) => UseColor = useColor;

        /// <summary>
        /// Creates and initializes a logger that writes messages to <see cref="Console.Out" />.
        /// </summary>
        /// <param name="logName">The name, usually type name of the calling class, of the logger.</param>
        /// <param name="logLevel">The current logging threshold. Messages recieved that are beneath this threshold will not be logged.</param>
        /// <param name="showLevel">Include the current log level in the log message.</param>
        /// <param name="showDateTime">Include the current time in the log message.</param>
        /// <param name="showLogName">Include the instance name in the log message.</param>
        /// <param name="dateTimeFormat">The date and time format to use in the log message.</param>
        public ConsoleOutLogger(string logName, LogLevel logLevel, bool showLevel, bool showDateTime, bool showLogName, string dateTimeFormat)
            : base(logName, logLevel, showLevel, showDateTime, showLogName, dateTimeFormat)
        { }

        private bool UseColor { get; }

        /// <summary>
        /// Do the actual logging by constructing the log message using a <see cref="StringBuilder" /> then
        /// sending the output to <see cref="Console.Out" />.
        /// </summary>
        /// <param name="level">The <see cref="LogLevel" /> of the message.</param>
        /// <param name="message">The log message.</param>
        /// <param name="exception">An optional <see cref="Exception" /> associated with the message.</param>
        protected override void WriteInternal(LogLevel level, object message, Exception exception)
        {
            var builder = new StringBuilder();
            FormatOutput(builder, level, message, exception);

            // Print to the appropriate destination
            if (UseColor && colors.TryGetValue(level, out ConsoleColor color))
            {
                var originalColor = Console.ForegroundColor;
                try
                {
                    Console.ForegroundColor = color;
                    Console.Out.WriteLine(builder.ToString());
                }
                finally
                {
                    Console.ForegroundColor = originalColor;
                }
            }
            else Console.Out.WriteLine(builder.ToString());
        }
    }
}