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
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;

namespace AddUp.CommonLogging.Configuration
{
    /// <summary>
    /// Various utility methods for using during factory and logger instance configuration
    /// </summary>
    /// <author>Erich Eichinger</author>
    public static class ArgUtils
    {
        private static readonly Dictionary<Type, object> parsers;

        static ArgUtils()
        {
            parsers = new Dictionary<Type, object>();
            RegisterTypeParser(Convert.ToBoolean);
            RegisterTypeParser(Convert.ToInt16);
            RegisterTypeParser(Convert.ToInt32);
            RegisterTypeParser(Convert.ToInt64);
            RegisterTypeParser(Convert.ToSingle);
            RegisterTypeParser(Convert.ToDouble);
            RegisterTypeParser(Convert.ToDecimal);
        }

        /// <summary>
        /// A delegate converting a string representation into the target type
        /// </summary>
        public delegate T ParseHandler<out T>(string text);

        /// <summary>
        /// Adds the parser to the list of known type parsers.
        /// </summary>
        /// <remarks>
        /// .NET intrinsic types are pre-registerd: short, int, long, float, double, decimal, bool
        /// </remarks>
        public static void RegisterTypeParser<T>(ParseHandler<T> parser) => parsers[typeof(T)] = parser;

        /// <summary>
        /// Retrieves the named value from the specified <see cref="NameValueCollection"/>.
        /// </summary>
        /// <param name="values">may be null</param>
        /// <param name="key">the value's key</param>
        /// <returns>if <paramref key="values"/> is not null, the value returned by values[key]. <c>null</c> otherwise.</returns>
        public static string GetValue(NameValueCollection values, string key) => GetValue(values, key, null);

        /// <summary>
        /// Retrieves the named value from the specified <see cref="NameValueCollection"/>.
        /// </summary>
        /// <param name="values">may be null</param>
        /// <param name="key">the value's key</param>
        /// <param name="defaultValue">the default value, if not found</param>
        /// <returns>if <paramref key="values"/> is not null, the value returned by values[key]. <c>null</c> otherwise.</returns>
        public static string GetValue(NameValueCollection values, string key, string defaultValue)
        {
            if (values == null) return defaultValue;

            foreach (string valueKey in values.Keys)
            {
                if (string.Compare(key, valueKey, StringComparison.OrdinalIgnoreCase) == 0)
                    return values[key];
            }

            return defaultValue;
        }

        /// <summary>
        /// Returns the first nonnull, nonempty value among its arguments.
        /// </summary>
        /// <remarks>
        /// Returns <c>null</c>, if the initial list was null or empty.
        /// </remarks>
        /// <seealso cref="Coalesce{T}"/>
        public static string Coalesce(params string[] values) => Coalesce(v => !string.IsNullOrEmpty(v), values);

        /// <summary>
        /// Returns the first non-null, non-empty value among its arguments.
        /// </summary>
        public static T Coalesce<T>(Predicate<T> predicate, params T[] values) where T : class
        {
            if (values == null || values.Length == 0) return null;
            if (predicate == null) predicate = v => v != null;

            for (var i = 0; i < values.Length; i++)
            {
                T val = values[i];
                if (predicate(val))
                    return val;
            }

            return null;
        }

        /// <summary>
        /// Tries parsing <paramref key="stringValue"/> into an enum of the type of <paramref key="defaultValue"/>.
        /// </summary>
        /// <param name="defaultValue">the default value to return if parsing fails</param>
        /// <param name="stringValue">the string value to parse</param>
        /// <returns>the successfully parsed value, <paramref key="defaultValue"/> otherwise.</returns>
        public static T TryParseEnum<T>(T defaultValue, string stringValue) where T : struct
        {
            var enumType = typeof(T);
            if (!enumType.IsEnum) throw new ArgumentException($"Type '{typeof(T).FullName}' is not an enum type");

            try
            {
                // If a string is specified then try to parse and return it
                if (!string.IsNullOrEmpty(stringValue))
                    return (T)Enum.Parse(enumType, stringValue, true);
            }
            catch (Exception ex)
            {
                Trace.WriteLine($"WARN: failed to convert value '{stringValue}' to enum type '{defaultValue.GetType().FullName}' ({ex.Message})");
            }

            return defaultValue;
        }

