using Nethereum.ABI.FunctionEncoding.Attributes;
using Nethereum.Contracts.CQS;

namespace Nethereum.StandardTokenEIP20.CQS
{
    [Function("balances", "uint256")]
    public class BalancesFunction:ContractMessage
    {
        [Parameter("address", "", 1)]
        public string B {get; set;}
    }
}
