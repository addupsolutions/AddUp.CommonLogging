using System;
using System.Collections.Generic;
using AddUp.CommonLogging.Configuration;

namespace AddUp.CommonLogging.Simple
{
    /// <summary>
    /// A logger created by <see cref="CapturingLoggerFactoryAdapter"/> that 
    /// sends all log events to the owning adapter's <see cref="CapturingLoggerFactoryAdapter.AddEvent"/>
    /// </summary>
    /// <author>Erich Eichinger</author>
    public class CapturingLogger : AbstractSimpleLogger
    {
        private volatile CapturingLoggerEvent lastEvent;

        public CapturingLogger(CapturingLoggerFactoryAdapter owner, string logName) : base(logName, LogLevel.All, true, true, true, null)
        {
            _ = ArgUtils.AssertNotNull("owner", owner);
            Owner = owner;
        }

        /// <summary>
        /// Holds the list of logged events.
        /// </summary>
        /// <remarks>
        /// To access this collection in a multithreaded application, put a lock on the list instance.
        /// </remarks>
        public IList<CapturingLoggerEvent> LoggerEvents { get; } = new List<CapturingLoggerEvent>();

        /// <summary>
        /// The adapter that created this logger instance.
        /// </summary>
        public CapturingLoggerFactoryAdapter Owner { get; }

        /// <summary>
        /// Holds the last log event received from any of this adapter's loggers.
        /// </summary>
        public CapturingLoggerEvent LastEvent => lastEvent;

        /// <summary>
        /// Clears all captured events
        /// </summary>
        public void Clear()
        {
            lock (LoggerEvents)
            {
                ClearLastEvent();
                LoggerEvents.Clear();
            }
        }

        /// <summary>
        /// Resets the <see cref="LastEvent"/> to <c>null</c>.
        /// </summary>
        public void ClearLastEvent() => lastEvent = null;

        /// <summary>
        /// <see cref="CapturingLogger"/> instances send their captured log events to this method.
        /// </summary>
        public virtual void AddEvent(CapturingLoggerEvent loggerEvent)
        {
            lastEvent = loggerEvent;
            lock (LoggerEvents)
                LoggerEvents.Add(loggerEvent);

            Owner.AddEvent(LastEvent);
        }

        /// <summary>
        /// Create a new <see cref="CapturingLoggerEvent"/> and send it to <see cref="CapturingLoggerFactoryAdapter.AddEvent"/>
        /// </summary>
        /// <param name="level"></param>
        /// <param name="message"></param>
        /// <param name="exception"></param>
        protected override void WriteInternal(LogLevel level, object message, Exception exception)
        {
            CapturingLoggerEvent ev = new CapturingLoggerEvent(this, level, message, exception);
            AddEvent(ev);
        }
    }
}