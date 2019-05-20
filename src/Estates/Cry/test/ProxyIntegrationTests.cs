using Gamer.Proxy;
using Gamer.Proxy.Server;
using System;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace Gamer.Estate.Cry.Tests
{
    public class TestsFixture : IDisposable
    {
        public readonly ProxyTarget Target = new ProxyTarget
        {
            Host = "127.0.0.1",
            Port = HttpServer.FindFreeTcpPort(),
        };

        public TestsFixture() => Target.Initialize(new CryEstateHandler());
        public void Dispose() => Target.Dispose();
    }

    public class ProxyIntegrationTests : IClassFixture<TestsFixture>
    {
        readonly TestsFixture _fixture;
        public ProxyIntegrationTests(TestsFixture fixture, ITestOutputHelper helper) { _fixture = fixture; Core.Debug.LogFunc = x => helper.WriteLine(x.ToString()); }

        [Theory]
        [InlineData("http://localhost:{0}/Abc", "xxx")]
        public async Task LoadAssetPack(string path, string modelPath)
        {
            // given
            var uri = new Uri(string.Format(path, _fixture.Target.Port));
            var assetPack = await uri.GetCryAssetPackAsync();
            // when
            var exists = assetPack.ContainsFile(modelPath);
            // then
            Assert.True(exists);
        }
    }
}
