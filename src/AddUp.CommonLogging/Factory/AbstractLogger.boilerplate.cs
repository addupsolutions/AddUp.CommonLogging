using System;
using FormatMessageCallback = System.Action<AddUp.CommonLogging.FormatMessageHandler>;

namespace AddUp.CommonLogging.Factory
{
    partial class AbstractLogger
    {
        public abstract bool IsTraceEnabled { get; }
        public abstract bool IsDebugEnabled { get; }
        public abstract bool IsInfoEnabled { get; }
        public abstract bool IsWarnEnabled { get; }
        public abstract bool IsErrorEnabled { get; }
        public abstract bool IsFatalEnabled { get; }

        public virtual void Trace(object message)
        {
            if (IsTraceEnabled)
                writeHandler(LogLevel.Trace, message, null);
        }

        public virtual void Trace(object message, Exception exception)
        {
            if (IsTraceEnabled)
                writeHandler(LogLevel.Trace, message, exception);
        }

        public virtual void Trace(FormatMessageCallback formatMessageCallback)
        {
            if (IsTraceEnabled)
                writeHandler(LogLevel.Trace, new FormatMessageCallbackFormattedMessage(formatMessageCallback), null);
        }

        public virtual void Trace(FormatMessageCallback formatMessageCallback, Exception exception)
        {
            if (IsTraceEnabled)
                writeHandler(LogLevel.Trace, new FormatMessageCallbackFormattedMessage(formatMessageCallback), exception);
        }

        public virtual void Trace(IFormatProvider formatProvider, FormatMessageCallback formatMessageCallback)
        {
            if (IsTraceEnabled)
                writeHandler(LogLevel.Trace, new FormatMessageCallbackFormattedMessage(formatProvider, formatMessageCallback), null);
        }

        public virtual void Trace(IFormatProvider formatProvider, FormatMessageCallback formatMessageCallback, Exception exception)
        {
            if (IsTraceEnabled)
                writeHandler(LogLevel.Trace, new FormatMessageCallbackFormattedMessage(formatProvider, formatMessageCallback), exception);
        }

        public virtual void TraceFormat(IFormatProvider formatProvider, string format, params object[] args)
        {
            if (IsTraceEnabled)
                writeHandler(LogLevel.Trace, new StringFormatFormattedMessage(formatProvider, format, args), null);
        }

        public virtual void TraceFormat(IFormatProvider formatProvider, string format, Exception exception, params object[] args)
        {
            if (IsTraceEnabled)
                writeHandler(LogLevel.Trace, new StringFormatFormattedMessage(formatProvider, format, args), exception);
        }

        public virtual void TraceFormat(string format, params object[] args)
        {
            if (IsTraceEnabled)
                writeHandler(LogLevel.Trace, new StringFormatFormattedMessage(null, format, args), null);
        }

        public virtual void TraceFormat(string format, Exception exception, params object[] args)
        {
            if (IsTraceEnabled)
                writeHandler(LogLevel.Trace, new StringFormatFormattedMessage(null, format, args), exception);
        }

        public virtual void Debug(object message)
        {
            if (IsDebugEnabled)
                writeHandler(LogLevel.Debug, message, null);
        }

        public virtual void Debug(object message, Exception exception)
        {
            if (IsDebugEnabled)
                writeHandler(LogLevel.Debug, message, exception);
        }

        public virtual void Debug(FormatMessageCallback formatMessageCallback)
        {
            if (IsDebugEnabled)
                writeHandler(LogLevel.Debug, new FormatMessageCallbackFormattedMessage(formatMessageCallback), null);
        }

        public virtual void Debug(FormatMessageCallback formatMessageCallback, Exception exception)
        {
            if (IsDebugEnabled)
                writeHandler(LogLevel.Debug, new FormatMessageCallbackFormattedMessage(formatMessageCallback), exception);
        }

        public virtual void Debug(IFormatProvider formatProvider, FormatMessageCallback formatMessageCallback)
        {
            if (IsDebugEnabled)
                writeHandler(LogLevel.Debug, new FormatMessageCallbackFormattedMessage(formatProvider, formatMessageCallback), null);
        }

        public virtual void Debug(IFormatProvider formatProvider, FormatMessageCallback formatMessageCallback, Exception exception)
        {
            if (IsDebugEnabled)
                writeHandler(LogLevel.Debug, new FormatMessageCallbackFormattedMessage(formatProvider, formatMessageCallback), exception);
        }

        public virtual void DebugFormat(IFormatProvider formatProvider, string format, params object[] args)
        {
            if (IsDebugEnabled)
                writeHandler(LogLevel.Debug, new StringFormatFormattedMessage(formatProvider, format, args), null);
        }

        public virtual void DebugFormat(IFormatProvider formatProvider, string format, Exception exception, params object[] args)
        {
            if (IsDebugEnabled)
                writeHandler(LogLevel.Debug, new StringFormatFormattedMessage(formatProvider, format, args), exception);
        }

