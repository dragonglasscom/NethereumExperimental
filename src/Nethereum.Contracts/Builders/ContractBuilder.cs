﻿using System;
using System.Linq;
using Nethereum.ABI.FunctionEncoding.Attributes;
using Nethereum.ABI.JsonDeserialisation;
using Nethereum.ABI.Model;
using Nethereum.RPC.Eth.DTOs;

namespace Nethereum.Contracts
{
    public class ContractBuilder
    {
        public ContractBuilder(string abi, string contractAddress)
        {
            ContractABI = new ABIDeserialiser().DeserialiseContract(abi);
            Address = contractAddress;
        }

        public ContractBuilder(Type contractMessageType, string contractAddress)
        {
            var abiExtractor = new AttributesToABIExtractor();
            ContractABI = abiExtractor.ExtractContractABI(contractMessageType);
            Address = contractAddress;
        }

        public ContractBuilder(Type[] contractMessagesTypes, string contractAddress)
        {
            var abiExtractor = new AttributesToABIExtractor();
            ContractABI = abiExtractor.ExtractContractABI(contractMessagesTypes);
            Address = contractAddress;
        }

        public ContractABI ContractABI { get; set; }

        public BlockParameter DefaultBlock { get; set; }

        public string Address { get; set; }

        public NewFilterInput GetDefaultFilterInput(BlockParameter fromBlock = null, BlockParameter toBlock = null)
        {
            var ethFilterInput = new NewFilterInput
            {
                FromBlock = fromBlock,
                ToBlock = toBlock ?? BlockParameter.CreateLatest(),
                Address = new[] {Address}
            };
            return ethFilterInput;
        }

        public EventBuilder GetEventBuilder(string name)
        {
            return new EventBuilder(this, GetEventAbi(name));
        }

        public FunctionBuilder<TFunction> GetFunctionBuilder<TFunction>()
        {
            var function = FunctionAttribute.GetAttribute<TFunction>();
            if (function == null) throw new Exception("Invalid TFunction required a Function Attribute");
            return new FunctionBuilder<TFunction>(this, GetFunctionAbi(function.Name));
        }

        public FunctionBuilder GetFunctionBuilder(string name)
        {
            return new FunctionBuilder(this, GetFunctionAbi(name));
        }

        private EventABI GetEventAbi(string name)
        {
            if (ContractABI == null) throw new Exception("Contract abi not initialised");
            var eventAbi = ContractABI.Events.FirstOrDefault(x => x.Name == name);
            if (eventAbi == null) throw new Exception("Event not found");
            return eventAbi;
        }

        private FunctionABI GetFunctionAbi(string name)
        {
            if (ContractABI == null) throw new Exception("Contract abi not initialised");
            var functionAbi = ContractABI.Functions.FirstOrDefault(x => x.Name == name);
            if (functionAbi == null) throw new Exception("Function not found:" + name);
            return functionAbi;
        }
    }
}