using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;
using NUnit.Framework;

namespace AddUp.CommonLogging;

[TestFixture, ExcludeFromCodeCoverage]
public class LoggingExceptionWithIndexerBugTests
{
    [Test]
    public void ErrorNotThrownWhenLoggedExceptionHasIndexerProperty()
    {
        ILog log = LogManager.GetLogger<LoggingExceptionWithIndexerBugTests>();
        var exception = new ExceptionWithIndexerException();
        Assert.That(() => log.Error("error catched", exception), Throws.Nothing);
    }

    [Serializable]
    public class ExceptionWithIndexerException : Exception
    {
        public ExceptionWithIndexerException() { }
        public ExceptionWithIndexerException(string message) : base(message) { }
        public ExceptionWithIndexerException(string message, Exception innerException) : base(message, innerException) { }
        protected ExceptionWithIndexerException(SerializationInfo info, StreamingContext context) : base(info, context) { }

        public string this[string key] => null;
    }
}
