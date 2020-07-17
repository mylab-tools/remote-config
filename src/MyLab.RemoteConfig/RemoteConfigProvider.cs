using System;
using System.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;

namespace MyLab.RemoteConfig
{
    class RemoteConfigProvider : ConfigurationProvider 
    {
        private readonly IConfigJsonProvider _configJsonProvider;

        public RemoteConfigProvider(IConfigJsonProvider configJsonProvider)
        {
            _configJsonProvider = configJsonProvider ?? throw new ArgumentNullException(nameof(configJsonProvider));
        }

        public override void Load()
        {
            var config = _configJsonProvider.Provide();

            if (config != null)
            {
                using (var mem = new MemoryStream(config))
                {
                    Data = JsonConfigurationFileParser.Parse(mem);
                }
            }
        }
    }
}
