using System.Threading;
using System.Threading.Tasks;
using Nethereum.Hex.HexTypes;
using Nethereum.RPC.Eth.DTOs;

namespace Nethereum.Contracts
{
    public class Function : FunctionBase
    {
        public Function(Contract contract, FunctionBuilder functionBuilder) : base(contract, functionBuilder)
        {
        }

        protected FunctionBuilder FunctionBuilder => (FunctionBuilder) FunctionBuilderBase;

        public Task<TReturn> CallAsync<TReturn>(params object[] functionInput)
        {
            return base.CallAsync<TReturn>(CreateCallInput(functionInput));
        }

        public Task<TReturn> CallAsync<TReturn>(string from, HexBigInteger gas,
            HexBigInteger value, params object[] functionInput)
        {
            return base.CallAsync<TReturn>(CreateCallInput(from, gas, value, functionInput));
        }

        public Task<TReturn> CallAsync<TReturn>(string from, HexBigInteger gas,
            HexBigInteger value, BlockParameter block, params object[] functionInput)
        {
            return base.CallAsync<TReturn>(CreateCallInput(from, gas, value, functionInput), block);
        }

        public Task<TReturn> CallAsync<TReturn>(BlockParameter block, params object[] functionInput)
        {
            return base.CallAsync<TReturn>(CreateCallInput(functionInput), block);
        }

        public Task<TReturn> CallDeserializingToObjectAsync<TReturn>(params object[] functionInput)
            where TReturn : new()
        {
            return base.CallAsync(new TReturn(), CreateCallInput(functionInput));
        }

        public Task<TReturn> CallDeserializingToObjectAsync<TReturn>(string from, HexBigInteger gas,
            HexBigInteger value, params object[] functionInput) where TReturn : new()
        {
            return base.CallAsync(new TReturn(), CreateCallInput(from, gas, value, functionInput));
        }

        public Task<TReturn> CallDeserializingToObjectAsync<TReturn>(string from, HexBigInteger gas,
            HexBigInteger value, BlockParameter block, params object[] functionInput) where TReturn : new()
        {
            return base.CallAsync(new TReturn(), CreateCallInput(from, gas, value, functionInput), block);
        }

        public Task<TReturn> CallDeserializingToObjectAsync<TReturn>(
            BlockParameter blockParameter, params object[] functionInput) where TReturn : new()
        {
            return base.CallAsync(new TReturn(), CreateCallInput(functionInput), blockParameter);
        }

        public Task<HexBigInteger> EstimateGasAsync(params object[] functionInput)
        {
            return EstimateGasFromEncAsync(CreateCallInput(functionInput));
        }

        public Task<HexBigInteger> EstimateGasAsync(string from, HexBigInteger gas,
            HexBigInteger value, params object[] functionInput)
        {
            return EstimateGasFromEncAsync(CreateCallInput(from, gas, value, functionInput));
        }

        public Task<string> SendTransactionAsync(string from, params object[] functionInput)
        {
            return base.SendTransactionAsync(CreateTransactionInput(from, functionInput));
        }

        public Task<string> SendTransactionAsync(string from, HexBigInteger gas,
            HexBigInteger value, params object[] functionInput)
        {
            return base.SendTransactionAsync(CreateTransactionInput(from, gas, value, functionInput));
        }

        public Task<string> SendTransactionAsync(string from, HexBigInteger gas, HexBigInteger gasPrice,
            HexBigInteger value, params object[] functionInput)
        {
            return base.SendTransactionAsync(CreateTransactionInput(from, gas, gasPrice, value, functionInput));
        }

        public Task<string> SendTransactionAsync(TransactionInput input, params object[] functionInput)
        {
            return base.SendTransactionAsync(CreateTransactionInput(input, functionInput));
        }


        public CallInput CreateCallInput(params object[] functionInput)
        {
            return FunctionBuilder.CreateCallInput(functionInput);
        }

