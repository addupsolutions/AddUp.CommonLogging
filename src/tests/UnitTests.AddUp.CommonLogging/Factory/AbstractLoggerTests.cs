using System;
using System.Collections;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Reflection;
using AddUp.CommonLogging.Utils;
using NUnit.Framework;
using NUnit.Framework.Legacy;
using Rhino.Mocks;
using Rhino.Mocks.Interfaces;
using FormatMessageCallback = System.Action<AddUp.CommonLogging.FormatMessageHandler>;
using Is = Rhino.Mocks.Constraints.Is;

namespace AddUp.CommonLogging.Factory;

[TestFixture, ExcludeFromCodeCoverage]
public class AbstractLoggerTests
{
    private sealed class TestFormatMessageCallback
    {
        private readonly bool throwOnInvocation;
        private readonly string messageToReturn;
        private readonly object[] argsToReturn;

        private TestFormatMessageCallback(bool shouldThrowOnInvocation) =>
            throwOnInvocation = shouldThrowOnInvocation;

        private TestFormatMessageCallback(string theMessageToReturn, object[] args)
        {
            messageToReturn = theMessageToReturn;
            argsToReturn = args;
        }

        public static FormatMessageCallback FailCallback() => new(new TestFormatMessageCallback(true).FormatMessage);
        public static FormatMessageCallback MessageCallback(string message, params object[] args) => new(new TestFormatMessageCallback(message, args).FormatMessage);

        private void FormatMessage(FormatMessageHandler fmh)
        {
            if (throwOnInvocation)
                Assert.Fail();

            _ = fmh(messageToReturn, argsToReturn);
        }
    }

    private static readonly Type[][] methodSignatures =
    {
        new[] {typeof (object)},
        new[] {typeof (object), typeof (Exception)},
        new[] {typeof (string), typeof (object[])},
        new[] {typeof (string), typeof (Exception), typeof (object[])},
        new[] {typeof (IFormatProvider), typeof (string), typeof (object[])},
        new[] {typeof (IFormatProvider), typeof (string), typeof (Exception), typeof (object[])},
        new[] {typeof (FormatMessageCallback)},
        new[] {typeof (FormatMessageCallback), typeof (Exception)},
        new[] {typeof (IFormatProvider), typeof (FormatMessageCallback)},
        new[] {typeof (IFormatProvider), typeof (FormatMessageCallback), typeof (Exception)}
    };

    [Test]
    public void IsSerializable() => ClassicAssert.IsTrue(SerializationTestUtils.IsSerializable<AbstractLogger>());

    [Test]
    public void ImplementsAllMethodsForAllLevels()
    {
        var logLevels = Exclude(Enum.GetNames(typeof(LogLevel)), "All", "Off");

        foreach (string logLevel in logLevels)
        {
            var logMethods = GetLogMethodSignatures(logLevel);
            for (var i = 0; i < logLevels.Length; i++)
                ClassicAssert.IsNotNull(logMethods[i], "Method with signature #{0} not implemented for level {1}", i, logLevel);
        }
    }

    [Test]
    public void LogsMessage()
    {
        var logLevels = Exclude(Enum.GetNames(typeof(LogLevel)), "All", "Off");
        foreach (string logLevel in logLevels)
            LogsMessage(logLevel);
    }

    [Test]
    public void WriteIsCalledWithCorrectLogLevel()
    {
        var logLevels = Exclude(Enum.GetNames(typeof(LogLevel)), "All", "Off");
        foreach (string logLevel in logLevels)
            WriteIsCalledWithCorrectLogLevel(logLevel);
    }

    [Test]
    public void WriteAndEvaluateOnlyWhenLevelEnabled()
    {
        var logLevels = Exclude(Enum.GetNames(typeof(LogLevel)), "All", "Off");
        foreach (string logLevel in logLevels)
            WriteAndEvaluateOnlyWhenLevelEnabled(logLevel);
    }

