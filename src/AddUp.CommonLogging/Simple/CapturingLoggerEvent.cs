using System;

namespace AddUp.CommonLogging.Simple
{
    /// <summary>
    /// A logging event captured by <see cref="CapturingLogger"/>
    /// </summary>
    /// <author>Erich Eichinger</author>
    public class CapturingLoggerEvent
    {
        public CapturingLoggerEvent(CapturingLogger source, LogLevel level, object messageObject, Exception exception)
        {
            Source = source;
            Level = level;
            MessageObject = messageObject;
            Exception = exception;
        }

        /// <summary>
        /// The logger that logged this event
        /// </summary>
        public CapturingLogger Source { get; }
        
        /// <summary>
        /// The level used to log this event
        /// </summary>
        public LogLevel Level { get; }
        
        /// <summary>
        /// The raw message object
        /// </summary>
        public object MessageObject { get; }
        
        /// <summary>
        /// A logged exception
        /// </summary>
        public Exception Exception { get; }

        /// <summary>
        /// Retrieves the formatted message text
        /// </summary>
        public string RenderedMessage => MessageObject.ToString();
    }
}