        public virtual void DebugFormat(string format, params object[] args)
        {
            if (IsDebugEnabled)
                writeHandler(LogLevel.Debug, new StringFormatFormattedMessage(null, format, args), null);
        }

        public virtual void DebugFormat(string format, Exception exception, params object[] args)
        {
            if (IsDebugEnabled)
                writeHandler(LogLevel.Debug, new StringFormatFormattedMessage(null, format, args), exception);
        }

        public virtual void Info(object message)
        {
            if (IsInfoEnabled)
                writeHandler(LogLevel.Info, message, null);
        }

        public virtual void Info(object message, Exception exception)
        {
            if (IsInfoEnabled)
                writeHandler(LogLevel.Info, message, exception);
        }

        public virtual void Info(FormatMessageCallback formatMessageCallback)
        {
            if (IsInfoEnabled)
                writeHandler(LogLevel.Info, new FormatMessageCallbackFormattedMessage(formatMessageCallback), null);
        }

        public virtual void Info(FormatMessageCallback formatMessageCallback, Exception exception)
        {
            if (IsInfoEnabled)
                writeHandler(LogLevel.Info, new FormatMessageCallbackFormattedMessage(formatMessageCallback), exception);
        }

        public virtual void Info(IFormatProvider formatProvider, FormatMessageCallback formatMessageCallback)
        {
            if (IsInfoEnabled)
                writeHandler(LogLevel.Info, new FormatMessageCallbackFormattedMessage(formatProvider, formatMessageCallback), null);
        }

        public virtual void Info(IFormatProvider formatProvider, FormatMessageCallback formatMessageCallback, Exception exception)
        {
            if (IsInfoEnabled)
                writeHandler(LogLevel.Info, new FormatMessageCallbackFormattedMessage(formatProvider, formatMessageCallback), exception);
        }

        public virtual void InfoFormat(IFormatProvider formatProvider, string format, params object[] args)
        {
            if (IsInfoEnabled)
                writeHandler(LogLevel.Info, new StringFormatFormattedMessage(formatProvider, format, args), null);
        }

        public virtual void InfoFormat(IFormatProvider formatProvider, string format, Exception exception, params object[] args)
        {
            if (IsInfoEnabled)
                writeHandler(LogLevel.Info, new StringFormatFormattedMessage(formatProvider, format, args), exception);
        }

        public virtual void InfoFormat(string format, params object[] args)
        {
            if (IsInfoEnabled)
                writeHandler(LogLevel.Info, new StringFormatFormattedMessage(null, format, args), null);
        }

        public virtual void InfoFormat(string format, Exception exception, params object[] args)
        {
            if (IsInfoEnabled)
                writeHandler(LogLevel.Info, new StringFormatFormattedMessage(null, format, args), exception);
        }

        public virtual void Warn(object message)
        {
            if (IsWarnEnabled)
                writeHandler(LogLevel.Warn, message, null);
        }

        public virtual void Warn(object message, Exception exception)
        {
            if (IsWarnEnabled)
                writeHandler(LogLevel.Warn, message, exception);
        }

        public virtual void Warn(FormatMessageCallback formatMessageCallback)
        {
            if (IsWarnEnabled)
                writeHandler(LogLevel.Warn, new FormatMessageCallbackFormattedMessage(formatMessageCallback), null);
        }

        public virtual void Warn(FormatMessageCallback formatMessageCallback, Exception exception)
        {
            if (IsWarnEnabled)
                writeHandler(LogLevel.Warn, new FormatMessageCallbackFormattedMessage(formatMessageCallback), exception);
        }

        public virtual void Warn(IFormatProvider formatProvider, FormatMessageCallback formatMessageCallback)
        {
            if (IsWarnEnabled)
                writeHandler(LogLevel.Warn, new FormatMessageCallbackFormattedMessage(formatProvider, formatMessageCallback), null);
        }

        public virtual void Warn(IFormatProvider formatProvider, FormatMessageCallback formatMessageCallback, Exception exception)
        {
            if (IsWarnEnabled)
                writeHandler(LogLevel.Warn, new FormatMessageCallbackFormattedMessage(formatProvider, formatMessageCallback), exception);
        }

        public virtual void WarnFormat(IFormatProvider formatProvider, string format, params object[] args)
        {
            if (IsWarnEnabled)
                writeHandler(LogLevel.Warn, new StringFormatFormattedMessage(formatProvider, format, args), null);
        }

        public virtual void WarnFormat(IFormatProvider formatProvider, string format, Exception exception, params object[] args)
        {
            if (IsWarnEnabled)
                writeHandler(LogLevel.Warn, new StringFormatFormattedMessage(formatProvider, format, args), exception);
        }

        public virtual void WarnFormat(string format, params object[] args)
        {
            if (IsWarnEnabled)
                writeHandler(LogLevel.Warn, new StringFormatFormattedMessage(null, format, args), null);
        }

