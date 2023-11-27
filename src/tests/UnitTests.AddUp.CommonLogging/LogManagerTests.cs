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

using System;
using System.Diagnostics.CodeAnalysis;
using AddUp.CommonLogging.Configuration;
using AddUp.CommonLogging.Simple;
using AddUp.CommonLogging.Utils;
using NUnit.Framework;
using NUnit.Framework.Legacy;
using Rhino.Mocks;

namespace AddUp.CommonLogging;

[TestFixture, ExcludeFromCodeCoverage]
public class LogManagerTests
{
    public MockRepository mocks;

    [SetUp]
    public void SetUp()
    {
        LogManager.Reset();
        mocks = new MockRepository();
    }

    [Test]
    public void AdapterProperty()
    {
        ILoggerFactoryAdapter adapter = new NoOpLoggerFactoryAdapter();
        LogManager.Adapter = adapter;
        ClassicAssert.AreSame(adapter, LogManager.Adapter);

        _ = Assert.Throws<ArgumentNullException>(delegate { LogManager.Adapter = null; });
    }

    [Test]
    public void Reset()
    {
        LogManager.Reset();
        ClassicAssert.IsInstanceOf<DefaultConfigurationReader>(LogManager.ConfigurationReader);

        _ = Assert.Throws<ArgumentNullException>(delegate { LogManager.Reset(null); });

        IConfigurationReader r = mocks.StrictMock<IConfigurationReader>();
        using (mocks.Record())
            _ = Expect.Call(r.GetSection(LogManager.ADDUP_COMMONLOGGING_SECTION)).Return(new TraceLoggerFactoryAdapter());

        using (mocks.Playback())
        {
            LogManager.Reset(r);
            ClassicAssert.IsInstanceOf<TraceLoggerFactoryAdapter>(LogManager.Adapter);
        }
    }

    [Test]
    public void ConfigureFromConfigurationReader()
    {
        var r = mocks.StrictMock<IConfigurationReader>();
        using (mocks.Record())
        {
            _ = Expect.Call(r.GetSection(LogManager.ADDUP_COMMONLOGGING_SECTION)).Return(null);
            _ = Expect.Call(r.GetSection(LogManager.ADDUP_COMMONLOGGING_SECTION)).Return(new TraceLoggerFactoryAdapter());
            _ = Expect.Call(r.GetSection(LogManager.ADDUP_COMMONLOGGING_SECTION)).Return(new LogSetting(typeof(ConsoleOutLoggerFactoryAdapter), null));
            _ = Expect.Call(r.GetSection(LogManager.ADDUP_COMMONLOGGING_SECTION)).Return(new object());
        }

        using (mocks.Playback())
        {
            ILog log;

            // accepts null sectionhandler return
            LogManager.Reset(r);
            log = LogManager.GetLogger<LogManagerTests>();
            ClassicAssert.AreEqual(typeof(NoOpLogger), log.GetType());

            // accepts ILoggerFactoryAdapter sectionhandler returns
            LogManager.Reset(r);
            log = LogManager.GetLogger(typeof(LogManagerTests));
            ClassicAssert.AreEqual(typeof(TraceLogger), log.GetType());

            // accepts LogSetting sectionhandler returns
            LogManager.Reset(r);
            log = LogManager.GetLogger(typeof(LogManagerTests));
            ClassicAssert.AreEqual(typeof(ConsoleOutLogger), log.GetType());

            // every other return type throws ConfigurationException
            LogManager.Reset(r);
            _ = Assert.Throws<ConfigurationException>(() => log = LogManager.GetLogger<LogManagerTests>());
        }
    }

