﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Nethereum.JsonRpc.Client;
using Nethereum.Quorum.RPC.DTOs;
using Nethereum.RPC.Eth.DTOs;
using Newtonsoft.Json.Linq;

namespace Nethereum.Quorum.RPC.Interceptors
{
    public class PrivateForInterceptor : RequestInterceptor
    {
        private readonly List<string> privateFor;
        private readonly string privateFrom;

        public PrivateForInterceptor(List<string> privateFor, string privateFrom)
        {
            this.privateFor = privateFor;
            this.privateFrom = privateFrom;
        }

        public override async Task<object> InterceptSendRequestAsync<T>(
            Func<RpcRequest, string, Task<T>> interceptedSendRequestAsync, RpcRequest request,
            string route = null)
        {
            if (request.Method == "eth_sendTransaction")
            {
                var transaction = (TransactionInput) request.RawParameters[0];
                var privateTransaction = new PrivateTransactionInput(transaction, privateFor.ToArray(), privateFrom);
                return await interceptedSendRequestAsync(new RpcRequest(request.Id, request.Method, privateTransaction), route).ConfigureAwait(false);
            }
            return await interceptedSendRequestAsync(request, route).ConfigureAwait(false);
        }

        public override async Task<object> InterceptSendRequestAsync<T>(
            Func<string, string, object[], Task<T>> interceptedSendRequestAsync, string method,
            string route = null, params object[] paramList)
        {
            if (method == "eth_sendTransaction")
            {
                var transaction = (TransactionInput) paramList[0];
                var privateTransaction = new PrivateTransactionInput(transaction, privateFor.ToArray(), privateFrom);
                paramList[0] = privateTransaction;
                return await interceptedSendRequestAsync(method, route, paramList).ConfigureAwait(false);
            }

            return await interceptedSendRequestAsync(method, route, paramList).ConfigureAwait(false);
        }

    }
}