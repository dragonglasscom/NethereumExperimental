using System;
using System.Threading.Tasks;
using Nethereum.Geth.RPC.Debug;
using Nethereum.JsonRpc.Client;
using Nethereum.RPC.Tests.Testers;
using Xunit;

namespace Nethereum.Geth.Tests.Testers
{
    public class DebugStacksTester : RPCRequestTester<string>, IRPCRequestTester
    {
        public override async Task<string> ExecuteAsync(IClient client)
        {
            var debugStacks = new DebugStacks(client);
            return await debugStacks.SendRequestAsync();
        }

        public override Type GetRequestType()
        {
            return typeof(DebugStacks);
        }

        [Fact]
        public async void ShouldReturnStacksAsString()
        {
            var result = await ExecuteAsync();
            Assert.NotNull(result);
        }
    }
}