    [Test]
    public void ConfigureFromLogConfiguration()
    {
        ILog log;

        // accepts simple factory adapter
        LogManager.Configure(new LogConfiguration()
        {
            FactoryAdapter = new FactoryAdapterConfiguration()
            {
                Type = typeof(TraceLoggerFactoryAdapter).FullName
            }
        });
        log = LogManager.GetLogger<LogManagerTests>();
        ClassicAssert.AreEqual(typeof(TraceLogger), log.GetType());

        // accepts parameterized factory adapter
        LogManager.Configure(new LogConfiguration()
        {
            FactoryAdapter = new FactoryAdapterConfiguration()
            {
                Type = typeof(DebugLoggerFactoryAdapter).FullName,
                Arguments = new NameValueCollection
                {
                    { "level", "All" },
                    { "showDateTime", "true" },
                    { "showLogName", "true"},
                    { "showLevel", "true"},
                    { "dateTimeFormat", "yyyy/MM/dd hh:tt:ss.fff" }
                }
            }
        });
        log = LogManager.GetLogger<LogManagerTests>();
        ClassicAssert.AreEqual(typeof(DebugOutLogger), log.GetType());
        ClassicAssert.AreEqual(true, ((DebugOutLogger)log).ShowLogName);
    }

    [Test]
    public void ConfigureFromStandaloneConfig()
    {
        const string xml =
@"<?xml version='1.0' encoding='UTF-8' ?>
<logging>
    <factoryAdapter type='AddUp.CommonLogging.Simple.ConsoleOutLoggerFactoryAdapter, AddUp.CommonLogging'>
    </factoryAdapter>
</logging>";

        var log = GetLog(xml);
        ClassicAssert.IsAssignableFrom(typeof(ConsoleOutLogger), log);
    }

    [Test]
    public void InvalidAdapterType()
    {
        const string xml =
@"<?xml version='1.0' encoding='UTF-8' ?>
<logging>
    <factoryAdapter type='AddUp.CommonLogging.Simple.NonExistentAdapter, AddUp.CommonLogging'>
    </factoryAdapter>
</logging>";
        _ = Assert.Throws<ConfigurationException>(() => GetLog(xml));
    }

    [Test]
    public void AdapterDoesNotImplementInterface()
    {
        const string xml =
@"<?xml version='1.0' encoding='UTF-8' ?>
<logging>
    <factoryAdapter type='AddUp.CommonLogging.StandaloneConfigurationReader, UnitTests.AddUp.CommonLogging'>
    </factoryAdapter>
</logging>";
        _ = Assert.Throws<ConfigurationException>(() => GetLog(xml));
    }

    [Test]
    public void AdapterDoesNotHaveCorrectCtors()
    {
        const string xml =
@"<?xml version='1.0' encoding='UTF-8' ?>
<logging>
    <factoryAdapter type='AddUp.CommonLogging.Utils.MissingCtorFactoryAdapter, UnitTests.AddUp.CommonLogging'>
    </factoryAdapter>
</logging>";
        _ = Assert.Throws<ConfigurationException>(() => GetLog(xml));
    }

    [Test]
    public void AdapterDoesNotHaveCorrectCtorsWithArgs()
    {
        const string xml =
@"<?xml version='1.0' encoding='UTF-8' ?>
<logging>
    <factoryAdapter type='AddUp.CommonLogging.Utils.MissingCtorFactoryAdapter, UnitTests.AddUp.CommonLogging'>
        <arg key='level' value='DEBUG' />
    </factoryAdapter>
</logging>";
        _ = Assert.Throws<ConfigurationException>(() => GetLog(xml));
    }

    [Test]
    public void InvalidXmlSection()
    {
        const string xml =
@"<?xml version='1.0' encoding='UTF-8' ?>
<foo>
    <logging>
      <factoryAdapter type='AddUp.CommonLogging.MissingCtorFactoryAdapter, UnitTests.AddUp.CommonLogging'>
            <arg key='level' value='DEBUG' />
      </factoryAdapter>
    </logging>
</foo>";
        ILog log = GetLog(xml);
        // lack of proper config section fallsback to no-op logging.
        NoOpLogger noOpLogger = log as NoOpLogger;
        ClassicAssert.IsNotNull(noOpLogger);
    }

    private static ILog GetLog(string xml)
    {
        var configReader = new StandaloneConfigurationReader(xml);
        LogManager.Reset(configReader);
        return LogManager.GetLogger(typeof(LogManagerTests));
    }
}