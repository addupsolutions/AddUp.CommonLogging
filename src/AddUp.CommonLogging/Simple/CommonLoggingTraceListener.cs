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
using System.Collections.Specialized;
using System.Diagnostics;
using System.Text;

namespace AddUp.CommonLogging.Simple
{
    /// <summary>
    /// A <see cref="TraceListener"/> implementation sending all <see cref="Trace">System.Diagnostics.Trace</see> output to 
    /// the AddUp.CommonLogging infrastructure.
    /// </summary>
    /// <remarks>
    /// This listener captures all output sent by calls to <see cref="System.Diagnostics.Trace">System.Diagnostics.Trace</see> and
    /// and <see cref="TraceSource"/> and sends it to an <see cref="ILog"/> instance.<br/>
    /// The <see cref="ILog"/> instance to be used is obtained by calling
    /// <see cref="LogManager.GetLogger(string)"/>. The name of the logger is created by passing 
    /// this listener's <see cref="TraceListener.Name"/> and any <c>source</c> or <c>category</c> passed 
    /// into this listener (see <see cref="TraceListener.WriteLine(object,string)"/> or <see cref="TraceListener.TraceEvent(TraceEventCache,string,TraceEventType,int,string,object[])"/> for example).
    /// </remarks>
    /// <example>
    /// The snippet below shows how to add and configure this listener to your app.config:
    /// <code lang="XML">
    /// &lt;system.diagnostics&gt;
    ///   &lt;sharedListeners&gt;
    ///     &lt;add name=&quot;Diagnostics&quot;
    ///          type=&quot;AddUp.CommonLogging.Simple.CommonLoggingTraceListener, AddUp.CommonLogging&quot;
    ///          initializeData=&quot;DefaultTraceEventType=Information; LoggerNameFormat={listenerName}.{sourceName}&quot;&gt;
    ///       &lt;filter type=&quot;System.Diagnostics.EventTypeFilter&quot; initializeData=&quot;Information&quot;/&gt;
    ///     &lt;/add&gt;
    ///   &lt;/sharedListeners&gt;
    ///   &lt;trace&gt;
    ///     &lt;listeners&gt;
    ///       &lt;add name=&quot;Diagnostics&quot; /&gt;
    ///     &lt;/listeners&gt;
    ///   &lt;/trace&gt;
    /// &lt;/system.diagnostics&gt;
    /// </code>
    /// </example>
    /// <author>Erich Eichinger</author>
    public class CommonLoggingTraceListener : TraceListener
    {
        private int callDepth;

        /// <summary>
        /// Creates a new instance with the default name "Diagnostics" and <see cref="LogLevel"/> "Trace".
        /// </summary>
        public CommonLoggingTraceListener() : this(string.Empty) { }

        /// <summary>
        /// Creates a new instance initialized with properties from the <paramref name="initializeData"/>. string.
        /// </summary>
        /// <remarks>
        /// <paramref name="initializeData"/> is a semicolon separated string of name/value pairs, where each pair has
        /// the form <c>key=value</c>. E.g.
        /// "<c>Name=MyLoggerName;LogLevel=Debug</c>"
        /// </remarks>
        /// <param name="initializeData">a semicolon separated list of name/value pairs.</param>
        public CommonLoggingTraceListener(string initializeData) : this(GetPropertiesFromInitString(initializeData)) { }

        /// <summary>
        /// Creates a new instance initialized with the specified properties.
        /// </summary>
        /// <param name="properties">name/value configuration properties.</param>
        public CommonLoggingTraceListener(NameValueCollection properties) : base()
        {
            if (properties == null)
                properties = new NameValueCollection();

            ApplyProperties(properties);
        }

        /// <summary>
        /// Sets the default <see cref="TraceEventType"/> to use for logging
        /// all events emitted by <see cref="Trace"/><c>.Write(...)</c> and
        /// <see cref="Trace"/><c>.WriteLine(...)</c> methods.
        /// </summary>
        /// <remarks>
        /// This listener captures all output sent by calls to <see cref="System.Diagnostics.Trace"/> and
        /// sends it to an <see cref="ILog"/> instance using the <see cref="AddUp.CommonLogging.LogLevel"/> specified
        /// on <see cref="LogLevel"/>.
        /// </remarks>
        public TraceEventType DefaultTraceEventType { get; set; } = TraceEventType.Verbose;

