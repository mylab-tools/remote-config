using System;
using Microsoft.Extensions.Configuration;

namespace MyLab.RemoteConfig
{
    class RemoteConfigSource : IConfigurationSource
    {
        private readonly IConfigJsonProvider _configJsonProvider;

        public RemoteConfigSource(IConfigJsonProvider configJsonProvider)
        {
            _configJsonProvider = configJsonProvider ?? throw new ArgumentNullException(nameof(configJsonProvider));
        }

        public IConfigurationProvider Build(IConfigurationBuilder builder)
        {
            return new RemoteConfigProvider(_configJsonProvider);
        }
    }
}