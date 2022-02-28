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

namespace AddUp.CommonLogging.Configuration
{
    /// <summary>
    /// Container used to hold configuration information from configuration file.
    /// </summary>
    /// <author>Gilles Bayon</author>
    public sealed class LogSetting
    {
        /// <summary>
        /// Create a new <see cref="LogSetting"/> instance
        /// </summary>
        /// <param name="factoryAdapterType">The <see cref="ILoggerFactoryAdapter" /> type  that will be used for creating <see cref="ILog" /></param>
        /// <param name="properties">Additional user supplied properties that are passed to the <paramref key="factoryAdapterType" />'s constructor.</param>
        public LogSetting(Type factoryAdapterType, NameValueCollection properties)
        {
            _ = ArgUtils.AssertNotNull(nameof(factoryAdapterType), factoryAdapterType);
            _ = ArgUtils.AssertIsAssignable<ILoggerFactoryAdapter>(
                nameof(factoryAdapterType), factoryAdapterType, 
                $"Type {factoryAdapterType.AssemblyQualifiedName} does not implement {typeof(ILoggerFactoryAdapter).FullName}");

            FactoryAdapterType = factoryAdapterType;
            Properties = properties;
        }

        /// <summary>
        /// The <see cref="ILoggerFactoryAdapter" /> type that will be used for creating <see cref="ILog" />
        /// instances.
        /// </summary>
        public Type FactoryAdapterType { get; }

        /// <summary>
        /// Additional user supplied properties that are passed to the <see cref="FactoryAdapterType" />'s constructor.
        /// </summary>
        public NameValueCollection Properties { get; }
    }
}