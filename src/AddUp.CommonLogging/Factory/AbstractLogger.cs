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
using AddUp.CommonLogging.Simple;
using FormatMessageCallback = System.Action<AddUp.CommonLogging.FormatMessageHandler>;

namespace AddUp.CommonLogging.Factory
{
    /// <summary>
    /// Provides base implementation suitable for almost all logger adapters
    /// </summary>
    /// <author>Erich Eichinger</author>
    [Serializable]
    public abstract partial class AbstractLogger : ILog
    {
        protected sealed class FormatMessageCallbackFormattedMessage
        {
            private volatile string cachedMessage;
            private readonly IFormatProvider messageFormatProvider;
            private readonly FormatMessageCallback formatMessageCallback;

            public FormatMessageCallbackFormattedMessage(FormatMessageCallback callback) =>
                formatMessageCallback = callback;

            public FormatMessageCallbackFormattedMessage(IFormatProvider formatProvider, FormatMessageCallback callback)
            {
                messageFormatProvider = formatProvider;
                formatMessageCallback = callback;
            }

            public override string ToString()
            {
                if (cachedMessage == null && formatMessageCallback != null)
                    formatMessageCallback(FormatMessage);
                return cachedMessage;
            }

            private string FormatMessage(string format, params object[] args)
            {
                if (args.Length > 0 && messageFormatProvider != null)
                    cachedMessage = string.Format(messageFormatProvider, format, args);
                else if (args.Length > 0)
                    cachedMessage = string.Format(format, args);
                else if (messageFormatProvider != null)
                    cachedMessage = string.Format(messageFormatProvider, format);
                else
                    cachedMessage = format;

                return cachedMessage;
            }
        }

        protected sealed class StringFormatFormattedMessage
        {
            private volatile string cachedMessage;
            private readonly IFormatProvider FormatProvider;
            private readonly string formatPart;
            private readonly object[] arguments;

            public StringFormatFormattedMessage(IFormatProvider formatProvider, string format, params object[] args)
            {
                FormatProvider = formatProvider;
                formatPart = format;
                arguments = args;
            }

            public override string ToString()
            {
                if (cachedMessage == null && formatPart != null)
                    cachedMessage = string.Format(FormatProvider, formatPart, arguments);
                return cachedMessage;
            }
        }

        protected delegate void WriteHandler(LogLevel level, object message, Exception exception);
        private readonly WriteHandler writeHandler;

        [SuppressMessage("Critical Code Smell", "S1699:Constructors should only call non-overridable methods", Justification = "Used by unit tests")]
        protected AbstractLogger()
        {
            writeHandler = GetWriteHandler();
            if (writeHandler == null)
                writeHandler = new WriteHandler(WriteInternal);
        }

        public virtual IVariablesContext GlobalVariablesContext => new NoOpVariablesContext();
        public virtual IVariablesContext ThreadVariablesContext => new NoOpVariablesContext();
        public virtual INestedVariablesContext NestedThreadVariablesContext => new NoOpNestedVariablesContext();

        protected virtual WriteHandler GetWriteHandler() => null;

        protected abstract void WriteInternal(LogLevel level, object message, Exception exception);
    }
}
