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
using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;
using AddUp.CommonLogging.Configuration;
using AddUp.CommonLogging.Simple;

namespace AddUp.CommonLogging
{
    /// <summary>
    /// Use the LogManager's <see cref="GetLogger(string)"/> or <see cref="GetLogger(System.Type)"/> 
    /// methods to obtain <see cref="ILog"/> instances for logging.
    /// </summary>
    /// <remarks>
    /// For configuring the underlying log system using application configuration, see the example 
    /// at <c>System.Configuration.ConfigurationManager</c>
    /// For configuring programmatically, see the example section below.
    /// </remarks>
    /// <example>
    /// The example below shows the typical use of LogManager to obtain a reference to a logger
    /// and log an exception:
    /// <code>
    /// 
    /// ILog log = LogManager.GetLogger(this.GetType());
    /// ...
    /// try 
    /// { 
    ///   /* .... */ 
    /// }
    /// catch(Exception ex)
    /// {
    ///   log.ErrorFormat("Hi {0}", ex, "dude");
    /// }
    /// 
    /// </code>
    /// The example below shows programmatic configuration of the underlying log system:
    /// <code>
    /// 
    /// // create properties
    /// NameValueCollection properties = new NameValueCollection();
    /// properties[&quot;showDateTime&quot;] = &quot;true&quot;;
    /// 
    /// // set Adapter
    /// AddUp.CommonLogging.LogManager.Adapter = new 
    /// AddUp.CommonLogging.Simple.ConsoleOutLoggerFactoryAdapter(properties);
    /// 
    /// </code>
    /// </example>
    /// <seealso cref="ILog"/>
    /// <seealso cref="Adapter"/>
    /// <seealso cref="ILoggerFactoryAdapter"/>
    /// <author>Gilles Bayon</author>
    public class LogManager : ILogManager
    {
        /// <summary>
        /// The key of the default configuration section to read settings from.
        /// </summary>
        /// <remarks>
        /// You can always change the source of your configuration settings by setting another <see cref="IConfigurationReader"/> instance
        /// on <see cref="ConfigurationReader"/>.
        /// </remarks>
        public static string COMMON_LOGGING_SECTION => "common/logging";

        /// <summary>
        /// The key of the default configuration section to read settings from.
        /// </summary>
        /// <remarks>
        /// You can always change the source of your configuration settings by setting another <see cref="IConfigurationReader"/> instance
        /// on <see cref="ConfigurationReader"/>.
        /// </remarks>
        string ILogManager.COMMON_LOGGING_SECTION => COMMON_LOGGING_SECTION;

        private static ILoggerFactoryAdapter adapter;
        private static readonly object loadLock = new object();

        /// <summary>
        /// Performs static 1-time initialization of LogManager by calling <see cref="Reset()"/>
        /// </summary>
        static LogManager() => Reset();

        /// <summary>
        /// Gets the configuration reader used to initialize the LogManager.
        /// </summary>
        /// <remarks>Primarily used for testing purposes but maybe useful to obtain configuration
        /// information from some place other than the .NET application configuration file.</remarks>
        /// <value>The configuration reader.</value>
        public static IConfigurationReader ConfigurationReader { get; private set; }

        IConfigurationReader ILogManager.ConfigurationReader => ConfigurationReader;

        /// <summary>
        /// Reset the <see cref="AddUp.CommonLogging" /> infrastructure to its default settings. This means, that configuration settings
        /// will be re-read from section <c>&lt;common/logging&gt;</c> of your <c>app.config</c>.
        /// </summary>
        /// <remarks>
        /// This is mainly used for unit testing, you wouldn't normally use this in your applications.<br/>
        /// <b>Note:</b><see cref="ILog"/> instances already handed out from this LogManager are not(!) affected. 
        /// Resetting LogManager only affects new instances being handed out.
        /// </remarks>
        public static void Reset() => Reset(new DefaultConfigurationReader());

        /// <summary>
        /// Reset the <see cref="AddUp.CommonLogging" /> infrastructure to its default settings. This means, that configuration settings
        /// will be re-read from section <c>&lt;common/logging&gt;</c> of your <c>app.config</c>.
        /// </summary>
        /// <remarks>
        /// This is mainly used for unit testing, you wouldn't normally use this in your applications.<br/>
        /// <b>Note:</b><see cref="ILog"/> instances already handed out from this LogManager are not(!) affected. 
        /// Resetting LogManager only affects new instances being handed out.
        /// </remarks>
        /// <param name="reader">
        /// the <see cref="IConfigurationReader"/> instance to obtain settings for 
        /// re-initializing the LogManager.
        /// </param>
        public static void Reset(IConfigurationReader reader)
        {
            lock (loadLock)
            {
                ConfigurationReader = reader ?? throw new ArgumentNullException("reader");
                adapter = null;
            }
        }

        void ILogManager.Reset() => Reset();
        void ILogManager.Reset(IConfigurationReader reader) { Reset(reader); }

        /// <summary>
        /// Reset the <see cref="AddUp.CommonLogging" /> infrastructure to the provided configuration.
        /// </summary>
        /// <remarks>
        /// <b>Note:</b><see cref="ILog"/> instances already handed out from this LogManager are not(!) affected.
        /// Configuring LogManager only affects new instances being handed out.
        /// </remarks>
        /// <param name="configuration">
        /// the <see cref="LogConfiguration"/> containing settings for
        /// re-initializing the LogManager.
        /// </param>
        public static void Configure(LogConfiguration configuration)
        {
            if (configuration == null)
                throw new ArgumentNullException("configuration");
            Reset(new LogConfigurationReader(configuration));
        }

