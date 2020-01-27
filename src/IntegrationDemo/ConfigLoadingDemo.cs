using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using IntegrationTestServer;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using Xunit;
using Xunit.Abstractions;

namespace IntegrationDemo
{
    public class ConfigLoadingDemo : IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly WebApplicationFactory<Startup> _factory;
        private readonly ITestOutputHelper _output;

        public ConfigLoadingDemo(WebApplicationFactory<Startup> factory, ITestOutputHelper output)
        {
            _factory = factory;
            _output = output;

            Environment.SetEnvironmentVariable("MYLAB_REMOTECONFIG__URL", "http://localhost:54004");
            Environment.SetEnvironmentVariable("MYLAB_REMOTECONFIG__USER", "foo");
            Environment.SetEnvironmentVariable("MYLAB_REMOTECONFIG__PASSWORD", "right-pass");
        }

        [Fact]
        public async Task ConfigLoading()
        {
            //Arrange
            var client = _factory.CreateClient();

            //Act
            var resp = await client.GetAsync("api/config");

            _output.WriteLine($"Response code: {(int)resp.StatusCode}({resp.StatusCode})\n");

            //Assert
            Assert.True(resp.IsSuccessStatusCode);

            _output.WriteLine(JsonPrettify(await resp.Content.ReadAsStringAsync()));
        }

        public static string JsonPrettify(string json)
        {
            using var stringReader = new StringReader(json);
            using var stringWriter = new StringWriter();

            var jsonReader = new JsonTextReader(stringReader);
            var jsonWriter = new JsonTextWriter(stringWriter) { Formatting = Formatting.Indented };
            jsonWriter.WriteToken(jsonReader);
            return stringWriter.ToString();
        }
    }
}
