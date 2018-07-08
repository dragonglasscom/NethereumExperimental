using Nethereum.Signer;
using Transaction = Nethereum.Signer.Transaction;
using Nethereum.RPC.Eth.DTOs;
using Nethereum.RPC.Eth.Transactions;
using Nethereum.Hex.HexTypes;
using System.Collections;
using System;
using UnityEngine;

namespace Nethereum.JsonRpc.UnityClient
{
 
    public class TransactionSignedUnityRequest:UnityRequest<string>
    {
        private string _url;
        private readonly string _privateKey;
        private readonly string _account;
        private readonly TransactionSigner _transactionSigner;
        private readonly EthGetTransactionCountUnityRequest _transactionCountRequest;
        private readonly EthSendRawTransactionUnityRequest _ethSendTransactionRequest;

        public TransactionSignedUnityRequest(string url, string privateKey, string account)
        {
            _url = url;
            _account = account;
            _privateKey = privateKey;
            _transactionSigner = new TransactionSigner(); 
            _ethSendTransactionRequest = new EthSendRawTransactionUnityRequest(_url);
            _transactionCountRequest = new EthGetTransactionCountUnityRequest(_url);
        }

        public IEnumerator SignAndSendTransaction(TransactionInput transactionInput)
        {
            if (transactionInput == null) throw new ArgumentNullException("transactionInput");

            var nonce = transactionInput.Nonce;
            
            if (nonce == null)
            {   
                yield return _transactionCountRequest.SendRequest(_account, Nethereum.RPC.Eth.DTOs.BlockParameter.CreateLatest());
            
                if(_transactionCountRequest.Exception == null) 
                {
                    nonce =  _transactionCountRequest.Result;
                }
                else
                {
                    this.Exception = _transactionCountRequest.Exception;
                    yield break;
                }
            }

            var gasPrice = transactionInput.GasPrice;
            if (gasPrice == null)
                gasPrice = new HexBigInteger(Transaction.DEFAULT_GAS_PRICE);

            var gasLimit = transactionInput.Gas;
            if (gasLimit == null)
                gasLimit = new HexBigInteger(Transaction.DEFAULT_GAS_LIMIT);

            var value = transactionInput.Value;
            if (value == null)
                value = new HexBigInteger(0);

            var signedTransaction = _transactionSigner.SignTransaction(_privateKey, transactionInput.To, value.Value, nonce,
                gasPrice.Value, gasLimit.Value, transactionInput.Data);
            
            
            yield return _ethSendTransactionRequest.SendRequest(signedTransaction);
            
            if(_ethSendTransactionRequest.Exception == null) 
            {
                this.Result = _ethSendTransactionRequest.Result;
            }
            else
            {
                this.Exception = _ethSendTransactionRequest.Exception;
                yield break;
            }
        }
    }
}
