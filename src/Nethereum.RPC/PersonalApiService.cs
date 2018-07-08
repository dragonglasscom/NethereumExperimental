﻿using Nethereum.JsonRpc.Client;
using Nethereum.RPC.Personal;

namespace Nethereum.RPC
{
    public class PersonalApiService : RpcClientWrapper
    {
        public PersonalApiService(IClient client) : base(client)
        {
            ListAccounts = new PersonalListAccounts(client);
            NewAccount = new PersonalNewAccount(client);
            UnlockAccount = new PersonalUnlockAccount(client);
            LockAccount = new PersonalLockAccount(client);
            SignAndSendTransaction = new PersonalSignAndSendTransaction(client);
        }

        public PersonalListAccounts ListAccounts { get; private set; }
        public PersonalNewAccount NewAccount { get; private set; }
        public PersonalUnlockAccount UnlockAccount { get; private set; }
        public PersonalLockAccount LockAccount { get; private set; }
        public PersonalSignAndSendTransaction SignAndSendTransaction { get; private set; }
    }
}