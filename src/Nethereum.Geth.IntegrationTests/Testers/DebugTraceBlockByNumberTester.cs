using System;
using System.Threading.Tasks;
using Nethereum.Geth.RPC.Debug;
using Nethereum.JsonRpc.Client;
using Nethereum.RPC.Tests.Testers;
using Newtonsoft.Json.Linq;
using Xunit;

namespace Nethereum.Geth.Tests.Testers
{
    public class DebugTraceBlockByNumberTester : RPCRequestTester<JObject>, IRPCRequestTester
    {
        public override async Task<JObject> ExecuteAsync(IClient client)
        {
            var debugTraceBlockByNumber = new DebugTraceBlockByNumber(client);
            return await debugTraceBlockByNumber.SendRequestAsync(Settings.GetBlockNumber());
        }

        public override Type GetRequestType()
        {
            return typeof(DebugTraceBlockByNumber);
        }


        [Fact]
        public async void ShouldDecodeTheBlockRplAsJObject()
        {
            var result = await ExecuteAsync();
            Assert.NotNull(result);
        }
    }
}