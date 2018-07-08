﻿using System.Threading;
using System.Threading.Tasks;
using Nethereum.Hex.HexTypes;
using Nethereum.RPC.Eth.DTOs;
using Nethereum.Web3.Accounts;
using Nethereum.XUnitEthereumClients;
using Xunit;

namespace Nethereum.Accounts.IntegrationTests
{
    [Collection(EthereumClientIntegrationFixture.ETHEREUM_CLIENT_COLLECTION_DEFAULT)]
    public class TransactionInterceptorTests
    {
        private readonly EthereumClientIntegrationFixture _ethereumClientIntegrationFixture;

        public TransactionInterceptorTests(EthereumClientIntegrationFixture ethereumClientIntegrationFixture)
        {
            _ethereumClientIntegrationFixture = ethereumClientIntegrationFixture;
        }

        [Fact]
        public async Task ShouldIntereceptAndSendRawTransaction()
        {
            var privateKey = "0xb5b1870957d373ef0eeffecc6e4812c0fd08f554b37b233526acc331bf1544f7";
            var senderAddress = "0x12890d2cce102216644c59daE5baed380d84830c";
            var receiveAddress = "0x13f022d72158410433cbd66f5dd8bf6d2d129924";
            var web3 = _ethereumClientIntegrationFixture.GetWeb3();
            var transactionInterceptor = new AccountTransactionSigningInterceptor(privateKey, web3.Client);
            web3.Client.OverridingRequestInterceptor = transactionInterceptor;

            var txId = await web3.Eth.Transactions.SendTransaction.SendRequestAsync(
                new TransactionInput {From = senderAddress, To = receiveAddress, Value = new HexBigInteger(10)});

            var receipt = await web3.Eth.Transactions.GetTransactionReceipt.SendRequestAsync(txId);
            while (receipt == null)
            {
                Thread.Sleep(1000);
                receipt = await web3.Eth.Transactions.GetTransactionReceipt.SendRequestAsync(txId);
            }
            Assert.Equal(txId, receipt.TransactionHash);
        }

        [Fact]
        public async Task ShouldIntereceptContractDeploymentAndContractTrasanctionWithRawTransaction()
        {
            var contractByteCode =
                "0x6060604052604060405190810160405280600a81526020017f4d756c7469706c6965720000000000000000000000000000000000000000000081526020015060016000509080519060200190828054600181600116156101000203166002900490600052602060002090601f016020900481019282601f1061008c57805160ff19168380011785556100bd565b828001600101855582156100bd579182015b828111156100bc57825182600050559160200191906001019061009e565b5b5090506100e891906100ca565b808211156100e457600081815060009055506001016100ca565b5090565b5050604051602080610303833981016040528080519060200190919050505b806000600050819055505b506101e2806101216000396000f360606040526000357c01000000000000000000000000000000000000000000000000000000009004806340490a901461004f57806375d0c0dc14610072578063c6888fa1146100ed5761004d565b005b61005c6004805050610119565b6040518082815260200191505060405180910390f35b61007f6004805050610141565b60405180806020018281038252838181518152602001915080519060200190808383829060006004602084601f0104600f02600301f150905090810190601f1680156100df5780820380516001836020036101000a031916815260200191505b509250505060405180910390f35b610103600480803590602001909190505061012b565b6040518082815260200191505060405180910390f35b60006000600050549050610128565b90565b60006000600050548202905061013c565b919050565b60016000508054600181600116156101000203166002900480601f0160208091040260200160405190810160405280929190818152602001828054600181600116156101000203166002900480156101da5780601f106101af576101008083540402835291602001916101da565b820191906000526020600020905b8154815290600101906020018083116101bd57829003601f168201915b50505050508156";

            var abi =
                @"[{""constant"":true,""inputs"":[],""name"":""getMultiplier"",""outputs"":[{""name"":""d"",""type"":""uint256""}],""type"":""function""},{""constant"":true,""inputs"":[],""name"":""contractName"",""outputs"":[{""name"":"""",""type"":""string""}],""type"":""function""},{""constant"":false,""inputs"":[{""name"":""a"",""type"":""uint256""}],""name"":""multiply"",""outputs"":[{""name"":""d"",""type"":""uint256""}],""type"":""function""},{""inputs"":[{""name"":""multiplier"",""type"":""uint256""}],""type"":""constructor""}]";


            var privateKey = "0xb5b1870957d373ef0eeffecc6e4812c0fd08f554b37b233526acc331bf1544f7";
            var senderAddress = "0x12890d2cce102216644c59daE5baed380d84830c";

            var web3 = _ethereumClientIntegrationFixture.GetWeb3();
            var transactionInterceptor = new AccountTransactionSigningInterceptor(privateKey, web3.Client);
            web3.Client.OverridingRequestInterceptor = transactionInterceptor;

            var txId = await web3.Eth.DeployContract.SendRequestAsync(abi, contractByteCode, senderAddress,
                new HexBigInteger(900000), 7);

            var receipt = await web3.Eth.Transactions.GetTransactionReceipt.SendRequestAsync(txId);
            while (receipt == null)
            {
                Thread.Sleep(1000);
                receipt = await web3.Eth.Transactions.GetTransactionReceipt.SendRequestAsync(txId);
            }

            Assert.Equal(txId, receipt.TransactionHash);
            Assert.NotNull(receipt.ContractAddress);

            var contract = web3.Eth.GetContract(abi, receipt.ContractAddress);
            var multiplyFunction = contract.GetFunction("multiply");

            txId = await multiplyFunction.SendTransactionAsync(senderAddress, new HexBigInteger(900000),
                new HexBigInteger(0), 69);

            receipt = await web3.Eth.Transactions.GetTransactionReceipt.SendRequestAsync(txId);
            while (receipt == null)
            {
                Thread.Sleep(1000);
                receipt = await web3.Eth.Transactions.GetTransactionReceipt.SendRequestAsync(txId);
            }

            Assert.Equal(txId, receipt.TransactionHash);
        }
    }
}