    /// <summary>
    /// Ensures, that all interface methods delegate to Write() with correct level + arguments
    /// and that arguments are still not evaluated up to this point (e.g. calling ToString())
    /// </summary>
    [SuppressMessage("Minor Code Smell", "S3878:Arrays should not be created for params parameters", Justification = "False positive, this is an array considered as one parameter")]
    private static void LogsMessage(string levelName)
    {
        var log = new TestLogger();
        var ex = new Exception();

        var logMethods = GetLogMethodSignatures(levelName);
        var logLevel = (LogLevel)Enum.Parse(typeof(LogLevel), levelName);

        Invoke(log, logMethods[0], "messageObject0");
        ClassicAssert.AreEqual(logLevel, log.LastLogLevel);
        ClassicAssert.AreEqual("messageObject0", log.LastMessage);
        ClassicAssert.AreEqual(null, log.LastException);

        Invoke(log, logMethods[1], "messageObject1", ex);
        ClassicAssert.AreEqual(logLevel, log.LastLogLevel);
        ClassicAssert.AreEqual("messageObject1", log.LastMessage);
        ClassicAssert.AreEqual(ex, log.LastException);

        Invoke(log, logMethods[2], "format2 {0}", new object[] { "arg2" });
        ClassicAssert.AreEqual(logLevel, log.LastLogLevel);
        ClassicAssert.AreEqual("format2 arg2", log.LastMessage);
        ClassicAssert.AreEqual(null, log.LastException);

        Invoke(log, logMethods[3], "format3 {0}", ex, new object[] { "arg3" });
        ClassicAssert.AreEqual(logLevel, log.LastLogLevel);
        ClassicAssert.AreEqual("format3 arg3", log.LastMessage);
        ClassicAssert.AreEqual(ex, log.LastException);

        Invoke(log, logMethods[4], CultureInfo.CreateSpecificCulture("de-de"), "format4 {0}", new object[] { 4.1 });
        ClassicAssert.AreEqual(logLevel, log.LastLogLevel);
        ClassicAssert.AreEqual("format4 4,1", log.LastMessage);
        ClassicAssert.AreEqual(null, log.LastException);

        Invoke(log, logMethods[5], CultureInfo.CreateSpecificCulture("de-de"), "format5 {0}", ex, new object[] { 5.1 });
        ClassicAssert.AreEqual(logLevel, log.LastLogLevel);
        ClassicAssert.AreEqual("format5 5,1", log.LastMessage);
        ClassicAssert.AreEqual(ex, log.LastException);

        Invoke(log, logMethods[6], TestFormatMessageCallback.MessageCallback("message6"));
        ClassicAssert.AreEqual(logLevel, log.LastLogLevel);
        ClassicAssert.AreEqual("message6", log.LastMessage);
        ClassicAssert.AreEqual(null, log.LastException);

        Invoke(log, logMethods[7], TestFormatMessageCallback.MessageCallback("message7"), ex);
        ClassicAssert.AreEqual(logLevel, log.LastLogLevel);
        ClassicAssert.AreEqual("message7", log.LastMessage);
        ClassicAssert.AreEqual(ex, log.LastException);

        Invoke(log, logMethods[8], CultureInfo.CreateSpecificCulture("de-de"), TestFormatMessageCallback.MessageCallback("format8 {0}", new object[] { 8.1 }));
        ClassicAssert.AreEqual(logLevel, log.LastLogLevel);
        ClassicAssert.AreEqual("format8 8,1", log.LastMessage);
        ClassicAssert.AreEqual(null, log.LastException);

        Invoke(log, logMethods[9], CultureInfo.CreateSpecificCulture("de-de"), TestFormatMessageCallback.MessageCallback("format9 {0}", 9.1), ex);
        ClassicAssert.AreEqual(logLevel, log.LastLogLevel);
        ClassicAssert.AreEqual("format9 9,1", log.LastMessage);
        ClassicAssert.AreEqual(ex, log.LastException);
    }