        /// <summary>
        /// Format to use for creating the logger name. Defaults to "{listenerName}.{sourceName}".
        /// </summary>
        /// <remarks>
        /// Available placeholders are:
        /// <list type="bullet">
        /// <item>{listenerName}: the configured name of this listener instance.</item>
        /// <item>{sourceName}: the trace source name an event originates from (see e.g. <see cref="TraceListener.TraceEvent(System.Diagnostics.TraceEventCache,string,System.Diagnostics.TraceEventType,int,string,object[])"/>.</item>
        /// </list>
        /// </remarks>
        public string LoggerNameFormat { get; set; } = "{listenerName}.{sourceName}";

        /// <summary>
        /// Writes message to logger provided by <see cref="LogManager.GetLogger(string)"/>.
        /// </summary>
        public override void Write(object o)
        {
            if (Filter == null || Filter.ShouldTrace(null, Name, DefaultTraceEventType, 0, null, null, o, null))
                Log(DefaultTraceEventType, null, 0, "{0}", o);
        }

        /// <summary>
        /// Writes message to logger provided by <see cref="LogManager.GetLogger(string)"/>.
        /// </summary>
        public override void Write(object o, string category)
        {
            if (Filter == null || Filter.ShouldTrace(null, Name, DefaultTraceEventType, 0, null, null, o, null))
                Log(DefaultTraceEventType, category, 0, "{0}", o);
        }

        /// <summary>
        /// Writes message to logger provided by <see cref="LogManager.GetLogger(string)"/>.
        /// </summary>
        public override void Write(string message)
        {
            if (Filter == null || Filter.ShouldTrace(null, Name, DefaultTraceEventType, 0, null, null, null, null))
                Log(DefaultTraceEventType, null, 0, message);
        }

        /// <summary>
        /// Writes message to logger provided by <see cref="LogManager.GetLogger(string)"/>.
        /// </summary>
        public override void Write(string message, string category)
        {
            if (Filter == null || Filter.ShouldTrace(null, Name, DefaultTraceEventType, 0, null, null, null, null))
                Log(DefaultTraceEventType, category, 0, message);
        }

        /// <summary>
        /// Writes message to logger provided by <see cref="LogManager.GetLogger(string)"/>.
        /// </summary>
        public override void WriteLine(object o)
        {
            if (Filter == null || Filter.ShouldTrace(null, Name, DefaultTraceEventType, 0, null, null, o, null))
                Log(DefaultTraceEventType, null, 0, "{0}", o);
        }

        /// <summary>
        /// Writes message to logger provided by <see cref="LogManager.GetLogger(string)"/>.
        /// </summary>
        public override void WriteLine(object o, string category)
        {
            if (Filter == null || Filter.ShouldTrace(null, Name, DefaultTraceEventType, 0, null, null, o, null))
                Log(DefaultTraceEventType, category, 0, "{0}", o);
        }

        /// <summary>
        /// Writes message to logger provided by <see cref="LogManager.GetLogger(string)"/>.
        /// </summary>
        public override void WriteLine(string message)
        {
            if (Filter == null || Filter.ShouldTrace(null, Name, DefaultTraceEventType, 0, null, null, null, null))
                Log(DefaultTraceEventType, null, 0, message);
        }

        /// <summary>
        /// Writes message to logger provided by <see cref="LogManager.GetLogger(string)"/>
        /// </summary>
        public override void WriteLine(string message, string category)
        {
            if (Filter == null || Filter.ShouldTrace(null, Name, DefaultTraceEventType, 0, null, null, null, null))
                Log(DefaultTraceEventType, category, 0, message);
        }

        /// <summary>
        /// Writes message to logger provided by <see cref="LogManager.GetLogger(string)"/>
        /// </summary>
        public override void TraceEvent(TraceEventCache eventCache, string source, TraceEventType eventType, int id)
        {
            if (Filter == null || Filter.ShouldTrace(eventCache, source, eventType, id, null, null, null, null))
                Log(eventType, source, id, "Event Id {0}", id);
        }

        /// <summary>
        /// Writes message to logger provided by <see cref="LogManager.GetLogger(string)"/>
        /// </summary>
        public override void TraceEvent(TraceEventCache eventCache, string source, TraceEventType eventType, int id, string message)
        {
            if (Filter == null || Filter.ShouldTrace(eventCache, source, eventType, id, message, null, null, null))
                Log(eventType, source, id, message);
        }

        /// <summary>
        /// Writes message to logger provided by <see cref="LogManager.GetLogger(string)"/>
        /// </summary>
        public override void TraceEvent(TraceEventCache eventCache, string source, TraceEventType eventType, int id, string format, params object[] args)
        {
            if (Filter == null || Filter.ShouldTrace(eventCache, source, eventType, id, format, args, null, null))
                Log(eventType, source, id, format, args);
        }