        public CallInput CreateCallInput(string from, HexBigInteger gas,
            HexBigInteger value, params object[] functionInput)
        {
            return FunctionBuilder.CreateCallInput(from, gas, value, functionInput);
        }

        public string GetData(params object[] functionInput)
        {
            return FunctionBuilder.GetData(functionInput);
        }

        public TransactionInput CreateTransactionInput(string from, params object[] functionInput)
        {
            return FunctionBuilder.CreateTransactionInput(from, functionInput);
        }

        public TransactionInput CreateTransactionInput(string from, HexBigInteger gas,
            HexBigInteger value, params object[] functionInput)
        {
            return FunctionBuilder.CreateTransactionInput(from, gas, value, functionInput);
        }

        public TransactionInput CreateTransactionInput(string from, HexBigInteger gas, HexBigInteger gasPrice,
            HexBigInteger value, params object[] functionInput)
        {
            return FunctionBuilder.CreateTransactionInput(from, gas, gasPrice, value, functionInput);
        }

        public TransactionInput CreateTransactionInput(TransactionInput input, params object[] functionInput)
        {
            return FunctionBuilder.CreateTransactionInput(input, functionInput);
        }

#if !DOTNET35
        public Task<TransactionReceipt> SendTransactionAndWaitForReceiptAsync(string from,
            CancellationTokenSource receiptRequestCancellationToken = null, params object[] functionInput)
        {
            return base.SendTransactionAndWaitForReceiptAsync(CreateTransactionInput(from, functionInput),
                receiptRequestCancellationToken);
        }

        public Task<TransactionReceipt> SendTransactionAndWaitForReceiptAsync(string from, HexBigInteger gas,
            HexBigInteger value, CancellationTokenSource receiptRequestCancellationToken = null,
            params object[] functionInput)
        {
            return base.SendTransactionAndWaitForReceiptAsync(CreateTransactionInput(from, gas, value, functionInput),
                receiptRequestCancellationToken);
        }

        public Task<TransactionReceipt> SendTransactionAndWaitForReceiptAsync(string from, HexBigInteger gas,
            HexBigInteger gasPrice,
            HexBigInteger value, CancellationTokenSource receiptRequestCancellationToken = null,
            params object[] functionInput)
        {
            return base.SendTransactionAndWaitForReceiptAsync(
                CreateTransactionInput(from, gas, gasPrice, value, functionInput), receiptRequestCancellationToken);
        }

        public Task<TransactionReceipt> SendTransactionAndWaitForReceiptAsync(TransactionInput input,
            CancellationTokenSource receiptRequestCancellationToken = null, params object[] functionInput)
        {
            return base.SendTransactionAndWaitForReceiptAsync(CreateTransactionInput(input, functionInput),
                receiptRequestCancellationToken);
        }
#endif
    }

    public class Function<TFunctionInput> : FunctionBase
    {
        public Function(Contract contract, FunctionBuilder<TFunctionInput> functionBuilder)
            : base(contract, functionBuilder)
        {
        }

        protected FunctionBuilder<TFunctionInput> FunctionBuilder =>
            (FunctionBuilder<TFunctionInput>) FunctionBuilderBase;


        public CallInput CreateCallInputParameterless()
        {
            return FunctionBuilder.CreateCallInputParameterless();
        }

        public CallInput CreateCallInput(TFunctionInput functionInput)
        {
            return FunctionBuilder.CreateCallInput(functionInput);
        }

        public CallInput CreateCallInput(TFunctionInput functionInput, string from, HexBigInteger gas,
            HexBigInteger value)
        {
            return FunctionBuilder.CreateCallInput(functionInput, from, gas, value);
        }

        public string GetData(TFunctionInput functionInput)
        {
            return FunctionBuilder.GetData(functionInput);
        }

        public TFunctionInput DecodeFunctionInput(TFunctionInput functionInput, TransactionInput transactionInput)
        {
            return FunctionBuilder.DecodeFunctionInput(functionInput, transactionInput);
        }

