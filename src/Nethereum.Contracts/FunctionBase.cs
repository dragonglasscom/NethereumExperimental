using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Nethereum.ABI.FunctionEncoding;
using Nethereum.Hex.HexConvertors.Extensions;
using Nethereum.Hex.HexTypes;
using Nethereum.RPC.Eth.DTOs;
using Nethereum.RPC.Eth.Transactions;
using Nethereum.RPC.TransactionManagers;

namespace Nethereum.Contracts
{
    public abstract class FunctionBase
    {
        private readonly Contract _contract;
        protected FunctionBuilderBase FunctionBuilderBase { get; set; }
        public BlockParameter DefaultBlock => _contract.DefaultBlock;

        private EthCall EthCall => _contract.Eth.Transactions.Call;
        protected ITransactionManager TransactionManager => _contract.Eth.TransactionManager;

        public string ContractAddress => _contract.Address;

        protected FunctionBase(Contract contract, FunctionBuilderBase functionBuilder)
        {
            FunctionBuilderBase = functionBuilder;
            _contract = contract;
        }

        public Task<string> SendTransactionAsync(string from, HexBigInteger gas,
            HexBigInteger value)
        {
            return SendTransactionAsync(FunctionBuilderBase.CreateTransactionInput(from, gas, value));
        }

        protected Task<string> SendTransactionAsync(TransactionInput transactionInput)
        {
            return TransactionManager.SendTransactionAsync(transactionInput);
        }

   

#if !DOTNET35
        protected Task<TransactionReceipt> SendTransactionAndWaitForReceiptAsync(TransactionInput transactionInput,
            CancellationTokenSource receiptRequestCancellationToken = null)
        {
            return TransactionManager.TransactionReceiptService.SendRequestAndWaitForReceiptAsync(transactionInput,
                receiptRequestCancellationToken);
        }
#endif

#if !DOTNET35

        protected async Task<byte[]> CallRawAsync(CallInput callInput)
        {
            var result =
                await
                    EthCall.SendRequestAsync(callInput, DefaultBlock)
                        .ConfigureAwait(false);


            return result.HexToByteArray();
        }

        protected async Task<byte[]> CallRawAsync(CallInput callInput, BlockParameter block)
        {
            var result =
                await
                    EthCall.SendRequestAsync(callInput, block)
                        .ConfigureAwait(false);

            return result.HexToByteArray();
        }

        protected async Task<TReturn> CallAsync<TReturn>(CallInput callInput)
        {
            var result =
                await
                    EthCall.SendRequestAsync(callInput, DefaultBlock)
                        .ConfigureAwait(false);


            return FunctionBuilderBase.DecodeSimpleTypeOutput<TReturn>(result);
        }

        protected async Task<TReturn> CallAsync<TReturn>(CallInput callInput, BlockParameter block)
        {
            var result =
                await
                    EthCall.SendRequestAsync(callInput, block)
                        .ConfigureAwait(false);

            return FunctionBuilderBase.DecodeSimpleTypeOutput<TReturn>(result);
        }

        protected async Task<TReturn> CallAsync<TReturn>(TReturn functionOuput, CallInput callInput)
        {
            var result =
                await
                    EthCall.SendRequestAsync(callInput, DefaultBlock)
                        .ConfigureAwait(false);

            return FunctionBuilderBase.DecodeDTOTypeOutput(functionOuput, result);
        }

        protected async Task<TReturn> CallAsync<TReturn>(TReturn functionOuput, CallInput callInput,
            BlockParameter block)
        {
            var result =
                await
                    EthCall.SendRequestAsync(callInput, block)
                        .ConfigureAwait(false);

            return FunctionBuilderBase.DecodeDTOTypeOutput(functionOuput, result);
        }

        protected async Task<HexBigInteger> EstimateGasFromEncAsync(CallInput callInput)
        {
            return
                await
                    TransactionManager.EstimateGasAsync(callInput)
                        .ConfigureAwait(false);
        }
#else
        protected Task<TReturn> CallAsync<TReturn>(CallInput callInput)
        {

           return EthCall.SendRequestAsync(callInput, DefaultBlock).ContinueWith(result =>
           {
               if (result.Exception != null) throw result.Exception;
               return FunctionBuilderBase.DecodeSimpleTypeOutput<TReturn>(result.Result);
           });
        }

        protected Task<TReturn> CallAsync<TReturn>(CallInput callInput, BlockParameter block)
        {
            return EthCall.SendRequestAsync(callInput, block).ContinueWith(result =>
            {
                if (result.Exception != null) throw result.Exception;
                return FunctionBuilderBase.DecodeSimpleTypeOutput<TReturn>(result.Result);
            });
           
        }

        protected  Task<TReturn> CallAsync<TReturn>(TReturn functionOuput, CallInput callInput)
        {
            return EthCall.SendRequestAsync(callInput, DefaultBlock).ContinueWith(result =>
            {
                if (result.Exception != null) throw result.Exception;
                return FunctionBuilderBase.DecodeDTOTypeOutput<TReturn>(functionOuput, result.Result);
            });
        }

        protected  Task<TReturn> CallAsync<TReturn>(TReturn functionOuput, CallInput callInput, BlockParameter block)
        {
            return EthCall.SendRequestAsync(callInput, block).ContinueWith(result =>
            {
                if (result.Exception != null) throw result.Exception;
                return FunctionBuilderBase.DecodeDTOTypeOutput<TReturn>(functionOuput, result.Result);
            });
        }

        protected Task<HexBigInteger> EstimateGasFromEncAsync(CallInput callInput)
        {
            return TransactionManager.EstimateGasAsync(callInput);          
        }
#endif

        public List<ParameterOutput> DecodeInput(string data)
        {
            return FunctionBuilderBase.DecodeInput(data);
        }

        public TReturn DecodeSimpleTypeOutput<TReturn>(string output)
        {
            return FunctionBuilderBase.DecodeSimpleTypeOutput<TReturn>(output);
        }

        public TReturn DecodeDTOTypeOutput<TReturn>(TReturn functionOuput, string output)
        {
            return FunctionBuilderBase.DecodeDTOTypeOutput(functionOuput, output);
        }

        public TReturn DecodeDTOTypeOutput<TReturn>(string output) where TReturn : new()
        {
            return FunctionBuilderBase.DecodeDTOTypeOutput<TReturn>(output);
        }

        public TransactionInput CreateTransactionInput(string from, HexBigInteger gas,
            HexBigInteger value)
        {
            return FunctionBuilderBase.CreateTransactionInput(from, gas, value);
        }
    }
}