using System;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace MyLab.RemoteConfig
{
    /// <summary>
    /// Extends configuration system
    /// </summary>
    public static class ConfigurationExtensions
    {
        /// <summary>
        /// Inject remote configuration source into configuration system
        /// </summary>
        public static IConfigurationBuilder AddRemoteConfiguration(
            this IConfigurationBuilder configuration,
            RemoteConfigConnectionParameters connectionParameters)
        {
            if (configuration == null) throw new ArgumentNullException(nameof(configuration));
            if (connectionParameters == null) throw new ArgumentNullException(nameof(connectionParameters));

            var jsonProvider =new DefaultConfigJsonProvider(connectionParameters);

            configuration.Add(new RemoteConfigSource(jsonProvider));

            return configuration;
        }
    }

    /// <summary>
    /// Extends <see cref="IWebHostBuilder"/>
    /// </summary>
    public static class WebHostBuilderExtensions
    {
        /// <summary>
        /// Adds remote configuration from MyLab.ConfigServer
        /// </summary>
        public static IWebHostBuilder AddRemoteConfiguration(this IWebHostBuilder webHostBuilder, bool optional = false)
        {
            webHostBuilder.ConfigureAppConfiguration((ctx, cb) =>
            {
                var configuration = cb.Build();
                var remoteConnectionParameters = new RemoteConfigConnectionParameters();
                configuration.GetSection("RemoteConfig").Bind(remoteConnectionParameters);

                AddRemoteConfiguration(cb, remoteConnectionParameters, optional);
            });
            return webHostBuilder;
        }

        /// <summary>
        /// Adds remote configuration from MyLab.ConfigServer with connection parameters from environment variables
        /// </summary>
        public static IWebHostBuilder LoadRemoteConfigConnectionFromEnvironmentVars(this IWebHostBuilder webHostBuilder)
        {
            webHostBuilder.ConfigureAppConfiguration((ctx, cb) =>
            {
                cb.AddEnvironmentVariables("MYLAB_");
            });
            return webHostBuilder;
        }

        static void AddRemoteConfiguration(
            IConfigurationBuilder configuration,
            RemoteConfigConnectionParameters connectionParameters,
            bool optional)
        {
            if (configuration == null) throw new ArgumentNullException(nameof(configuration));
            if (connectionParameters == null) throw new ArgumentNullException(nameof(connectionParameters));

            var jsonProvider = new DefaultConfigJsonProvider(connectionParameters)
            {
                ThrowIfAddressNotSpecified = !optional
            };

            configuration.Add(new RemoteConfigSource(jsonProvider));
        }
    }
}