    /// <summary>
    /// Ensures, that all interface methods delegate to Write() with correct level + arguments
    /// and that arguments are still not evaluated up to this point (e.g. calling ToString())
    /// </summary>
    [SuppressMessage("Minor Code Smell", "S3878:Arrays should not be created for params parameters", Justification = "False positive, this is an array considered as one parameter")]
    private static void WriteIsCalledWithCorrectLogLevel(string levelName)
    {
        var mocks = new MockRepository();

        var log = (AbstractTestLogger)mocks.PartialMock(typeof(AbstractTestLogger));
        var ex = (Exception)mocks.StrictMock(typeof(Exception));
        var messageObject = mocks.StrictMock(typeof(object));
        var formatArg = mocks.StrictMock(typeof(object));
        var failCallback = TestFormatMessageCallback.FailCallback();

        var logMethods = GetLogMethodSignatures(levelName);
        var logLevel = (LogLevel)Enum.Parse(typeof(LogLevel), levelName);

        using (mocks.Ordered())
        {
            log.Log(logLevel, null, null);
            _ = LastCall.Constraints(Is.Equal(logLevel), Is.Anything(), Is.Null());
            log.Log(logLevel, null, ex);
            _ = LastCall.Constraints(Is.Equal(logLevel), Is.Anything(), Is.Same(ex));
            log.Log(logLevel, null, null);
            _ = LastCall.Constraints(Is.Equal(logLevel), Is.Anything(), Is.Null());
            log.Log(logLevel, null, ex);
            _ = LastCall.Constraints(Is.Equal(logLevel), Is.Anything(), Is.Same(ex));
            log.Log(logLevel, null, null);
            _ = LastCall.Constraints(Is.Equal(logLevel), Is.Anything(), Is.Null());
            log.Log(logLevel, null, ex);
            _ = LastCall.Constraints(Is.Equal(logLevel), Is.Anything(), Is.Same(ex));
            log.Log(logLevel, null, null);
            _ = LastCall.Constraints(Is.Equal(logLevel), Is.Anything(), Is.Null());
            log.Log(logLevel, null, ex);
            _ = LastCall.Constraints(Is.Equal(logLevel), Is.Anything(), Is.Same(ex));
            log.Log(logLevel, null, null);
            _ = LastCall.Constraints(Is.Equal(logLevel), Is.Anything(), Is.Null());
            log.Log(logLevel, null, ex);
            _ = LastCall.Constraints(Is.Equal(logLevel), Is.Anything(), Is.Same(ex));
        }

        mocks.ReplayAll();

        Invoke(log, logMethods[0], messageObject);
        Invoke(log, logMethods[1], messageObject, ex);
        Invoke(log, logMethods[2], "format", new object[] { formatArg });
        Invoke(log, logMethods[3], "format", ex, new object[] { formatArg });
        Invoke(log, logMethods[4], CultureInfo.InvariantCulture, "format", new object[] { formatArg });
        Invoke(log, logMethods[5], CultureInfo.InvariantCulture, "format", ex, new object[] { formatArg });
        Invoke(log, logMethods[6], failCallback);
        Invoke(log, logMethods[7], failCallback, ex);
        Invoke(log, logMethods[8], CultureInfo.InvariantCulture, failCallback);
        Invoke(log, logMethods[9], CultureInfo.InvariantCulture, failCallback, ex);

        mocks.VerifyAll();
    }

