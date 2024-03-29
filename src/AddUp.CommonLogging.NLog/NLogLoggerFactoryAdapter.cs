#region License

/*
 * Copyright � 2002-2009 the original author or authors.
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
using System.IO;
using AddUp.CommonLogging.Configuration;
using AddUp.CommonLogging.Factory;

namespace AddUp.CommonLogging.NLog
{
    /// <summary>
    /// Concrete subclass of ILoggerFactoryAdapter specific to NLog.
    /// </summary>
    /// <remarks>
    /// <para>Note, that you cannot use NLog in medium trust environments unless you use an unsigned build</para>
    /// The following configuration property values may be configured:
    /// <list type="bullet">
    ///     <item><c>configType</c>: <c>INLINE|FILE</c></item>
    ///     <item><c>configFile</c>: NLog XML configuration file path in case of FILE</item>
    /// </list>
    /// The configType values have the following implications:
    /// <list type="bullet">
    ///     <item>FILE: calls <c>NLog.LogManager.Configuration = new NLog.Config.XmlLoggingConfiguration(configFile)</c>.</item>
    ///     <item>&lt;any other value&gt;: expects NLog to be configured externally</item>
    /// </list>
    /// </remarks>
    /// <example>
    /// The following snippet shows how to configure EntLib logging for AddUp.CommonLogging:
    /// <code>
    /// &lt;configuration&gt;
    ///   &lt;configSections&gt;
    ///       &lt;section name=&quot;logging&quot; type=&quot;AddUp.CommonLogging.ConfigurationSectionHandler, AddUp.CommonLogging&quot; /&gt;
    ///   &lt;/configSections&gt;
    ///   &lt;common&gt;
    ///     &lt;logging&gt;
    ///       &lt;factoryAdapter type=&quot;AddUp.CommonLogging.NLog.NLogLoggerFactoryAdapter, AddUp.CommonLogging.NLog&quot;&gt;
    ///         &lt;arg key=&quot;configType&quot; value=&quot;FILE&quot; /&gt;
    ///         &lt;arg key=&quot;configFile&quot; value=&quot;~/nlog.config&quot; /&gt;
    ///       &lt;/factoryAdapter&gt;
    ///     &lt;/logging&gt;
    ///   &lt;/common&gt;
    /// &lt;/configuration&gt;
    /// </code>
    /// </example>
    /// <author>Bruno Baia</author>
    /// <author>Erich Eichinger</author>
    public class NLogLoggerFactoryAdapter : AbstractCachingLoggerFactoryAdapter
    {
        public NLogLoggerFactoryAdapter(NameValueCollection properties) : base(true)
        {
            var (configType, configFile) = GetConfiguration(properties);
            switch (configType)
            {
                case "INLINE":
                    // Looks like this is not supported
                    break;
                case "FILE":
                    global::NLog.LogManager.Configuration = new global::NLog.Config.XmlLoggingConfiguration(configFile);
                    break;
                default:
                    break;
            }
        }

        protected override ILog CreateLogger(string name) => new NLogLogger(global::NLog.LogManager.GetLogger(name));

        private static (string configType, string configFile) GetConfiguration(NameValueCollection properties)
        {
            if (properties == null)
                return (string.Empty, string.Empty);

            var configType = string.Empty;
            var configFile = string.Empty;

            if (properties["configType"] != null)
                configType = properties["configType"].ToUpperInvariant();

            if (properties["configFile"] != null)
            {
                configFile = properties["configFile"];
                if (configFile.StartsWith("~/") || configFile.StartsWith("~\\"))
                    configFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory.TrimEnd('/', '\\'), configFile.Substring(2));
            }

            if (configType == "FILE")
            {
                if (configFile == string.Empty)
                    throw new ConfigurationException("Configuration property 'configFile' must be set for NLog configuration of type 'FILE'.");

                if (!File.Exists(configFile))
                    throw new ConfigurationException($"NLog configuration file '{configFile}' does not exist");
            }

            return (configType, configFile);
        }
    }
}