        /// <summary>
        /// Gets or sets the adapter.
        /// </summary>
        /// <value>The adapter.</value>
        public static ILoggerFactoryAdapter Adapter
        {
            get
            {
                if (adapter == null)
                    lock (loadLock)
                    {
                        if (adapter == null)
                            adapter = BuildLoggerFactoryAdapter();
                    }

                return adapter;
            }
            set
            {
                if (value == null)
                    throw new ArgumentNullException("Adapter");

                lock (loadLock)
                    adapter = value;
            }
        }

        ILoggerFactoryAdapter ILogManager.Adapter
        {
            get => Adapter;
            set => Adapter = value;
        }

        /// <summary>
        /// Gets the logger by calling <see cref="ILoggerFactoryAdapter.GetLogger(Type)"/>
        /// on the currently configured <see cref="Adapter"/> using the specified type.
        /// </summary>
        /// <returns>the logger instance obtained from the current <see cref="Adapter"/></returns>
        public static ILog GetLogger<T>() => Adapter.GetLogger(typeof(T));

        /// <summary>
        /// Gets the logger by calling <see cref="ILoggerFactoryAdapter.GetLogger(Type)"/>
        /// on the currently configured <see cref="Adapter"/> using the specified type.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>the logger instance obtained from the current <see cref="Adapter"/></returns>
        public static ILog GetLogger(Type type) => Adapter.GetLogger(type);

        /// <summary>
        /// Gets the logger by calling <see cref="ILoggerFactoryAdapter.GetLogger(string)"/>
        /// on the currently configured <see cref="Adapter"/> using the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>the logger instance obtained from the current <see cref="Adapter"/></returns>
        public static ILog GetLogger(string key) => Adapter.GetLogger(key);

        ILog ILogManager.GetLogger<T>() => GetLogger<T>();
        ILog ILogManager.GetLogger(Type type) => GetLogger(type);
        ILog ILogManager.GetLogger(string key) => GetLogger(key);

        private static ILoggerFactoryAdapter BuildLoggerFactoryAdapter()
        {
            var sectionResult = ArgUtils.Guard(
                () => ConfigurationReader.GetSection(COMMON_LOGGING_SECTION),
                "Failed obtaining configuration for AddUp.CommonLogging from configuration section 'common/logging'.");

            // configuration reader returned <null>
            if (sectionResult == null)
            {
                var message = (ConfigurationReader is DefaultConfigurationReader)
                    ? $"no configuration section <{COMMON_LOGGING_SECTION}> found - suppressing logging output"
                    : $"Custom ConfigurationReader '{ConfigurationReader.GetType().FullName}' returned <null> - suppressing logging output";
                Trace.WriteLine(message);
                ILoggerFactoryAdapter defaultFactory = new NoOpLoggerFactoryAdapter();
                return defaultFactory;
            }

            // ready to use ILoggerFactoryAdapter?
            if (sectionResult is ILoggerFactoryAdapter factoryAdapter)
            {
                Trace.WriteLine($"Using ILoggerFactoryAdapter returned from custom ConfigurationReader '{ConfigurationReader.GetType().FullName}'");
                return factoryAdapter;
            }

            // ensure what's left is a LogSetting instance
            _ = ArgUtils.Guard(
                () => ArgUtils.AssertIsAssignable<LogSetting>("sectionResult", sectionResult.GetType()),
                $"ConfigurationReader {ConfigurationReader.GetType().FullName} returned unknown settings instance of type {sectionResult.GetType().FullName}");

            return ArgUtils.Guard(
                () => BuildLoggerFactoryAdapterFromLogSettings((LogSetting)sectionResult),
                "Failed creating LoggerFactoryAdapter from settings");
        }

        /// <summary>
        /// Builds a <see cref="ILoggerFactoryAdapter"/> instance from the given <see cref="LogSetting"/>
        /// using <see cref="Activator"/>.
        /// </summary>
        /// <param name="setting"></param>
        /// <returns>the <see cref="ILoggerFactoryAdapter"/> instance. Is never <c>null</c></returns>
        public static ILoggerFactoryAdapter BuildLoggerFactoryAdapterFromLogSettings(LogSetting setting)
        {
            _ = ArgUtils.AssertNotNull(nameof(setting), setting);
            var factoryAdapter = ArgUtils.Guard(() =>
            {
                if (setting.Properties != null && setting.Properties.Count > 0)
                {
                    var args = new object[] { setting.Properties };
                    return (ILoggerFactoryAdapter)Activator.CreateInstance(setting.FactoryAdapterType, args);
                }

                return (ILoggerFactoryAdapter)Activator.CreateInstance(setting.FactoryAdapterType);
            }, $"Unable to create instance of type {setting.FactoryAdapterType.FullName}. Possible explanation is lack of zero arg and single arg AddUp.CommonLogging.Configuration.NameValueCollection constructors");

            // make sure
            _ = ArgUtils.AssertNotNull(nameof(factoryAdapter), factoryAdapter, "Activator.CreateInstance() returned <null>");
            return factoryAdapter;
        }
    }
}