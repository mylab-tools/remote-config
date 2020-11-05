using System;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

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

                RemoteConfigAdder.AddRemoteConfiguration(cb, remoteConnectionParameters, optional);
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
    }

    /// <summary>
    /// Extends <see cref="IHostBuilder"/>
    /// </summary>
    public static class HostBuilderExtensions
    {
        /// <summary>
        /// Adds remote configuration from MyLab.ConfigServer
        /// </summary>
        public static IHostBuilder AddRemoteConfiguration(this IHostBuilder hostBuilder, bool optional = false)
        {
            hostBuilder.ConfigureAppConfiguration((ctx, cb) =>
            {
                var configuration = cb.Build();
                var remoteConnectionParameters = new RemoteConfigConnectionParameters();
                configuration.GetSection("RemoteConfig").Bind(remoteConnectionParameters);

                RemoteConfigAdder.AddRemoteConfiguration(cb, remoteConnectionParameters, optional);
            });
            return hostBuilder;
        }

        /// <summary>
        /// Adds remote configuration from MyLab.ConfigServer with connection parameters from environment variables
        /// </summary>
        public static IHostBuilder LoadRemoteConfigConnectionFromEnvironmentVars(this IHostBuilder hostBuilder)
        {
            hostBuilder.ConfigureAppConfiguration((ctx, cb) =>
            {
                cb.AddEnvironmentVariables("MYLAB_");
            });
            return hostBuilder;
        }
    }

    static class RemoteConfigAdder
    {
        public static void AddRemoteConfiguration(
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
