#region License

/*
 * Copyright © 2002-2013 the original author or authors.
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
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using System.Text;
using System.Threading;

namespace AddUp.CommonLogging.Simple
{
    internal static class ExceptionFormatter
    {
        // constants
        private const string unavailable = "<unavailable>";
        private const string STANDARD_DELIMETER = "================================================================================\r\n";
        private const string INNERMOST_DELIMETER = "=======================================================(inner most exception)===\r\n";

        internal static string Format(Exception exception) => Format(exception, CultureInfo.InvariantCulture);
        internal static string Format(Exception exception, IFormatProvider formatProvider)
        {
            if (exception == null) return null;

            // push all inner exceptions onto stack
            var exceptionStack = new Stack<Exception>();
            var currentException = exception;
            while (currentException != null)
            {
                exceptionStack.Push(currentException);
                currentException = currentException.InnerException;
            }

            // go through inner exceptions in reverse order
            var builder = new StringBuilder();
            for (var i = 1; exceptionStack.Count > 0; i++)
            {
                currentException = exceptionStack.Pop();
                FormatSingleException(formatProvider, builder, currentException, i);
            }

            return builder.ToString();
        }

        private static void FormatSingleException(IFormatProvider formatProvider, StringBuilder builder, Exception exception, int exceptionIndex)
        {
            OutputHeader(formatProvider, builder, exception, exceptionIndex);
            OutputDetails(formatProvider, builder, exception);
            OutputMessage(formatProvider, builder, exception);
            OutputProperties(formatProvider, builder, exception);
            OutputData(formatProvider, builder, exception);
            OutputStackTrace(formatProvider, builder, exception);
            _ = builder.Append(STANDARD_DELIMETER);
        }

        // output header:
        //
        //	=======================================================(inner most exception)===
        //	 (index) exception-type-name
        //  ================================================================================
        //
        private static void OutputHeader(IFormatProvider formatProvider, StringBuilder builder, Exception exception, int exceptionIndex) => builder
            .Append(exceptionIndex == 1 ? INNERMOST_DELIMETER : STANDARD_DELIMETER)
            .AppendFormat(formatProvider, " ({0}) {1}\r\n", exceptionIndex, exception.GetType().FullName)
            .Append(STANDARD_DELIMETER);

        private static void OutputDetails(IFormatProvider formatProvider, StringBuilder builder, Exception exception)
        {
            // output exception details:
            //
            //	Method        :  set_Attributes
            //	Type          :  System.IO.FileSystemInfo
            //	Assembly      :  mscorlib, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
            //	Assembly Path :  C:\WINDOWS\Microsoft.NET\Framework\v2.0.50727\mscorlib.dll
            //	Source        :  mscorlib
            //	Thread        :  123 'TestRunnerThread'
            //  Helplink      :  <unavailable>

            SafeGetTargetSiteInfo(exception, out var assemblyName, out var assemblyModuleName, out var typeName, out var methodName);
            SafeGetSourceAndHelplink(exception, out var source, out var helplink);

            _ = builder.AppendFormat(formatProvider,
                "Method        :  {0}\r\n" +
                "Type          :  {1}\r\n" +
                "Assembly      :  {2}\r\n" +
                "Assembly Path :  {3}\r\n" +
                "Source        :  {4}\r\n" +
                "Thread        :  {5} '{6}'\r\n" +
                "Helplink      :  {7}\r\n",
                methodName, typeName, assemblyName, assemblyModuleName,
                source,
                Environment.CurrentManagedThreadId, Thread.CurrentThread.Name,
                helplink);
        }

        private static void OutputMessage(IFormatProvider formatProvider, StringBuilder builder, Exception exception) =>
            builder.AppendFormat(formatProvider, "\r\nMessage:\r\n\"{0}\"\r\n", exception.Message);

        private static void OutputProperties(IFormatProvider formatProvider, StringBuilder builder, Exception exception)
        {
            var properties = exception.GetType().GetProperties(BindingFlags.FlattenHierarchy | BindingFlags.Instance | BindingFlags.Public);
            var first = true;

            foreach (var property in properties)
            {
                if (property.DeclaringType == typeof(Exception)) continue;
                if (property.Name == "Message") continue;

                if (first)
                {
                    first = false;
                    _ = builder.Append("\r\nProperties:\r\n");
                }

                OutputProperty(property, formatProvider, builder, exception);
            }
        }

        private static void OutputProperty(PropertyInfo property, IFormatProvider formatProvider, StringBuilder builder, Exception exception)
        {
            object propertyValue = unavailable;

            if (property.CanRead && property.GetIndexParameters().Length <= 0)
                propertyValue = property.GetValue(exception, null);

            var propertyTypeName = property.ReflectedType.Name;
            if (propertyValue is IEnumerable enumerableValue && !(propertyValue is string))
            {
                _ = builder.AppendFormat(formatProvider, "  {0}.{1} = {{\r\n", propertyTypeName, property.Name);

                foreach (var item in enumerableValue)
                    _ = builder.AppendFormat("    \"{0}\",\r\n", item != null ? item.ToString() : "<null>");

                _ = builder.Append("  }\r\n");
            }
            else _ = builder.AppendFormat(formatProvider, "  {0}.{1} = \"{2}\"\r\n", propertyTypeName, property.Name, propertyValue);
        }

        private static void OutputData(IFormatProvider formatProvider, StringBuilder builder, Exception exception)
        {
            if (exception.Data.Count == 0) return;

            _ = builder.Append("\r\nData:\r\n");
            foreach (DictionaryEntry entry in exception.Data)
                _ = builder.AppendFormat(formatProvider, "{0} = \"{1}\"\r\n", entry.Key, entry.Value);
        }

        // output stack trace:
        //
        //	Stack Trace:
        //	  at System.IO.FileSystemInfo.set_Attributes(FileAttributes value)
        //    at AddUp.CommonLogging.LogStoreWriter._SetupRootFolder() 
        //    at AddUp.CommonLogging.LogStoreWriter..ctor(String rootPath, Int32 maxStoreSize, Int32 minBacklogs) 
        private static void OutputStackTrace(IFormatProvider formatProvider, StringBuilder builder, Exception exception) =>
            builder.AppendFormat(formatProvider, "\r\nStack Trace:\r\n{0}\r\n", exception.StackTrace);

        private static void SafeGetTargetSiteInfo(Exception exception, out string assemblyName, out string assemblyModulePath, out string typeName, out string methodName)
        {
            assemblyName = unavailable;
            assemblyModulePath = unavailable;
            typeName = unavailable;
            methodName = unavailable;

            var targetSite = exception.TargetSite;
            if (targetSite != null)
            {
                methodName = targetSite.Name;

                var type = targetSite.ReflectedType;
                typeName = type.FullName;

                var assembly = type.Assembly;
                assemblyName = assembly.FullName;

                var assemblyModule = assembly.ManifestModule;
                assemblyModulePath = assemblyModule.FullyQualifiedName;
            }
        }

        private static void SafeGetSourceAndHelplink(Exception exception, out string source, out string helplink)
        {
            source = exception.Source;
            helplink = exception.HelpLink;
        }
    }
}