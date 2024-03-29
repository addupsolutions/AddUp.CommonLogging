using System;
using System.Diagnostics.CodeAnalysis;
using NUnit.Framework;
#if !NET8_0_OR_GREATER
using System.Runtime.Serialization;
#endif

namespace AddUp.CommonLogging;

[TestFixture, ExcludeFromCodeCoverage]
public class LoggingExceptionWithIndexerBugTests
{
    [Test]
    public void ErrorNotThrownWhenLoggedExceptionHasIndexerProperty()
    {
        var log = LogManager.GetLogger<LoggingExceptionWithIndexerBugTests>();
        var exception = new ExceptionWithIndexerException();
        Assert.That(() => log.Error("error catched", exception), Throws.Nothing);
    }

#if !NET8_0_OR_GREATER
    [Serializable]
#endif
    public class ExceptionWithIndexerException : Exception
    {
        public ExceptionWithIndexerException() { }
        public ExceptionWithIndexerException(string message) : base(message) { }
        public ExceptionWithIndexerException(string message, Exception innerException) : base(message, innerException) { }

#if !NET8_0_OR_GREATER
        protected ExceptionWithIndexerException(SerializationInfo info, StreamingContext context) : base(info, context) { }
#endif

        public string this[string key] => null;
    }
}