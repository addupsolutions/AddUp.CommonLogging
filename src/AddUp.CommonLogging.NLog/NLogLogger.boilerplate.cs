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
using FormatMessageCallback = System.Action<AddUp.CommonLogging.FormatMessageHandler>;

namespace AddUp.CommonLogging.NLog
{
    partial class NLogLogger
    {
        public override bool IsTraceEnabled => logger.IsTraceEnabled;
        public override bool IsDebugEnabled => logger.IsDebugEnabled;
        public override bool IsInfoEnabled => logger.IsInfoEnabled;
        public override bool IsWarnEnabled => logger.IsWarnEnabled;
        public override bool IsErrorEnabled => logger.IsErrorEnabled;
        public override bool IsFatalEnabled => logger.IsFatalEnabled;

        public override void Trace(object message)
        {
            if (IsTraceEnabled)
                WriteInternal(LogLevel.Trace, message, null);
        }

        public override void Trace(object message, Exception exception)
        {
            if (IsTraceEnabled)
                WriteInternal(LogLevel.Trace, message, exception);
        }

        public override void Trace(FormatMessageCallback formatMessageCallback)
        {
            if (IsTraceEnabled)
                WriteInternal(LogLevel.Trace, new FormatMessageCallbackFormattedMessage(formatMessageCallback), null);
        }

        public override void Trace(FormatMessageCallback formatMessageCallback, Exception exception)
        {
            if (IsTraceEnabled)
                WriteInternal(LogLevel.Trace, new FormatMessageCallbackFormattedMessage(formatMessageCallback), exception);
        }

        public override void Trace(IFormatProvider formatProvider, FormatMessageCallback formatMessageCallback)
        {
            if (IsTraceEnabled)
                WriteInternal(LogLevel.Trace, new FormatMessageCallbackFormattedMessage(formatProvider, formatMessageCallback), null);
        }

        public override void Trace(IFormatProvider formatProvider, FormatMessageCallback formatMessageCallback, Exception exception)
        {
            if (IsTraceEnabled)
                WriteInternal(LogLevel.Trace, new FormatMessageCallbackFormattedMessage(formatProvider, formatMessageCallback), exception);
        }

        public override void TraceFormat(IFormatProvider formatProvider, string format, params object[] args)
        {
            if (IsTraceEnabled)
                WriteInternal(LogLevel.Trace, new StringFormatFormattedMessage(formatProvider, format, args), null);
        }

        public override void TraceFormat(IFormatProvider formatProvider, string format, Exception exception, params object[] args)
        {
            if (IsTraceEnabled)
                WriteInternal(LogLevel.Trace, new StringFormatFormattedMessage(formatProvider, format, args), exception);
        }

        public override void TraceFormat(string format, params object[] args)
        {
            if (IsTraceEnabled)
                WriteInternal(LogLevel.Trace, new StringFormatFormattedMessage(null, format, args), null);
        }

        public override void TraceFormat(string format, Exception exception, params object[] args)
        {
            if (IsTraceEnabled)
                WriteInternal(LogLevel.Trace, new StringFormatFormattedMessage(null, format, args), exception);
        }

        public override void Debug(object message)
        {
            if (IsDebugEnabled)
                WriteInternal(LogLevel.Debug, message, null);
        }

        public override void Debug(FormatMessageCallback formatMessageCallback)
        {
            if (IsDebugEnabled)
                WriteInternal(LogLevel.Debug, new FormatMessageCallbackFormattedMessage(formatMessageCallback), null);
        }

        public override void Debug(FormatMessageCallback formatMessageCallback, Exception exception)
        {
            if (IsDebugEnabled)
                WriteInternal(LogLevel.Debug, new FormatMessageCallbackFormattedMessage(formatMessageCallback), exception);
        }

        public override void Debug(IFormatProvider formatProvider, FormatMessageCallback formatMessageCallback)
        {
            if (IsDebugEnabled)
                WriteInternal(LogLevel.Debug, new FormatMessageCallbackFormattedMessage(formatProvider, formatMessageCallback), null);
        }

        public override void Debug(IFormatProvider formatProvider, FormatMessageCallback formatMessageCallback, Exception exception)
        {
            if (IsDebugEnabled)
                WriteInternal(LogLevel.Debug, new FormatMessageCallbackFormattedMessage(formatProvider, formatMessageCallback), exception);
        }

        public override void Debug(object message, Exception exception)
        {
            if (IsDebugEnabled)
                WriteInternal(LogLevel.Debug, message, exception);
        }

        public override void DebugFormat(IFormatProvider formatProvider, string format, params object[] args)
        {
            if (IsDebugEnabled)
                WriteInternal(LogLevel.Debug, new StringFormatFormattedMessage(formatProvider, format, args), null);
        }

        public override void DebugFormat(IFormatProvider formatProvider, string format, Exception exception, params object[] args)
        {
            if (IsDebugEnabled)
                WriteInternal(LogLevel.Debug, new StringFormatFormattedMessage(formatProvider, format, args), exception);
        }

