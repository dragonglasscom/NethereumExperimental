﻿using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Reflection;
using System.Threading.Tasks;
using Nethereum.ABI.FunctionEncoding.Attributes;
using Nethereum.Contracts.CQS;
using Nethereum.Hex.HexTypes;
using Nethereum.Web3.Accounts;
using Nethereum.XUnitEthereumClients;
using Xunit;

namespace Nethereum.Accounts.IntegrationTests
{
    [Collection(EthereumClientIntegrationFixture.ETHEREUM_CLIENT_COLLECTION_DEFAULT)]
    public class ABIStructsTests
    {
        /*
       pragma solidity 0.4.23;
        pragma experimental ABIEncoderV2;

        contract TestV2
        {
   
            uint256 public id1 = 1;
            uint256 public id2;
            uint256 public id3;
            string  public id4;
            TestStruct public testStructStorage;
            event TestStructStorageChanged(address sender, TestStruct testStruct);
    
            struct SubSubStruct {
                uint256 id;
            }
   
            struct SubStruct {
                uint256 id;
                SubSubStruct sub;
                string id2;
            }

            struct TestStruct {
                uint256 id;
                SubStruct subStruct1;
                SubStruct subStruct2;
                string id2;
            }
    
            function Test(TestStruct testScrut) public {
                id1 = testScrut.id;
                id2 = testScrut.subStruct1.id;
                id3 = testScrut.subStruct2.sub.id;
                id4 = testScrut.subStruct2.id2;
        
            }
    
            function SetStorageStruct(TestStruct testStruct) public {
                testStructStorage = testStruct;
                emit TestStructStorageChanged(msg.sender, testStruct);
            }
    
            function GetTest() public view returns(TestStruct testStruct, int test1, int test2){
                testStruct.id = 1;
                testStruct.id2 = "hello";
                testStruct.subStruct1.id = 200;
                testStruct.subStruct1.id2 = "Giraffe";
                testStruct.subStruct1.sub.id = 20;
                testStruct.subStruct2.id = 300;
                testStruct.subStruct2.id2 = "Elephant";
                testStruct.subStruct2.sub.id = 30000;
                test1 = 5;
                test2 = 6;
            }
        
                struct Empty{
            
                }
        
                function TestEmpty(Empty empty) public {
            
                }
}
        */
        [Fact]
        public async Task StringTest()
        {
            await SuperFunSampleStringAsync();
        }


        [Function("id1", "uint256")]
        public class Id1Function : ContractMessage
        {

        }

        [Function("id2", "uint256")]
        public class Id2Function : ContractMessage
        {

        }

        [Function("id3", "uint256")]
        public class Id3Function : ContractMessage
        {

        }

        [Function("id4", "string")]
        public class Id4Function : ContractMessage
        {

        }

        [Function("GetTest")]
        public class GetTestFunction : ContractMessage
        {
            
        }

        [Function("testStructStorage")]
        public class GetTestStructStorageFunction : ContractMessage
        {

        }

       

        [FunctionOutput]
        public class GetTestFunctionOuptputDTO
        {
            [Parameter("tuple")]
            public TestStructStrings TestStruct { get; set; }


            [Parameter("int256", "test1", 2)]
            public BigInteger Test1 { get; set; }


            [Parameter("int256", "test2", 3)]
            public BigInteger Test2 { get; set; }
        }

        [Function("Test")]
        public class TestFunction : ContractMessage
        {
            [Parameter("tuple", "testStruct")]
            public TestStructStrings TestStruct { get; set; }
        }

        [Function("SetStorageStruct")]
        public class SetStorageStructFunction : ContractMessage
        {
            [Parameter("tuple", "testStruct")]
            public TestStructStrings TestStruct { get; set; }
        }

        [Event("TestStructStorageChanged")]
        public class TestStructStorageChangedEvent
        {
            [Parameter("address", "sender", 1)]
            public string Address { get; set; }