        public TFunctionInput DecodeFunctionInput(TFunctionInput functionInput, string data)
        {
            return FunctionBuilder.DecodeFunctionInput(functionInput, data);
        }

        public TransactionInput CreateTransactionInput(TFunctionInput functionInput, string from)
        {
            return FunctionBuilder.CreateTransactionInput(functionInput, from);
        }

        public TransactionInput CreateTransactionInput(TFunctionInput functionInput, string from, HexBigInteger gas,
            HexBigInteger value)
        {
            return FunctionBuilder.CreateTransactionInput(functionInput, from, gas, value);
        }

        public TransactionInput CreateTransactionInput(TFunctionInput functionInput, string from, HexBigInteger gas,
            HexBigInteger gasPrice, HexBigInteger value)
        {
            return FunctionBuilder.CreateTransactionInput(functionInput, from, gas, gasPrice, value);
        }

       

        public Task<TReturn> CallAsync<TReturn>()
        {
            return base.CallAsync<TReturn>(CreateCallInputParameterless());
        }

        public Task<TReturn> CallAsync<TReturn>(TFunctionInput functionInput)
        {
            return base.CallAsync<TReturn>(CreateCallInput(functionInput));
        }


        public Task<TReturn> CallAsync<TReturn>(TFunctionInput functionInput, string from, HexBigInteger gas,
            HexBigInteger value)
        {
            return base.CallAsync<TReturn>(CreateCallInput(functionInput, from, gas, value));
        }

        public Task<TReturn> CallAsync<TReturn>(TFunctionInput functionInput, string from, HexBigInteger gas,
            HexBigInteger value, BlockParameter block)
        {
            return base.CallAsync<TReturn>(CreateCallInput(functionInput, from, gas, value), block);
        }

        public Task<TReturn> CallAsync<TReturn>(TFunctionInput functionInput,
            BlockParameter blockParameter)
        {
            return base.CallAsync<TReturn>(CreateCallInput(functionInput), blockParameter);
        }

        public Task<TReturn> CallDeserializingToObjectAsync<TReturn>() where TReturn : new()
        {
            return base.CallAsync(new TReturn(), CreateCallInputParameterless());
        }

        public Task<TReturn> CallDeserializingToObjectAsync<TReturn>(BlockParameter block) where TReturn : new()
        {
            return base.CallAsync(new TReturn(), CreateCallInputParameterless(), block);
        }

        public Task<TReturn> CallDeserializingToObjectAsync<TReturn>(TFunctionInput functionInput) where TReturn : new()
        {
            return base.CallAsync(new TReturn(), CreateCallInput(functionInput));
        }

        public Task<TReturn> CallDeserializingToObjectAsync<TReturn>(TFunctionInput functionInput, BlockParameter block)
            where TReturn : new()
        {
            return base.CallAsync(new TReturn(), CreateCallInput(functionInput), block);
        }

        public Task<TReturn> CallDeserializingToObjectAsync<TReturn>(TFunctionInput functionInput, string from,
            HexBigInteger gas,
            HexBigInteger value) where TReturn : new()
        {
            return base.CallAsync(new TReturn(), CreateCallInput(functionInput, from, gas, value));
        }

        public Task<TReturn> CallDeserializingToObjectAsync<TReturn>(TFunctionInput functionInput, string from,
            HexBigInteger gas,
            HexBigInteger value, BlockParameter block) where TReturn : new()
        {
            return base.CallAsync(new TReturn(), CreateCallInput(functionInput, from, gas, value), block);
        }

        public Task<HexBigInteger> EstimateGasAsync()
        {
            return EstimateGasFromEncAsync(CreateCallInputParameterless());
        }

        public Task<HexBigInteger> EstimateGasAsync(TFunctionInput functionInput)
        {
            return EstimateGasFromEncAsync(CreateCallInput(functionInput));
        }

        public Task<HexBigInteger> EstimateGasAsync(TFunctionInput functionInput, string from, HexBigInteger gas,
            HexBigInteger value)
        {
            return EstimateGasFromEncAsync(CreateCallInput(functionInput, from, gas, value));
        }

