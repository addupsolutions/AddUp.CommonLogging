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
using AddUp.CommonLogging.Configuration;
using AddUp.CommonLogging.Simple;
using AddUp.CommonLogging.Utils;
using NUnit.Framework;

namespace AddUp.CommonLogging
{
    [TestFixture, ExcludeFromCodeCoverage]
    public class ConfigurationSectionHandlerTests
    {
        [Test]
        public void NoParentSectionsAllowed()
        {
            IConfigurationSectionHandler handler = new ConfigurationSectionHandler();
            _ = Assert.Throws(
                Is.TypeOf<ConfigurationException>().And.Message.EqualTo("parent configuration sections are not allowed"),
                () => handler.Create(new LogSetting(typeof(ConsoleOutLoggerFactoryAdapter), null), null, null));
        }

        [Test]
        public void TooManyAdapterElements()
        {
            const string xml =
@"<?xml version='1.0' encoding='UTF-8' ?>
<logging>
    <factoryAdapter type='AddUp.CommonLogging.Simple.ConsoleOutLoggerFactoryAdapter, AddUp.CommonLogging'>
    </factoryAdapter>
    <factoryAdapter type='AddUp.CommonLogging.Simple.ConsoleOutLoggerFactoryAdapter, AddUp.CommonLogging'>
    </factoryAdapter>
</logging>";

            var reader = new StandaloneConfigurationReader(xml);
            _ = Assert.Throws(
                Is.TypeOf<ConfigurationException>().And.Message.EqualTo("Only one <factoryAdapter> element allowed"),
                () => reader.GetSection(null));
        }

        [Test]
        public void NoTypeElementForAdapterDeclaration()
        {
            const string xml =
@"<?xml version='1.0' encoding='UTF-8' ?>
<logging>
    <factoryAdapter clazz='AddUp.CommonLogging.Simple.ConsoleOutLoggerFactoryAdapter, AddUp.CommonLogging'>
    <arg kez='level' value='DEBUG' />
    </factoryAdapter>
</logging>";

            var reader = new StandaloneConfigurationReader(xml);
            _ = Assert.Throws<ConfigurationException>(() => reader.GetSection(null));
        }

        [Test]
        public void NoKeyElementForAdapterArguments()
        {
            const string xml =
@"<?xml version='1.0' encoding='UTF-8' ?>
<logging>
    <factoryAdapter type='AddUp.CommonLogging.Simple.ConsoleOutLoggerFactoryAdapter, AddUp.CommonLogging'>
    <arg kez='level' value='DEBUG' />
    </factoryAdapter>
</logging>";

            var reader = new StandaloneConfigurationReader(xml);
            _ = Assert.Throws<ConfigurationException>(() => reader.GetSection(null));
        }

        [Test]
        public void ConsoleShortcut()
        {
            const string xml =
@"<?xml version='1.0' encoding='UTF-8' ?>
<logging>
    <factoryAdapter type='CONSOLE'/>
</logging>";

            var reader = new StandaloneConfigurationReader(xml);
            var setting = reader.GetSection(null) as LogSetting;
            Assert.IsNotNull(setting);
            Assert.AreEqual(typeof(ConsoleOutLoggerFactoryAdapter), setting.FactoryAdapterType);
        }

        [Test]
        public void TraceShortCut()
        {
            const string xml =
@"<?xml version='1.0' encoding='UTF-8' ?>
<logging>
    <factoryAdapter type='TRACE'/>
</logging>";

            var reader = new StandaloneConfigurationReader(xml);
            var setting = reader.GetSection(null) as LogSetting;
            Assert.IsNotNull(setting);
            Assert.AreEqual(typeof(TraceLoggerFactoryAdapter), setting.FactoryAdapterType);
        }

        [Test]
        public void NoOpShortCut()
        {
            const string xml =
@"<?xml version='1.0' encoding='UTF-8' ?>
<logging>
    <factoryAdapter type='NOOP'/>
</logging>";
            
            var reader = new StandaloneConfigurationReader(xml);
            var setting = reader.GetSection(null) as LogSetting;
            Assert.IsNotNull(setting);
            Assert.AreEqual(typeof(NoOpLoggerFactoryAdapter), setting.FactoryAdapterType);
        }

        [Test]
        public void ArgumentKeysCaseInsensitive()
        {
            const string xml =
@"<?xml version='1.0' encoding='UTF-8' ?>
<logging>
    <factoryAdapter type='CONSOLE'>
    <arg key='LeVel1' value='DEBUG' />
    <arg key='LEVEL2' value='DEBUG' />
    <arg key='level3' value='DEBUG' />
    </factoryAdapter>
</logging>";

            var reader = new StandaloneConfigurationReader(xml);
            var setting = reader.GetSection(null) as LogSetting;
            Assert.IsNotNull(setting);

            Assert.AreEqual(3, setting.Properties.Count);

            var expectedValue = new[] { "DEBUG" };
            CollectionAssert.AreEqual(expectedValue, setting.Properties.GetValues("level1"));
            CollectionAssert.AreEqual(expectedValue, setting.Properties.GetValues("level2"));
            CollectionAssert.AreEqual(expectedValue, setting.Properties.GetValues("LEVEL3"));
        }
    }
}