            [Parameter("tuple", "testStruct", 2)]
            public TestStructStrings TestStruct { get; set; }
        }


        [FunctionOutput]
        public class TestStructStrings
        {
            [Parameter("uint256", "id", 1)]
            public BigInteger Id { get; set; }

            [Parameter("tuple", "subStruct1", 2)]
            public SubStructUintString SubStruct1 { get; set; }

            [Parameter("tuple", "subStruct2", 3)]
            public SubStructUintString SubStruct2 { get; set; }

            [Parameter("string", "id2", 4)]
            public string Id2 { get; set; }
        }


        public class SubStructUintString
        {
            [Parameter("uint256", "id", 1)]
            public BigInteger Id { get; set; }

            [Parameter("tuple", "sub", 2)]
            public SubStructUInt Sub { get; set; }

            [Parameter("string", "id2", 3)]
            public String Id2 { get; set; }
        }

        public class SubStructUInt
        {
            [Parameter("uint256", "id", 1)]
            public BigInteger Id { get; set; }
        }

        public class TestContractDeployment : ContractDeploymentMessage
        {
            public const string BYTE_CODE = "0x6080604052600160005534801561001557600080fd5b50610f13806100256000396000f300608060405260043610610099576000357c0100000000000000000000000000000000000000000000000000000000900463ffffffff16806319dcec3d1461009e57806326145698146100cb5780632abda41e146100f4578063517999bc1461011d57806363845ba314610148578063a55001a614610176578063e5207eaa1461019f578063e9159d64146101ca578063f6ee7d9f146101f5575b600080fd5b3480156100aa57600080fd5b506100b3610220565b6040516100c293929190610d31565b60405180910390f35b3480156100d757600080fd5b506100f260048036036100ed9190810190610b5e565b61034b565b005b34801561010057600080fd5b5061011b60048036036101169190810190610b36565b610442565b005b34801561012957600080fd5b50610132610445565b60405161013f9190610d6f565b60405180910390f35b34801561015457600080fd5b5061015d61044b565b60405161016d9493929190610d8a565b60405180910390f35b34801561018257600080fd5b5061019d60048036036101989190810190610b5e565b6106ad565b005b3480156101ab57600080fd5b506101b46106fc565b6040516101c19190610d6f565b60405180910390f35b3480156101d657600080fd5b506101df610702565b6040516101ec9190610d0f565b60405180910390f35b34801561020157600080fd5b5061020a6107a0565b6040516102179190610d6f565b60405180910390f35b6102286107a6565b60008060018360000181815250506040805190810160405280600581526020017f68656c6c6f000000000000000000000000000000000000000000000000000000815250836060018190525060c8836020015160000181815250506040805190810160405280600781526020017f47697261666665000000000000000000000000000000000000000000000000008152508360200151604001819052506014836020015160200151600001818152505061012c836040015160000181815250506040805190810160405280600881526020017f456c657068616e7400000000000000000000000000000000000000000000000081525083604001516040018190525061753083604001516020015160000181815250506005915060069050909192565b8060046000820151816000015560208201518160010160008201518160000155602082015181600101600082015181600001555050604082015181600201908051906020019061039c9291906107dc565b5050506040820151816004016000820151816000015560208201518160010160008201518160000155505060408201518160020190805190602001906103e39291906107dc565b50505060608201518160070190805190602001906104029291906107dc565b509050507fc4948cf046f20c08b2b7f5b0b6de7bdbe767d009d512c8440b98eb424bdb9ad83382604051610437929190610cdf565b60405180910390a150565b50565b60005481565b600480600001549080600101606060405190810160405290816000820154815260200160018201602060405190810160405290816000820154815250508152602001600282018054600181600116156101000203166002900480601f0160208091040260200160405190810160405280929190818152602001828054600181600116156101000203166002900480156105255780601f106104fa57610100808354040283529160200191610525565b820191906000526020600020905b81548152906001019060200180831161050857829003601f168201915b5050505050815250509080600401606060405190810160405290816000820154815260200160018201602060405190810160405290816000820154815250508152602001600282018054600181600116156101000203166002900480601f0160208091040260200160405190810160405280929190818152602001828054600181600116156101000203166002900480156106015780601f106105d657610100808354040283529160200191610601565b820191906000526020600020905b8154815290600101906020018083116105e457829003601f168201915b50505050508152505090806007018054600181600116156101000203166002900480601f0160208091040260200160405190810160405280929190818152602001828054600181600116156101000203166002900480156106a35780601f10610678576101008083540402835291602001916106a3565b820191906000526020600020905b81548152906001019060200180831161068657829003601f168201915b5050505050905084565b806000015160008190555080602001516000015160018190555080604001516020015160000151600281905550806040015160400151600390805190602001906106f892919061085c565b5050565b60025481565b60038054600181600116156101000203166002900480601f0160208091040260200160405190810160405280929190818152602001828054600181600116156101000203166002900480156107985780601f1061076d57610100808354040283529160200191610798565b820191906000526020600020905b81548152906001019060200180831161077b57829003601f168201915b505050505081565b60015481565b61010060405190810160405280600081526020016107c26108dc565b81526020016107cf6108dc565b8152602001606081525090565b828054600181600116156101000203166002900490600052602060002090601f016020900481019282601f1061081d57805160ff191683800117855561084b565b8280016001018555821561084b579182015b8281111561084a57825182559160200191906001019061082f565b5b5090506108589190610904565b5090565b828054600181600116156101000203166002900490600052602060002090601f016020900481019282601f1061089d57805160ff19168380011785556108cb565b828001600101855582156108cb579182015b828111156108ca5782518255916020019190600101906108af565b5b5090506108d89190610904565b5090565b606060405190810160405280600081526020016108f7610929565b8152602001606081525090565b61092691905b8082111561092257600081600090555060010161090a565b5090565b90565b602060405190810160405280600081525090565b600082601f830112151561095057600080fd5b813561096361095e82610e11565b610de4565b9150808252602083016020830185838301111561097f57600080fd5b61098a838284610e86565b50505092915050565b60008082840312156109a457600080fd5b6109ae6000610de4565b905092915050565b6000606082840312156109c857600080fd5b6109d26060610de4565b905060006109e284828501610b22565b60008301525060206109f684828501610a2e565b602083015250604082013567ffffffffffffffff811115610a1657600080fd5b610a228482850161093d565b60408301525092915050565b600060208284031215610a4057600080fd5b610a4a6020610de4565b90506000610a5a84828501610b22565b60008301525092915050565b600060808284031215610a7857600080fd5b610a826080610de4565b90506000610a9284828501610b22565b600083015250602082013567ffffffffffffffff811115610ab257600080fd5b610abe848285016109b6565b602083015250604082013567ffffffffffffffff811115610ade57600080fd5b610aea848285016109b6565b604083015250606082013567ffffffffffffffff811115610b0a57600080fd5b610b168482850161093d565b60608301525092915050565b6000610b2e8235610e7c565b905092915050565b6000808284031215610b4757600080fd5b6000610b5584828501610993565b91505092915050565b600060208284031215610b7057600080fd5b600082013567ffffffffffffffff811115610b8a57600080fd5b610b9684828501610a66565b91505092915050565b610ba881610e48565b82525050565b610bb781610e68565b82525050565b6000610bc882610e3d565b808452610bdc816020860160208601610e95565b610be581610ec8565b602085010191505092915050565b6000606083016000830151610c0b6000860182610cd0565b506020830151610c1e6020860182610c43565b5060408301518482036040860152610c368282610bbd565b9150508091505092915050565b602082016000820151610c596000850182610cd0565b50505050565b6000608083016000830151610c776000860182610cd0565b5060208301518482036020860152610c8f8282610bf3565b91505060408301518482036040860152610ca98282610bf3565b91505060608301518482036060860152610cc38282610bbd565b9150508091505092915050565b610cd981610e72565b82525050565b6000604082019050610cf46000830185610b9f565b8181036020830152610d068184610c5f565b90509392505050565b60006020820190508181036000830152610d298184610bbd565b905092915050565b60006060820190508181036000830152610d4b8186610c5f565b9050610d5a6020830185610bae565b610d676040830184610bae565b949350505050565b6000602082019050610d846000830184610cd0565b92915050565b6000608082019050610d9f6000830187610cd0565b8181036020830152610db18186610bf3565b90508181036040830152610dc58185610bf3565b90508181036060830152610dd98184610bbd565b905095945050505050565b6000604051905081810181811067ffffffffffffffff82111715610e0757600080fd5b8060405250919050565b600067ffffffffffffffff821115610e2857600080fd5b601f19601f8301169050602081019050919050565b600081519050919050565b600073ffffffffffffffffffffffffffffffffffffffff82169050919050565b6000819050919050565b6000819050919050565b6000819050919050565b82818337600083830152505050565b60005b83811015610eb3578082015181840152602081019050610e98565b83811115610ec2576000848401525b50505050565b6000601f19601f83011690509190505600a265627a7a72305820f4f39c18fc9b29d959a60b949d1f570d711338b96f5a36deade6e22fcb78813c6c6578706572696d656e74616cf50037";