        public override void DebugFormat(string format, params object[] args)
        {
            if (IsDebugEnabled)
                WriteInternal(LogLevel.Debug, new StringFormatFormattedMessage(null, format, args), null);
        }

        public override void DebugFormat(string format, Exception exception, params object[] args)
        {
            if (IsDebugEnabled)
                WriteInternal(LogLevel.Debug, new StringFormatFormattedMessage(null, format, args), exception);
        }

        public override void Info(object message)
        {
            if (IsInfoEnabled)
                WriteInternal(LogLevel.Info, message, null);
        }

        public override void Info(object message, Exception exception)
        {
            if (IsInfoEnabled)
                WriteInternal(LogLevel.Info, message, exception);
        }

        public override void Info(FormatMessageCallback formatMessageCallback)
        {
            if (IsInfoEnabled)
                WriteInternal(LogLevel.Info, new FormatMessageCallbackFormattedMessage(formatMessageCallback), null);
        }

        public override void Info(FormatMessageCallback formatMessageCallback, Exception exception)
        {
            if (IsInfoEnabled)
                WriteInternal(LogLevel.Info, new FormatMessageCallbackFormattedMessage(formatMessageCallback), exception);
        }

        public override void Info(IFormatProvider formatProvider, FormatMessageCallback formatMessageCallback)
        {
            if (IsInfoEnabled)
                WriteInternal(LogLevel.Info, new FormatMessageCallbackFormattedMessage(formatProvider, formatMessageCallback), null);
        }

        public override void Info(IFormatProvider formatProvider, FormatMessageCallback formatMessageCallback, Exception exception)
        {
            if (IsInfoEnabled)
                WriteInternal(LogLevel.Info, new FormatMessageCallbackFormattedMessage(formatProvider, formatMessageCallback), exception);
        }

        public override void InfoFormat(IFormatProvider formatProvider, string format, params object[] args)
        {
            if (IsInfoEnabled)
                WriteInternal(LogLevel.Info, new StringFormatFormattedMessage(formatProvider, format, args), null);
        }

        public override void InfoFormat(IFormatProvider formatProvider, string format, Exception exception, params object[] args)
        {
            if (IsInfoEnabled)
                WriteInternal(LogLevel.Info, new StringFormatFormattedMessage(formatProvider, format, args), exception);
        }

        public override void InfoFormat(string format, params object[] args)
        {
            if (IsInfoEnabled)
                WriteInternal(LogLevel.Info, new StringFormatFormattedMessage(null, format, args), null);
        }

        public override void InfoFormat(string format, Exception exception, params object[] args)
        {
            if (IsInfoEnabled)
                WriteInternal(LogLevel.Info, new StringFormatFormattedMessage(null, format, args), exception);
        }

        public override void Warn(object message)
        {
            if (IsWarnEnabled)
                WriteInternal(LogLevel.Warn, message, null);
        }

        public override void Warn(object message, Exception exception)
        {
            if (IsWarnEnabled)
                WriteInternal(LogLevel.Warn, message, exception);
        }

        public override void Warn(FormatMessageCallback formatMessageCallback)
        {
            if (IsWarnEnabled)
                WriteInternal(LogLevel.Warn, new FormatMessageCallbackFormattedMessage(formatMessageCallback), null);
        }

        public override void Warn(FormatMessageCallback formatMessageCallback, Exception exception)
        {
            if (IsWarnEnabled)
                WriteInternal(LogLevel.Warn, new FormatMessageCallbackFormattedMessage(formatMessageCallback), exception);
        }

        public override void Warn(IFormatProvider formatProvider, FormatMessageCallback formatMessageCallback)
        {
            if (IsWarnEnabled)
                WriteInternal(LogLevel.Warn, new FormatMessageCallbackFormattedMessage(formatProvider, formatMessageCallback), null);
        }

        public override void Warn(IFormatProvider formatProvider, FormatMessageCallback formatMessageCallback, Exception exception)
        {
            if (IsWarnEnabled)
                WriteInternal(LogLevel.Warn, new FormatMessageCallbackFormattedMessage(formatProvider, formatMessageCallback), exception);
        }

        public override void WarnFormat(IFormatProvider formatProvider, string format, params object[] args)
        {
            if (IsWarnEnabled)
                WriteInternal(LogLevel.Warn, new StringFormatFormattedMessage(formatProvider, format, args), null);
        }

        public override void WarnFormat(IFormatProvider formatProvider, string format, Exception exception, params object[] args)
        {
            if (IsWarnEnabled)
                WriteInternal(LogLevel.Warn, new StringFormatFormattedMessage(formatProvider, format, args), exception);
        }

        public override void WarnFormat(string format, params object[] args)
        {
            if (IsWarnEnabled)
                WriteInternal(LogLevel.Warn, new StringFormatFormattedMessage(null, format, args), null);
        }