        public virtual void WarnFormat(string format, Exception exception, params object[] args)
        {
            if (IsWarnEnabled)
                writeHandler(LogLevel.Warn, new StringFormatFormattedMessage(null, format, args), exception);
        }

        public virtual void Error(object message)
        {
            if (IsErrorEnabled)
                writeHandler(LogLevel.Error, message, null);
        }

        public virtual void Error(object message, Exception exception)
        {
            if (IsErrorEnabled)
                writeHandler(LogLevel.Error, message, exception);
        }

        public virtual void Error(FormatMessageCallback formatMessageCallback)
        {
            if (IsErrorEnabled)
                writeHandler(LogLevel.Error, new FormatMessageCallbackFormattedMessage(formatMessageCallback), null);
        }

        public virtual void Error(FormatMessageCallback formatMessageCallback, Exception exception)
        {
            if (IsErrorEnabled)
                writeHandler(LogLevel.Error, new FormatMessageCallbackFormattedMessage(formatMessageCallback), exception);
        }

        public virtual void Error(IFormatProvider formatProvider, FormatMessageCallback formatMessageCallback)
        {
            if (IsErrorEnabled)
                writeHandler(LogLevel.Error, new FormatMessageCallbackFormattedMessage(formatProvider, formatMessageCallback), null);
        }

        public virtual void Error(IFormatProvider formatProvider, FormatMessageCallback formatMessageCallback, Exception exception)
        {
            if (IsErrorEnabled)
                writeHandler(LogLevel.Error, new FormatMessageCallbackFormattedMessage(formatProvider, formatMessageCallback), exception);
        }

        public virtual void ErrorFormat(IFormatProvider formatProvider, string format, params object[] args)
        {
            if (IsErrorEnabled)
                writeHandler(LogLevel.Error, new StringFormatFormattedMessage(formatProvider, format, args), null);
        }

        public virtual void ErrorFormat(IFormatProvider formatProvider, string format, Exception exception, params object[] args)
        {
            if (IsErrorEnabled)
                writeHandler(LogLevel.Error, new StringFormatFormattedMessage(formatProvider, format, args), exception);
        }

        public virtual void ErrorFormat(string format, params object[] args)
        {
            if (IsErrorEnabled)
                writeHandler(LogLevel.Error, new StringFormatFormattedMessage(null, format, args), null);
        }

        public virtual void ErrorFormat(string format, Exception exception, params object[] args)
        {
            if (IsErrorEnabled)
                writeHandler(LogLevel.Error, new StringFormatFormattedMessage(null, format, args), exception);
        }

        public virtual void Fatal(object message)
        {
            if (IsFatalEnabled)
                writeHandler(LogLevel.Fatal, message, null);
        }

        public virtual void Fatal(object message, Exception exception)
        {
            if (IsFatalEnabled)
                writeHandler(LogLevel.Fatal, message, exception);
        }

        public virtual void Fatal(FormatMessageCallback formatMessageCallback)
        {
            if (IsFatalEnabled)
                writeHandler(LogLevel.Fatal, new FormatMessageCallbackFormattedMessage(formatMessageCallback), null);
        }

        public virtual void Fatal(FormatMessageCallback formatMessageCallback, Exception exception)
        {
            if (IsFatalEnabled)
                writeHandler(LogLevel.Fatal, new FormatMessageCallbackFormattedMessage(formatMessageCallback), exception);
        }

        public virtual void Fatal(IFormatProvider formatProvider, FormatMessageCallback formatMessageCallback)
        {
            if (IsFatalEnabled)
                writeHandler(LogLevel.Fatal, new FormatMessageCallbackFormattedMessage(formatProvider, formatMessageCallback), null);
        }

        public virtual void Fatal(IFormatProvider formatProvider, FormatMessageCallback formatMessageCallback, Exception exception)
        {
            if (IsFatalEnabled)
                writeHandler(LogLevel.Fatal, new FormatMessageCallbackFormattedMessage(formatProvider, formatMessageCallback), exception);
        }

        public virtual void FatalFormat(IFormatProvider formatProvider, string format, params object[] args)
        {
            if (IsFatalEnabled)
                writeHandler(LogLevel.Fatal, new StringFormatFormattedMessage(formatProvider, format, args), null);
        }

        public virtual void FatalFormat(IFormatProvider formatProvider, string format, Exception exception, params object[] args)
        {
            if (IsFatalEnabled)
                writeHandler(LogLevel.Fatal, new StringFormatFormattedMessage(formatProvider, format, args), exception);
        }

        public virtual void FatalFormat(string format, params object[] args)
        {
            if (IsFatalEnabled)
                writeHandler(LogLevel.Fatal, new StringFormatFormattedMessage(null, format, args), null);
        }

        public virtual void FatalFormat(string format, Exception exception, params object[] args)
        {
            if (IsFatalEnabled)
                writeHandler(LogLevel.Fatal, new StringFormatFormattedMessage(null, format, args), exception);
        }
    }
}
