using System;

namespace AddUp.CommonLogging.Configuration
{
    /// <summary>
    /// Implementation of <see cref="IConfigurationReader"/> that uses a supplied
    /// <see cref="LogConfiguration"/> object.
    /// </summary>
    /// <author>Brant Burnett</author>
    public sealed class LogConfigurationReader : IConfigurationReader
    {
        private readonly LogConfiguration configuration;

        /// <summary>
        /// Creates a new <see cref="LogConfigurationReader"/> given a <see cref="LogConfiguration"/> object.
        /// </summary>
        /// <param name="logConfiguration"><see cref="LogConfiguration"/> to be parsed.</param>
        public LogConfigurationReader(LogConfiguration logConfiguration) =>
            configuration = logConfiguration ?? throw new ArgumentNullException(nameof(logConfiguration));

        /// <summary>
        /// Returns a <see cref="LogSetting"/> based on the <see cref="LogConfiguration"/> supplied
        /// in the constructor.
        /// </summary>
        /// <param name="sectionName">This parameter is not used in this implementation.</param>
        /// <returns><see cref="LogSetting"/> based on the supplied configuration.</returns>
        public object GetSection(string sectionName)
        {
            if (configuration.FactoryAdapter == null)
                throw new ConfigurationException("LogConfiguration.FactoryAdapter is required.");

            if (string.IsNullOrEmpty(configuration.FactoryAdapter.Type))
                throw new ConfigurationException("LogConfiguration.FactoryAdapter.Type is required.");


            Type factoryType;
            try
            {
                factoryType = Type.GetType(configuration.FactoryAdapter.Type, true);
            }
            catch (Exception ex)
            {
                throw new ConfigurationException($"Unable to create type '{configuration.FactoryAdapter.Type}': {ex.Message}", ex);
            }

            return new LogSetting(factoryType, configuration.FactoryAdapter.Arguments);
        }
    }
}