        public override void WarnFormat(string format, Exception exception, params object[] args)
        {
            if (IsWarnEnabled)
                WriteInternal(LogLevel.Warn, new StringFormatFormattedMessage(null, format, args), exception);
        }

        public override void Error(object message)
        {
            if (IsErrorEnabled)
                WriteInternal(LogLevel.Error, message, null);
        }

        public override void Error(object message, Exception exception)
        {
            if (IsErrorEnabled)
                WriteInternal(LogLevel.Error, message, exception);
        }

        public override void Error(FormatMessageCallback formatMessageCallback)
        {
            if (IsErrorEnabled)
                WriteInternal(LogLevel.Error, new FormatMessageCallbackFormattedMessage(formatMessageCallback), null);
        }

        public override void Error(FormatMessageCallback formatMessageCallback, Exception exception)
        {
            if (IsErrorEnabled)
                WriteInternal(LogLevel.Error, new FormatMessageCallbackFormattedMessage(formatMessageCallback), exception);
        }

        public override void Error(IFormatProvider formatProvider, FormatMessageCallback formatMessageCallback)
        {
            if (IsErrorEnabled)
                WriteInternal(LogLevel.Error, new FormatMessageCallbackFormattedMessage(formatProvider, formatMessageCallback), null);
        }

        public override void Error(IFormatProvider formatProvider, FormatMessageCallback formatMessageCallback, Exception exception)
        {
            if (IsErrorEnabled)
                WriteInternal(LogLevel.Error, new FormatMessageCallbackFormattedMessage(formatProvider, formatMessageCallback), exception);
        }

        public override void ErrorFormat(IFormatProvider formatProvider, string format, params object[] args)
        {
            if (IsErrorEnabled)
                WriteInternal(LogLevel.Error, new StringFormatFormattedMessage(formatProvider, format, args), null);
        }

        public override void ErrorFormat(IFormatProvider formatProvider, string format, Exception exception, params object[] args)
        {
            if (IsErrorEnabled)
                WriteInternal(LogLevel.Error, new StringFormatFormattedMessage(formatProvider, format, args), exception);
        }

        public override void ErrorFormat(string format, params object[] args)
        {
            if (IsErrorEnabled)
                WriteInternal(LogLevel.Error, new StringFormatFormattedMessage(null, format, args), null);
        }

        public override void ErrorFormat(string format, Exception exception, params object[] args)
        {
            if (IsErrorEnabled)
                WriteInternal(LogLevel.Error, new StringFormatFormattedMessage(null, format, args), exception);
        }

        public override void Fatal(object message)
        {
            if (IsFatalEnabled)
                WriteInternal(LogLevel.Fatal, message, null);
        }

        public override void Fatal(object message, Exception exception)
        {
            if (IsFatalEnabled)
                WriteInternal(LogLevel.Fatal, message, exception);
        }

        public override void Fatal(FormatMessageCallback formatMessageCallback)
        {
            if (IsFatalEnabled)
                WriteInternal(LogLevel.Fatal, new FormatMessageCallbackFormattedMessage(formatMessageCallback), null);
        }

        public override void Fatal(FormatMessageCallback formatMessageCallback, Exception exception)
        {
            if (IsFatalEnabled)
                WriteInternal(LogLevel.Fatal, new FormatMessageCallbackFormattedMessage(formatMessageCallback), exception);
        }

        public override void Fatal(IFormatProvider formatProvider, FormatMessageCallback formatMessageCallback)
        {
            if (IsFatalEnabled)
                WriteInternal(LogLevel.Fatal, new FormatMessageCallbackFormattedMessage(formatProvider, formatMessageCallback), null);
        }

        public override void Fatal(IFormatProvider formatProvider, FormatMessageCallback formatMessageCallback, Exception exception)
        {
            if (IsFatalEnabled)
                WriteInternal(LogLevel.Fatal, new FormatMessageCallbackFormattedMessage(formatProvider, formatMessageCallback), exception);
        }

        public override void FatalFormat(IFormatProvider formatProvider, string format, params object[] args)
        {
            if (IsFatalEnabled)
                WriteInternal(LogLevel.Fatal, new StringFormatFormattedMessage(formatProvider, format, args), null);
        }

        public override void FatalFormat(IFormatProvider formatProvider, string format, Exception exception, params object[] args)
        {
            if (IsFatalEnabled)
                WriteInternal(LogLevel.Fatal, new StringFormatFormattedMessage(formatProvider, format, args), exception);
        }

        public override void FatalFormat(string format, params object[] args)
        {
            if (IsFatalEnabled)
                WriteInternal(LogLevel.Fatal, new StringFormatFormattedMessage(null, format, args), null);
        }

        public override void FatalFormat(string format, Exception exception, params object[] args)
        {
            if (IsFatalEnabled)
                WriteInternal(LogLevel.Fatal, new StringFormatFormattedMessage(null, format, args), exception);
        }
    }
}