            public TestContractDeployment() : base(BYTE_CODE)
            {
            }
        }

        public static async Task SuperFunSampleStringAsync()
        {

            var address = "0x12890d2cce102216644c59daE5baed380d84830c";
            var privateKey = "0xb5b1870957d373ef0eeffecc6e4812c0fd08f554b37b233526acc331bf1544f7";
            var web3 = new Web3.Web3(new Account(privateKey));
            var deploymentReceipt = await web3.Eth.GetContractDeploymentHandler<TestContractDeployment>()
                .SendRequestAndWaitForReceiptAsync();
            
            var functionTest = new TestFunction();
            var input = new TestStructStrings()
            {
                Id = 1,
                Id2 = "hello",
                SubStruct1 = new SubStructUintString()
                {
                    Id = 200,
                    Id2 = "Giraffe",
                    Sub = new SubStructUInt()
                    {
                        Id = 20
                    }
                },
                SubStruct2 = new SubStructUintString()
                {
                    Id = 300,
                    Id2 = "Elephant",
                    Sub = new SubStructUInt()
                    {
                        Id = 30000
                    }
                },
            };

            functionTest.TestStruct = input;

            var contractHandler = web3.Eth.GetContractHandler(deploymentReceipt.ContractAddress);

            var id1Before = await contractHandler.QueryAsync<Id1Function, BigInteger>();

            var receiptTransaction = await contractHandler.SendRequestAndWaitForReceiptAsync(functionTest);

            var id1After = await contractHandler.QueryAsync<Id1Function, BigInteger>();
            var id2After = await contractHandler.QueryAsync<Id2Function, BigInteger>();
            var id3After = await contractHandler.QueryAsync<Id3Function, BigInteger>();
            var id4After = await contractHandler.QueryAsync<Id4Function, string>();

            var testDataFromContract = await contractHandler.QueryDeserializingToObjectAsync<GetTestFunction, GetTestFunctionOuptputDTO>();

            var functionStorage = new SetStorageStructFunction {TestStruct = input};
            var receiptSending = await contractHandler.SendRequestAndWaitForReceiptAsync(functionStorage);

            var storageData =  await contractHandler.QueryDeserializingToObjectAsync<GetTestStructStorageFunction, TestStructStrings>();

            var eventStorage = contractHandler.GetEvent<TestStructStorageChangedEvent>();
            var eventOutputs = eventStorage.DecodeAllEventsForEvent(receiptSending.Logs);
        }

    }
}