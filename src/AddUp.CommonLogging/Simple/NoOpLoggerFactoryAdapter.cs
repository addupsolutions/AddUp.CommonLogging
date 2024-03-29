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
using AddUp.CommonLogging.Configuration;

namespace AddUp.CommonLogging.Simple
{
    /// <summary>
    /// Factory for creating <see cref="ILog" /> instances that silently ignores
    /// logging requests.
    /// </summary>
    /// <remarks>
    /// This logger adapter is the default used by AddUp.CommonLogging if unconfigured. Using this logger adapter is the most efficient
    /// way to suppress any logging output.
    /// <example>
    /// Below is an example how to configure this adapter:
    /// <code>
    /// &lt;configuration&gt;
    /// 
    ///   &lt;configSections&gt;
    ///     &lt;sectionGroup key=&quot;common&quot;&gt;
    ///       &lt;section key=&quot;logging&quot;
    ///                type=&quot;AddUp.CommonLogging.ConfigurationSectionHandler, AddUp.CommonLogging&quot;
    ///                requirePermission=&quot;false&quot; /&gt;
    ///     &lt;/sectionGroup&gt;
    ///   &lt;/configSections&gt;
    /// 
    ///   &lt;common&gt;
    ///     &lt;logging&gt;
    ///       &lt;factoryAdapter type=&quot;AddUp.CommonLogging.Simple.NoOpLoggerFactoryAdapter, AddUp.CommonLogging&quot;&gt;
    ///         &lt;arg key=&quot;level&quot; value=&quot;ALL&quot; /&gt;
    ///       &lt;/factoryAdapter&gt;
    ///     &lt;/logging&gt;
    ///   &lt;/common&gt;
    /// 
    /// &lt;/configuration&gt;
    /// </code>
    /// </example>
    /// </remarks>
    /// <author>Gilles Bayon</author>
    public sealed class NoOpLoggerFactoryAdapter : ILoggerFactoryAdapter
    {
        private static readonly ILog logger = new NoOpLogger();

        public NoOpLoggerFactoryAdapter() { }
        public NoOpLoggerFactoryAdapter(NameValueCollection properties) { }

        public ILog GetLogger(Type type) => logger;
        ILog ILoggerFactoryAdapter.GetLogger(string key) => logger;
    }
}
