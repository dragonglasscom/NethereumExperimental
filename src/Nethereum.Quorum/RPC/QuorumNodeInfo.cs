﻿using Nethereum.JsonRpc.Client;
using Nethereum.Quorum.RPC.DTOs;
using Nethereum.RPC.Infrastructure;

namespace Nethereum.Quorum.RPC
{
    public class QuorumNodeInfo : GenericRpcRequestResponseHandlerNoParam<NodeInfo>
    {
        public QuorumNodeInfo(IClient client) : base(client, ApiMethods.quorum_nodeInfo.ToString())
        {
        }
    }
}