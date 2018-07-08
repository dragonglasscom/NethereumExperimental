﻿using System;
using System.Collections;
using System.Numerics;
using System.Reflection;
using System.Text;
using Nethereum.JsonRpc.Client;
using Nethereum.RPC.Tests.Testers;

namespace Nethereum.RPC.Sample
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var client = new RpcClient(new Uri("http://localhost:8545/"));

            var testerType = typeof (IRPCRequestTester);
            var assembly = testerType.GetTypeInfo().Assembly;
            var types = assembly.GetTypes();

            foreach (var type in types)
            {
                if (typeof (IRPCRequestTester).IsAssignableFrom(type) && type != typeof (IRPCRequestTester))
                {
                    var tester = (IRPCRequestTester) Activator.CreateInstance(type);
                    try
                    {
                        var testerResult = tester.ExecuteTestAsync(client).Result;

                        ConsoleOutputResult(testerResult, tester.GetRequestType());
                    }
                    catch (Exception ex)
                    {
                        ConsoleOutputResult("Error:" + ex.InnerException.Message, tester.GetRequestType());
                    }
                }
            }

            Console.ReadLine();
        }

        public static void ConsoleOutputResult(object result, Type methodType)
        {
            Console.WriteLine();
            Console.Write(methodType.ToString());
            Console.WriteLine();
            Console.Write("------------------------");
            Console.WriteLine();
            if (result != null)
            {
                var array = result as IEnumerable;
                if (array != null && !(result is string))
                {
                    foreach (var item in array)
                    {
                        Console.WriteLine(ConvertToString(item));
                    }
                }
                else
                {
                    Console.Write(ConvertToString(result));
                }
            }
            Console.WriteLine();
        }

        public static string ConvertToString(object input)
        {
            if (IsSimpleType(input.GetType())) return input.ToString();

            var props = input.GetType().GetProperties();
            if (props.Length > 0)
            {
                var sb = new StringBuilder();
                foreach (var p in props)
                {
                    sb.AppendLine(p.Name + ": " + p.GetValue(input, null));
                }
                return sb.ToString();
            }
            return input.ToString();
        }

        public static bool IsSimpleType(Type type)
        {
            return type.GetTypeInfo().IsPrimitive || type.GetTypeInfo().IsEnum || type == typeof (string) ||
                   type == typeof (decimal) || type == typeof (BigInteger);
        }
    }
}