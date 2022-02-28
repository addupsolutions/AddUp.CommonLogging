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
using System.Diagnostics.CodeAnalysis;
using FormatMessageCallback = System.Action<AddUp.CommonLogging.FormatMessageHandler>;

namespace AddUp.CommonLogging.Simple
{
    /// <summary>
    /// Silently ignores all log messages.
    /// </summary>
    /// <author>Gilles Bayon</author>
    /// <author>Erich Eichinger</author>
    [Serializable]
    [SuppressMessage("Critical Code Smell", "S1186:Methods should not be empty", Justification = "Nop")]
    public sealed class NoOpLogger : ILog
    {
        public bool IsTraceEnabled => false;
        public bool IsDebugEnabled => false;
        public bool IsInfoEnabled => false;
        public bool IsWarnEnabled => false;
        public bool IsErrorEnabled => false;
        public bool IsFatalEnabled => false;

        public IVariablesContext GlobalVariablesContext => new NoOpVariablesContext();
        public IVariablesContext ThreadVariablesContext => new NoOpVariablesContext();
        public INestedVariablesContext NestedThreadVariablesContext => new NoOpNestedVariablesContext();
                
        public void Trace(object message) { }
        public void Trace(object message, Exception exception) { }
        public void Trace(FormatMessageCallback formatMessageCallback) { }
        public void Trace(FormatMessageCallback formatMessageCallback, Exception exception) { }
        public void Trace(IFormatProvider formatProvider, FormatMessageCallback formatMessageCallback) { }
        public void Trace(IFormatProvider formatProvider, FormatMessageCallback formatMessageCallback, Exception exception) { }
        public void TraceFormat(string format, params object[] args) { }
        public void TraceFormat(string format, Exception exception, params object[] args) { }
        public void TraceFormat(IFormatProvider formatProvider, string format, params object[] args) { }
        public void TraceFormat(IFormatProvider formatProvider, string format, Exception exception, params object[] args) { }

        public void Debug(object message) { }
        public void Debug(object message, Exception exception) { }
        public void Debug(FormatMessageCallback formatMessageCallback) { }
        public void Debug(FormatMessageCallback formatMessageCallback, Exception exception) { }
        public void Debug(IFormatProvider formatProvider, FormatMessageCallback formatMessageCallback) { }
        public void Debug(IFormatProvider formatProvider, FormatMessageCallback formatMessageCallback, Exception exception) { }
        public void DebugFormat(string format, params object[] args) { }
        public void DebugFormat(string format, Exception exception, params object[] args) { }
        public void DebugFormat(IFormatProvider formatProvider, string format, params object[] args) { }
        public void DebugFormat(IFormatProvider formatProvider, string format, Exception exception, params object[] args) { }

        public void Info(object message) { }
        public void Info(object message, Exception exception) { }
        public void Info(FormatMessageCallback formatMessageCallback) { }
        public void Info(FormatMessageCallback formatMessageCallback, Exception exception) { }
        public void Info(IFormatProvider formatProvider, FormatMessageCallback formatMessageCallback) { }
        public void Info(IFormatProvider formatProvider, FormatMessageCallback formatMessageCallback, Exception exception) { }
        public void InfoFormat(string format, params object[] args) { }
        public void InfoFormat(string format, Exception exception, params object[] args) { }
        public void InfoFormat(IFormatProvider formatProvider, string format, params object[] args) { }
        public void InfoFormat(IFormatProvider formatProvider, string format, Exception exception, params object[] args) { }

        public void Warn(object message) { }
        public void Warn(object message, Exception exception) { }
        public void Warn(FormatMessageCallback formatMessageCallback) { }
        public void Warn(FormatMessageCallback formatMessageCallback, Exception exception) { }
        public void Warn(IFormatProvider formatProvider, FormatMessageCallback formatMessageCallback) { }
        public void Warn(IFormatProvider formatProvider, FormatMessageCallback formatMessageCallback, Exception exception) { }
        public void WarnFormat(string format, params object[] args) { }
        public void WarnFormat(string format, Exception exception, params object[] args) { }
        public void WarnFormat(IFormatProvider formatProvider, string format, params object[] args) { }
        public void WarnFormat(IFormatProvider formatProvider, string format, Exception exception, params object[] args) { }

        public void Error(object message) { }
        public void Error(object message, Exception exception) { }
        public void Error(FormatMessageCallback formatMessageCallback) { }
        public void Error(FormatMessageCallback formatMessageCallback, Exception exception) { }
        public void Error(IFormatProvider formatProvider, FormatMessageCallback formatMessageCallback) { }
        public void Error(IFormatProvider formatProvider, FormatMessageCallback formatMessageCallback, Exception exception) { }
        public void ErrorFormat(string format, params object[] args) { }
        public void ErrorFormat(string format, Exception exception, params object[] args) { }
        public void ErrorFormat(IFormatProvider formatProvider, string format, params object[] args) { }
        public void ErrorFormat(IFormatProvider formatProvider, string format, Exception exception, params object[] args) { }

        public void Fatal(object message) { }
        public void Fatal(object message, Exception exception) { }
        public void Fatal(FormatMessageCallback formatMessageCallback) { }
        public void Fatal(FormatMessageCallback formatMessageCallback, Exception exception) { }
        public void Fatal(IFormatProvider formatProvider, FormatMessageCallback formatMessageCallback) { }
        public void Fatal(IFormatProvider formatProvider, FormatMessageCallback formatMessageCallback, Exception exception) { }
        public void FatalFormat(string format, params object[] args) { }
        public void FatalFormat(string format, Exception exception, params object[] args) { }
        public void FatalFormat(IFormatProvider formatProvider, string format, params object[] args) { }
        public void FatalFormat(IFormatProvider formatProvider, string format, Exception exception, params object[] args) { }
    }
}
