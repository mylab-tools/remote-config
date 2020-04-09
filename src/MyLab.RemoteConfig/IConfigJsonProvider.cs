using System;
using System.Net;
using System.Text;

namespace MyLab.RemoteConfig
{
    /// <summary>
    /// Defines remote configuration json provider
    /// </summary>
    public interface IConfigJsonProvider
    {
        /// <summary>
        /// Provides binary json configuration
        /// </summary>
        byte[] Provide();
    }

    class DefaultConfigJsonProvider : IConfigJsonProvider
    {
        private readonly RemoteConfigConnectionParameters _connectionParameters;

        public DefaultConfigJsonProvider(RemoteConfigConnectionParameters connectionParameters)
        {
            _connectionParameters = connectionParameters;
        }

        public byte[] Provide()
        {
            var baseAddress = _connectionParameters.Url ?? $"http://{_connectionParameters.Host}/api/config";

            var client = new WebClient
            {
                BaseAddress = baseAddress
            };

            string credentials = Convert.ToBase64String(Encoding.UTF8.GetBytes(
                $"{_connectionParameters.User}:{_connectionParameters.Password}"));
            client.Headers[HttpRequestHeader.Authorization] = $"Basic {credentials}";

            try
            {
                return client.DownloadData("");
            }
            catch (WebException e)
            {
                throw new InvalidOperationException("Remote config loading error", e);
            }
        }
    }
}