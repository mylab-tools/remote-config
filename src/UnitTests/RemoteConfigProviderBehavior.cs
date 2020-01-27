using System;
using System.Text;
using Moq;
using MyLab.RemoteConfig;
using Xunit;

namespace UnitTests
{
    public class RemoteConfigProviderBehavior
    {
        [Fact]
        public void ShouldParseJson()
        {
            //Arrange
            var strJson = "{\"Root\":{\"Param1\":\"Val1\", \"Param2\":\"Val2\"}}";
            var binJson = Encoding.UTF8.GetBytes(strJson);

            var jsonProviderMock = new Mock<IConfigJsonProvider>();
            jsonProviderMock
                .Setup(p => p.Provide())
                .Returns(binJson);
            var jsonProvider = jsonProviderMock.Object;

            var remoteConfigProvider = new RemoteConfigProvider(jsonProvider);

            //Act
            remoteConfigProvider.Load();
            
            //Assert
            Assert.True(remoteConfigProvider.TryGet("Root:Param1", out var val1));
            Assert.Equal("Val1", val1);
            Assert.True(remoteConfigProvider.TryGet("Root:Param2", out var val2));
            Assert.Equal("Val2", val2);
        }
    }
}
