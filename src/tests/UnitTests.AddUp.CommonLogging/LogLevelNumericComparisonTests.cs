using System.Diagnostics.CodeAnalysis;
using NUnit.Framework;

namespace AddUp.CommonLogging;

[TestFixture, ExcludeFromCodeCoverage]
public class LogLevelNumericComparisonTests
{
    [Test]
    [SuppressMessage("Major Code Smell", "S2589:Boolean expressions should not be gratuitous", Justification = "Tests")]
    public void IncreasingSeverityLogLevelsMaintainRelativeNumericOrdering() => Assert.That(
        LogLevel.All < LogLevel.Trace &&
        LogLevel.Trace < LogLevel.Debug &&
        LogLevel.Debug < LogLevel.Info &&
        LogLevel.Warn < LogLevel.Error &&
        LogLevel.Error < LogLevel.Fatal &&
        LogLevel.Fatal < LogLevel.Off);
}
