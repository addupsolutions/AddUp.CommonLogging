using System;
using System.Diagnostics.CodeAnalysis;
using AddUp.CommonLogging.Factory;
using NUnit.Framework;

namespace AddUp.CommonLogging.Utils;

[ExcludeFromCodeCoverage]
public abstract class AbstractTestLogger : AbstractLogger
{
    public override bool IsTraceEnabled => true;
    public override bool IsDebugEnabled => true;
    public override bool IsInfoEnabled => true;
    public override bool IsWarnEnabled => true;
    public override bool IsErrorEnabled => true;
    public override bool IsFatalEnabled => true;

    public abstract void Log(LogLevel level, object message, Exception exception);

    protected override void WriteInternal(LogLevel level, object message, Exception exception) =>
        Log(level, message, exception);
}

[ExcludeFromCodeCoverage]
public sealed class TestLogger : AbstractTestLogger
{
    public LogLevel LastLogLevel;
    public string LastMessage;
    public Exception LastException;

    public override void Log(LogLevel level, object message, Exception exception)
    {
        LastLogLevel = level;
        LastMessage = message.ToString();
        LastException = exception;
    }

    protected override WriteHandler GetWriteHandler() => new(Log);

    protected override void WriteInternal(LogLevel level, object message, Exception exception) =>
        Assert.Fail("must never been called - Log() should be called");
}