    /// <summary>
    /// This test ensures, that for a given loglevel
    /// a) <c>AbstractLogger.Write</c> is not called if that loglevel is disabled
    /// b) No argument is evaluated (e.g. calling ToString()) if that loglevel is disabled
    /// </summary>
    [SuppressMessage("Minor Code Smell", "S3878:Arrays should not be created for params parameters", Justification = "False positive, this is an array considered as one parameter")]
    private static void WriteAndEvaluateOnlyWhenLevelEnabled(string levelName)
    {
        var mocks = new MockRepository();
        var log = (AbstractLogger)mocks.StrictMock(typeof(AbstractLogger));
        var ex = (Exception)mocks.StrictMock(typeof(Exception));
        var messageObject = mocks.StrictMock(typeof(object));
        var formatArg = mocks.StrictMock(typeof(object));
        var failCallback = TestFormatMessageCallback.FailCallback();

        var logMethods = GetLogMethodSignatures(levelName);

        using (mocks.Ordered())
        {
            Invoke(log, logMethods[0], messageObject);
            _ = LastCall.CallOriginalMethod(OriginalCallOptions.CreateExpectation);
            _ = Expect.Call(IsLevelEnabled(log, levelName)).Return(false);
            Invoke(log, logMethods[1], messageObject, ex);
            _ = LastCall.CallOriginalMethod(OriginalCallOptions.CreateExpectation);
            _ = Expect.Call(IsLevelEnabled(log, levelName)).Return(false);
            Invoke(log, logMethods[2], "format", new object[] { formatArg });
            _ = LastCall.CallOriginalMethod(OriginalCallOptions.CreateExpectation);
            _ = Expect.Call(IsLevelEnabled(log, levelName)).Return(false);
            Invoke(log, logMethods[3], "format", ex, new object[] { formatArg });
            _ = LastCall.CallOriginalMethod(OriginalCallOptions.CreateExpectation);
            _ = Expect.Call(IsLevelEnabled(log, levelName)).Return(false);
            Invoke(log, logMethods[4], CultureInfo.InvariantCulture, "format", new object[] { formatArg });
            _ = LastCall.CallOriginalMethod(OriginalCallOptions.CreateExpectation);
            _ = Expect.Call(IsLevelEnabled(log, levelName)).Return(false);
            Invoke(log, logMethods[5], CultureInfo.InvariantCulture, "format", ex, new object[] { formatArg });
            _ = LastCall.CallOriginalMethod(OriginalCallOptions.CreateExpectation);
            _ = Expect.Call(IsLevelEnabled(log, levelName)).Return(false);
            Invoke(log, logMethods[6], failCallback);
            _ = LastCall.CallOriginalMethod(OriginalCallOptions.CreateExpectation);
            _ = Expect.Call(IsLevelEnabled(log, levelName)).Return(false);
            Invoke(log, logMethods[7], failCallback, ex);
            _ = LastCall.CallOriginalMethod(OriginalCallOptions.CreateExpectation);
            _ = Expect.Call(IsLevelEnabled(log, levelName)).Return(false);
            Invoke(log, logMethods[8], CultureInfo.InvariantCulture, failCallback);
            _ = LastCall.CallOriginalMethod(OriginalCallOptions.CreateExpectation);
            _ = Expect.Call(IsLevelEnabled(log, levelName)).Return(false);
            Invoke(log, logMethods[9], CultureInfo.InvariantCulture, failCallback, ex);
            _ = LastCall.CallOriginalMethod(OriginalCallOptions.CreateExpectation);
            _ = Expect.Call(IsLevelEnabled(log, levelName)).Return(false);
        }

        mocks.ReplayAll();

        Invoke(log, logMethods[0], messageObject);
        Invoke(log, logMethods[1], messageObject, ex);
        Invoke(log, logMethods[2], "format", new object[] { formatArg });
        Invoke(log, logMethods[3], "format", ex, new object[] { formatArg });
        Invoke(log, logMethods[4], CultureInfo.InvariantCulture, "format", new object[] { formatArg });
        Invoke(log, logMethods[5], CultureInfo.InvariantCulture, "format", ex, new object[] { formatArg });
        Invoke(log, logMethods[6], failCallback);
        Invoke(log, logMethods[7], failCallback, ex);
        Invoke(log, logMethods[8], CultureInfo.InvariantCulture, failCallback);
        Invoke(log, logMethods[9], CultureInfo.InvariantCulture, failCallback, ex);

        mocks.VerifyAll();
    }

    private static bool IsLevelEnabled(ILog log, string logLevelName) => logLevelName switch
    {
        "Trace" => log.IsTraceEnabled,
        "Debug" => log.IsDebugEnabled,
        "Info" => log.IsInfoEnabled,
        "Warn" => log.IsWarnEnabled,
        "Error" => log.IsErrorEnabled,
        "Fatal" => log.IsFatalEnabled,
        _ => throw new ArgumentOutOfRangeException(nameof(logLevelName), logLevelName, "unknown log level"),
    };

    private static MethodInfo[] GetLogMethodSignatures(string levelName) => new[]
    {
        typeof (ILog).GetMethod(levelName, methodSignatures[0]),
        typeof (ILog).GetMethod(levelName, methodSignatures[1]),
        typeof (ILog).GetMethod(levelName + "Format", methodSignatures[2]),
        typeof (ILog).GetMethod(levelName + "Format", methodSignatures[3]),
        typeof (ILog).GetMethod(levelName + "Format", methodSignatures[4]),
        typeof (ILog).GetMethod(levelName + "Format", methodSignatures[5]),
        typeof (ILog).GetMethod(levelName, methodSignatures[6]),
        typeof (ILog).GetMethod(levelName, methodSignatures[7]),
        typeof (ILog).GetMethod(levelName, methodSignatures[8]),
        typeof (ILog).GetMethod(levelName, methodSignatures[9])
    };

    private static void Invoke(object target, MethodInfo method, params object[] args) => method.Invoke(target, args);

    private static string[] Exclude(string[] arr, params string[] exclude)
    {
        var result = new ArrayList();
        foreach (string s in arr)
            if (0 > Array.BinarySearch(exclude, s))
                _ = result.Add(s);

        return (string[])result.ToArray(typeof(string));
    }
}