        public Task<HexBigInteger> EstimateGasAsync(TFunctionInput functionInput,
            CallInput callInput)
        {
            var encodedInput = GetData(functionInput);
            callInput.Data = encodedInput;
            return EstimateGasFromEncAsync(callInput);
        }


        public Task<string> SendTransactionAsync(TFunctionInput functionInput, string from)
        {
            return base.SendTransactionAsync(CreateTransactionInput(functionInput, from));
        }

        public Task<string> SendTransactionAsync(TFunctionInput functionInput, string from, HexBigInteger gas,
            HexBigInteger value)
        {
            return base.SendTransactionAsync(CreateTransactionInput(functionInput, from, gas, value));
        }

        public Task<string> SendTransactionAsync(TFunctionInput functionInput, string from, HexBigInteger gas,
            HexBigInteger gasPrice,
            HexBigInteger value)
        {
            return base.SendTransactionAsync(CreateTransactionInput(functionInput, from, gas, gasPrice, value));
        }

        public Task<string> SendTransactionAsync(TFunctionInput functionInput,
            TransactionInput input)
        {
            var encodedInput = GetData(functionInput);
            input.Data = encodedInput;
            return base.SendTransactionAsync(input);
        }

#if !DOTNET35

        public Task<byte[]> CallRawAsync<TReturn>()
        {
            return base.CallRawAsync(CreateCallInputParameterless());
        }

        public Task<byte[]> CallRawAsync<TReturn>(TFunctionInput functionInput)
        {
            return base.CallRawAsync(CreateCallInput(functionInput));
        }

        public Task<byte[]> CallRawAsync(TFunctionInput functionInput, string from, HexBigInteger gas,
            HexBigInteger value)
        {
            return base.CallRawAsync(CreateCallInput(functionInput, from, gas, value));
        }

        public Task<byte[]> CallRawAsync(TFunctionInput functionInput, string from, HexBigInteger gas,
            HexBigInteger value, BlockParameter block)
        {
            return base.CallRawAsync(CreateCallInput(functionInput, from, gas, value), block);
        }

        public Task<byte[]> CallRawAsync(TFunctionInput functionInput,
            BlockParameter blockParameter)
        {
            return base.CallRawAsync(CreateCallInput(functionInput), blockParameter);
        }


        public Task<TransactionReceipt> SendTransactionAndWaitForReceiptAsync(TFunctionInput functionInput, string from,
            CancellationTokenSource receiptRequestCancellationToken = null)
        {
            return base.SendTransactionAndWaitForReceiptAsync(CreateTransactionInput(functionInput, from),
                receiptRequestCancellationToken);
        }

        public Task<TransactionReceipt> SendTransactionAndWaitForReceiptAsync(TFunctionInput functionInput, string from,
            HexBigInteger gas,
            HexBigInteger value, CancellationTokenSource receiptRequestCancellationToken = null)
        {
            return base.SendTransactionAndWaitForReceiptAsync(CreateTransactionInput(functionInput, from, gas, value),
                receiptRequestCancellationToken);
        }

        public Task<TransactionReceipt> SendTransactionAndWaitForReceiptAsync(TFunctionInput functionInput, string from,
            HexBigInteger gas, HexBigInteger gasPrice,
            HexBigInteger value, CancellationTokenSource receiptRequestCancellationToken = null)
        {
            return base.SendTransactionAndWaitForReceiptAsync(
                CreateTransactionInput(functionInput, from, gas, gasPrice, value), receiptRequestCancellationToken);
        }

        public Task<TransactionReceipt> SendTransactionAndWaitForReceiptAsync(TFunctionInput functionInput,
            TransactionInput input, CancellationTokenSource receiptRequestCancellationToken = null)
        {
            var encodedInput = GetData(functionInput);
            input.Data = encodedInput;
            return base.SendTransactionAndWaitForReceiptAsync(input, receiptRequestCancellationToken);
        }
#endif
    }
}