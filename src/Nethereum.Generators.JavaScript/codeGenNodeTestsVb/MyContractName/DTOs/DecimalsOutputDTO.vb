Imports System
Imports System.Threading.Tasks
Imports System.Collections.Generic
Imports System.Numerics
Imports Nethereum.Hex.HexTypes
Imports Nethereum.ABI.FunctionEncoding.Attributes
Namespace StandardToken.MyContractName.DTOs

    <[FunctionOutput]>
    Public Class DecimalsOutputDTO
    
        <[Parameter]("uint8", "", 1)>
        Public Property ReturnValue1 As Byte
    
    End Class

End Namespace
