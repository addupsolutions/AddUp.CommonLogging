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

using System.Configuration;
using System.Diagnostics.CodeAnalysis;
using System.Xml;

namespace AddUp.CommonLogging.Utils
{
    /// <summary>
    /// A ConfigurationReader implementation that call the ConfigurationSectionHandler on a 
    /// supplied XML string.
    /// </summary>
    /// <author>Mark Pollack</author>
    [ExcludeFromCodeCoverage]
    internal sealed class StandaloneConfigurationReader : IConfigurationReader
    {
        public StandaloneConfigurationReader() { }
        public StandaloneConfigurationReader(string xmlString) => XmlString = xmlString;

        public string XmlString { get; }

        public object GetSection(string sectionName)
        {
            var handler = new ConfigurationSectionHandler();
            return handler.Create(null, BuildConfigurationSection(XmlString));
        }

        private static XmlNode BuildConfigurationSection(string xml)
        {
            var doc = new ConfigXmlDocument();
            doc.LoadXml(xml);
            return doc.DocumentElement;
        }
    }
}