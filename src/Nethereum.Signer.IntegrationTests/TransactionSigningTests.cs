using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Nethereum.Hex.HexConvertors.Extensions;
using Nethereum.XUnitEthereumClients;
using Xunit;

namespace Nethereum.Signer.IntegrationTests
{
    [Collection(EthereumClientIntegrationFixture.ETHEREUM_CLIENT_COLLECTION_DEFAULT)]
    public class TransactionSigningTests
    {

        private readonly EthereumClientIntegrationFixture _ethereumClientIntegrationFixture;

        public TransactionSigningTests(EthereumClientIntegrationFixture ethereumClientIntegrationFixture)
        {
            _ethereumClientIntegrationFixture = ethereumClientIntegrationFixture;
        }
 
        [Fact]
        public async Task<bool> ShouldSignAndSendRawTransaction()
        {
            var privateKey = "0xb5b1870957d373ef0eeffecc6e4812c0fd08f554b37b233526acc331bf1544f7";
            var senderAddress = "0x12890d2cce102216644c59daE5baed380d84830c";
            var receiveAddress = "0x13f022d72158410433cbd66f5dd8bf6d2d129924";
            var web3 = _ethereumClientIntegrationFixture.GetWeb3();

            var txCount = await web3.Eth.Transactions.GetTransactionCount.SendRequestAsync(senderAddress);
            var encoded = Web3.Web3.OfflineTransactionSigner.SignTransaction(privateKey, receiveAddress, 10,
                txCount.Value);

            Assert.True(Web3.Web3.OfflineTransactionSigner.VerifyTransaction(encoded));

            Debug.WriteLine(Web3.Web3.OfflineTransactionSigner.GetSenderAddress(encoded));
            Assert.Equal(senderAddress.EnsureHexPrefix().ToLower(),
                Web3.Web3.OfflineTransactionSigner.GetSenderAddress(encoded).EnsureHexPrefix().ToLower());

            var txId = await web3.Eth.Transactions.SendRawTransaction.SendRequestAsync("0x" + encoded);
            var receipt = await web3.Eth.Transactions.GetTransactionReceipt.SendRequestAsync(txId);
            while (receipt == null)
            {
                Thread.Sleep(1000);
                receipt = await web3.Eth.Transactions.GetTransactionReceipt.SendRequestAsync(txId);
            }

            Assert.Equal(txId, receipt.TransactionHash);
            return true;
        }
    }
}