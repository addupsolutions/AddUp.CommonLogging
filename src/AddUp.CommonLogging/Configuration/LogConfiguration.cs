namespace AddUp.CommonLogging.Configuration
{
    /// <summary>
    /// JSON serializable object representing the configuration of the logging subsystem.
    /// May be passed to <see cref="LogManager.Configure"/>.
    /// </summary>
    public sealed class LogConfiguration
    {
        /// <summary>
        /// Defines the <see cref="ILoggerFactoryAdapter"/> used by the logging subsystem.
        /// </summary>
        public FactoryAdapterConfiguration FactoryAdapter { get; set; }
    }
}