        /// <summary>
        /// Writes message to logger provided by <see cref="LogManager.GetLogger(string)"/>
        /// </summary>
        public override void TraceData(TraceEventCache eventCache, string source, TraceEventType eventType, int id, params object[] data)
        {
            if (Filter == null || Filter.ShouldTrace(eventCache, source, eventType, id, null, null, null, data))
                Log(eventType, source, id, GetFormat(data), data);
        }

        /// <summary>
        /// Writes message to logger provided by <see cref="LogManager.GetLogger(string)"/>
        /// </summary>
        public override void TraceData(TraceEventCache eventCache, string source, TraceEventType eventType, int id, object data)
        {
            if (Filter == null || Filter.ShouldTrace(eventCache, source, eventType, id, null, null, data, null))
                Log(eventType, source, id, GetFormat(data), data);
        }

        private void ApplyProperties(NameValueCollection props)
        {
            DefaultTraceEventType = props["defaultTraceEventType"] != null
                ? (TraceEventType)Enum.Parse(typeof(TraceEventType), props["defaultTraceEventType"], true)
                : TraceEventType.Verbose;

            Name = props["name"] ?? "Diagnostics";
            LoggerNameFormat = props["loggerNameFormat"] ?? "{listenerName}.{sourceName}";
        }

        /// <summary>
        /// Logs the given message to the AddUp.CommonLogging infrastructure.
        /// </summary>
        /// <param name="eventType">the eventType</param>
        /// <param name="source">the <see cref="TraceSource"/> name or category name passed into e.g. <see cref="Trace.Write(object,string)"/>.</param>
        /// <param name="id">the id of this event</param>
        /// <param name="format">the message format</param>
        /// <param name="args">the message arguments</param>
        protected virtual void Log(TraceEventType eventType, string source, int id, string format, params object[] args)
        {
            source = LoggerNameFormat.Replace("{listenerName}", Name).Replace("{sourceName}", "" + source);

            //ensure that Log(...) isn't called recursively
            // necessary b/c Log4Net calls Trace.Write(...) during its initialization this otherwise results in a StackOverflow exception 
            // (see https://github.com/net-commons/common-logging/issues/127 for details)
            callDepth++;

            if (callDepth > 1)
                return;

            var log = LogManager.GetLogger(source);

            callDepth--;
            LogLevel logLevel = MapLogLevel(eventType);

            switch (logLevel)
            {
                case LogLevel.Trace:
                    log.TraceFormat(format, args);
                    break;
                case LogLevel.Debug:
                    log.DebugFormat(format, args);
                    break;
                case LogLevel.Info:
                    log.InfoFormat(format, args);
                    break;
                case LogLevel.Warn:
                    log.WarnFormat(format, args);
                    break;
                case LogLevel.Error:
                    log.ErrorFormat(format, args);
                    break;
                case LogLevel.Fatal:
                    log.FatalFormat(format, args);
                    break;
                case LogLevel.Off:
                    break;
                default:
                    throw new ArgumentOutOfRangeException("eventType", eventType, "invalid TraceEventType value");
            }
        }

        private static NameValueCollection GetPropertiesFromInitString(string initializeData)
        {
            var props = new NameValueCollection();
            if (initializeData == null)
                return props;

            var parts = initializeData.Split(';');
            foreach (var s in parts)
            {
                var part = s.Trim();
                if (part.Length == 0)
                    continue;

                var ixEquals = part.IndexOf('=');
                if (ixEquals > -1)
                {
                    var name = part.Substring(0, ixEquals).Trim();
                    var value = (ixEquals < part.Length - 1) ? part.Substring(ixEquals + 1) : string.Empty;
                    props[name] = value.Trim();
                }
                else props[part.Trim()] = null;
            }

            return props;
        }

        private string GetFormat(params object[] data)
        {
            if (data == null || data.Length == 0)
                return null;

            var format = new StringBuilder();
            for (var i = 0; i < data.Length; i++)
            {
                _ = format.Append('{').Append(i).Append('}');
                if (i < data.Length - 1)
                    _ = format.Append(',');
            }

            return format.ToString();
        }

        private LogLevel MapLogLevel(TraceEventType eventType)
        {
            switch (eventType)
            {
                case TraceEventType.Start:
                case TraceEventType.Stop:
                case TraceEventType.Suspend:
                case TraceEventType.Resume:
                case TraceEventType.Transfer:
                    return LogLevel.Trace;
                case TraceEventType.Verbose:
                    return LogLevel.Debug;
                case TraceEventType.Information:
                    return LogLevel.Info;
                case TraceEventType.Warning:
                    return LogLevel.Warn;
                case TraceEventType.Error:
                    return LogLevel.Error;
                case TraceEventType.Critical:
                    return LogLevel.Fatal;
                default:
                    return LogLevel.Trace;
            }
        }
    }
}