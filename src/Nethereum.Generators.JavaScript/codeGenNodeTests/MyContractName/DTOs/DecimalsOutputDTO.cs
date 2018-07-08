using System;
using System.Threading.Tasks;
using System.Numerics;
using Nethereum.Hex.HexTypes;
using Nethereum.ABI.FunctionEncoding.Attributes;
namespace StandardToken.MyContractName.DTOs
{
    [FunctionOutput]
    public class DecimalsOutputDTO
    {
        [Parameter("uint8", "", 1)]
        public byte B {get; set;}
    }
}