        /// <summary>
        /// Tries parsing <paramref key="stringValue"/> into the specified return type.
        /// </summary>
        /// <param name="defaultValue">the default value to return if parsing fails</param>
        /// <param name="stringValue">the string value to parse</param>
        /// <returns>the successfully parsed value, <paramref key="defaultValue"/> otherwise.</returns>
        public static T TryParse<T>(T defaultValue, string stringValue)
        {
            T result = defaultValue;
            if (string.IsNullOrEmpty(stringValue)) return defaultValue;

            if (!parsers.TryGetValue(typeof(T), out var untypedParser) || !(untypedParser is ParseHandler<T>))
                throw new ArgumentException($"There is no parser registered for type {typeof(T).FullName}");

            var parser = (ParseHandler<T>)untypedParser;
            try
            {
                result = parser(stringValue);
            }
            catch (Exception ex)
            {
                Trace.WriteLine($"WARN: failed to convert value '{stringValue}' to type '{typeof(T).FullName}' - returning default '{result}' ({ex.Message})");
            }

            return result;
        }

        /// <summary>
        /// Throws an <see cref="ArgumentNullException"/> if <paramref key="val"/> is <c>null</c>.
        /// </summary>
        public static T AssertNotNull<T>(string paramName, T val) where T : class
        {
            if (val is null)
                throw new ArgumentNullException(paramName);
            return val;
        }

        /// <summary>
        /// Throws a <see cref="ArgumentNullException"/> if <paramref key="value"/> is <c>null</c>.
        /// </summary>
        public static T AssertNotNull<T>(string paramName, T value, string messageFormat, params object[] args) where T : class
        {
            if (value is null)
                throw new ArgumentNullException(paramName, string.Format(messageFormat, args));
            return value;
        }

        /// <summary>
        /// Throws a <see cref="ArgumentOutOfRangeException"/> if an object of type <paramref key="valueType"/> is not
        /// assignable to type <typeparam key="T"></typeparam>.
        /// </summary>
        public static Type AssertIsAssignable<T>(string paramName, Type valueType) => AssertIsAssignable<T>(
            paramName,
            valueType,
            $"Type '{valueType?.AssemblyQualifiedName ?? "<undefined>"}' of parameter '{paramName}' is not assignable to target type '{typeof(T).AssemblyQualifiedName}'");

        /// <summary>
        /// Throws a <see cref="ArgumentOutOfRangeException"/> if an object of type <paramref key="valType"/> is not
        /// assignable to type <typeparam key="T"></typeparam>.
        /// </summary>
        public static Type AssertIsAssignable<T>(string paramName, Type valueType, string messageFormat, params object[] args)
        {
            if (valueType == null) throw new ArgumentNullException(nameof(valueType));
            if (!typeof(T).IsAssignableFrom(valueType)) throw new ArgumentOutOfRangeException(paramName, valueType, string.Format(messageFormat, args));

            return valueType;
        }

        /// <summary>
        /// Ensures any exception thrown by the given <paramref key="action"/> is wrapped with an
        /// <see cref="ConfigurationException"/>. 
        /// </summary>
        /// <remarks>
        /// If <paramref key="action"/> already throws a ConfigurationException, it will not be wrapped.
        /// </remarks>
        /// <param name="action">the action to execute</param>
        /// <param name="messageFormat">the message to be set on the thrown <see cref="ConfigurationException"/></param>
        /// <param name="args">args to be passed to <see cref="string.Format(string,object[])"/> to format the message</param>
        public static void Guard(Action action, string messageFormat, params object[] args) =>
            Guard(() => { action(); return 0; }, messageFormat, args);

        /// <summary>
        /// Ensures any exception thrown by the given <paramref key="function"/> is wrapped with an
        /// <see cref="ConfigurationException"/>. 
        /// </summary>
        /// <remarks>
        /// If <paramref key="function"/> already throws a ConfigurationException, it will not be wrapped.
        /// </remarks>
        /// <param name="function">the action to execute</param>
        /// <param name="messageFormat">the message to be set on the thrown <see cref="ConfigurationException"/></param>
        /// <param name="args">args to be passed to <see cref="string.Format(string,object[])"/> to format the message</param>
        public static T Guard<T>(Func<T> function, string messageFormat, params object[] args)
        {
            try
            {
                return function();
            }
            catch (ConfigurationException)
            {
                throw;
            }
            catch (TargetInvocationException tex)
            {
                throw new ConfigurationException(string.Format(messageFormat, args), tex.InnerException);
            }
            catch (Exception ex)
            {
                throw new ConfigurationException(string.Format(messageFormat, args), ex);
            }
        }
    }
}